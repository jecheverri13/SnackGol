using LibraryConnection.ControllerAzure;
using LibraryConnection.Context;
using LibraryConnection.Dtos;
using LibraryConnection.DbSet;
using LibraryEntities.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Text.Json;
using MSSnackGol.Services;

namespace MSSnackGol.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderManagementController : ControllerBase
    {
        private readonly ILogger<OrderManagementController> _logger;
        private readonly IQRGeneratorService _qrService;
        private static readonly JsonSerializerOptions PickupJsonOptions = new(JsonSerializerDefaults.Web)
        {
            PropertyNameCaseInsensitive = true
        };

        private const string StatusConfirmed = "Confirmed";
        private const string StatusPreparing = "Preparing";
        private const string StatusReadyForPickup = "ReadyForPickup";
        private const string StatusDelivered = "Delivered";

        public OrderManagementController(ILogger<OrderManagementController> logger, IQRGeneratorService qrService)
        {
            _logger = logger;
            _qrService = qrService;
        }

        [HttpPost("Create")]
        public IActionResult CreateOrder([FromBody] OrderRequest orderRequest)
        {
            try
            {
                var oResponse = OrderController.CreateOrder(orderRequest);
                return StatusCode((int)oResponse.status, oResponse);
            }
            catch (Exception ex)
            {
                const string errorMessage = "Error al crear la orden";
                _logger.LogError(ex, "{ErrorMessage}", errorMessage);
                return StatusCode(
                    (int)HttpStatusCode.InternalServerError,
                    new Response<dynamic>(false, HttpStatusCode.InternalServerError, errorMessage, ex.Message)
                );
            }
        }

        [HttpPatch]
        public IActionResult UpdateOrder([FromBody] OrderRequest order)
        {
            try
            {
                var oResponse = new Response<OrderResponse>();
                oResponse = OrderController.UpdateOrder(order);
                return StatusCode((int)oResponse.status, oResponse);
            }
            catch (Exception ex)
            {
                const string errorMessage = "Error al actualizar la orden";
                _logger.LogError(ex, "{ErrorMessage}", errorMessage);
                return StatusCode(
                    (int)HttpStatusCode.InternalServerError,
                    new Response<dynamic>(false, HttpStatusCode.InternalServerError, errorMessage, ex.Message)
                );
            }
        }

        /// <summary>
        /// Obtiene una orden específica por su ID
        /// </summary>
        [HttpGet("{orderId}")]
        public IActionResult GetOrderById(string orderId)
        {
            try
            {
                var oResponse = OrderController.GetOrderById(orderId);
                return StatusCode((int)oResponse.status, oResponse);
            }
            catch (Exception ex)
            {
                const string errorMessage = "Error al obtener las órdenes";
                _logger.LogError(ex, "{ErrorMessage}", errorMessage);
                return StatusCode(
                    (int)HttpStatusCode.InternalServerError,
                    new Response<dynamic>(false, HttpStatusCode.InternalServerError, errorMessage, ex.Message)
                );
            }
        }
        [HttpGet("Customer/{customerId}")]
        public IActionResult GetOrdersByCustomer(string customerId)
        {
            try
            {
                var oResponse = OrderController.GetCustomerOrders(customerId);
                return StatusCode((int)oResponse.status, oResponse);
            }
            catch (Exception ex)
            {
                string errorMessage = $"Error al obtener las órdenes del cliente {customerId}";
                _logger.LogError(ex, "{ErrorMessage}", errorMessage);
                return StatusCode(
                    (int)HttpStatusCode.InternalServerError,
                    new Response<dynamic>(false, HttpStatusCode.InternalServerError, errorMessage, ex.Message)
                );
            }
        }

        /// <summary>
        /// Elimina todos los pedidos (solo para desarrollo/testing).
        /// </summary>
        [HttpDelete("All")]
        public IActionResult DeleteAllOrders()
        {
            try
            {
                using var db = new ApplicationDbContext();
                
                // Cargar pedidos con sus líneas
                var orders = db.orders.Include(o => o.OrderLines).ToList();
                var lineCount = orders.Sum(o => o.OrderLines?.Count ?? 0);
                
                // Eliminar pedidos (las líneas se eliminan en cascada si está configurado)
                db.orders.RemoveRange(orders);
                db.SaveChanges();
                
                _logger.LogWarning("Se eliminaron {OrderCount} pedidos y {LineCount} líneas de pedido", 
                    orders.Count, lineCount);
                
                return StatusCode((int)HttpStatusCode.OK, 
                    new Response<dynamic>(true, HttpStatusCode.OK, new
                    {
                        message = "Todos los pedidos han sido eliminados",
                        ordersDeleted = orders.Count,
                        linesDeleted = lineCount
                    }));
            }
            catch (Exception ex)
            {
                const string errorMessage = "Error al eliminar los pedidos";
                _logger.LogError(ex, "{ErrorMessage}", errorMessage);
                return StatusCode((int)HttpStatusCode.InternalServerError, 
                    new Response<dynamic>(false, HttpStatusCode.InternalServerError, errorMessage, ex.Message));
            }
        }

        /// <summary>
        /// Obtiene los pedidos asociados a un token de sesión (para usuarios no autenticados).
        /// </summary>
        [HttpGet("Session/{sessionToken}")]
        public IActionResult GetOrdersBySession(string sessionToken)
        {
            if (string.IsNullOrWhiteSpace(sessionToken))
            {
                return StatusCode((int)HttpStatusCode.BadRequest, 
                    new Response<dynamic>(false, HttpStatusCode.BadRequest, "Token de sesión requerido"));
            }

            try
            {
                using var db = new ApplicationDbContext();
                
                var ordersRaw = db.orders
                    .Include(o => o.OrderLines)
                    .Where(o => o.customer_id == sessionToken)
                    .OrderByDescending(o => o.order_date)
                    .ToList();

                var orders = ordersRaw.Select(o => new
                {
                    orderId = o.order_id,
                    status = o.status ?? StatusConfirmed,
                    total = o.total_net_price,
                    orderDate = o.order_date,
                    pickupCode = o.pickup_code,
                    itemCount = o.OrderLines?.Sum(ol => (int)ol.quantity) ?? 0,
                    items = (o.OrderLines ?? Enumerable.Empty<OrderLine>()).Select(ol => new
                    {
                        productName = ol.description ?? "Producto",
                        quantity = (int)ol.quantity,
                        unitPrice = ol.quantity > 0 ? ol.net_price / ol.quantity : ol.net_price,
                        subtotal = ol.net_price
                    }).ToList()
                }).ToList();

                if (!orders.Any())
                {
                    return StatusCode((int)HttpStatusCode.NotFound, 
                        new Response<dynamic>(false, HttpStatusCode.NotFound, "No se encontraron pedidos para esta sesión"));
                }

                return StatusCode((int)HttpStatusCode.OK, 
                    new Response<dynamic>(true, HttpStatusCode.OK, orders));
            }
            catch (Exception ex)
            {
                const string errorMessage = "Error al obtener los pedidos de la sesión";
                _logger.LogError(ex, "{ErrorMessage}", errorMessage);
                return StatusCode((int)HttpStatusCode.InternalServerError, 
                    new Response<dynamic>(false, HttpStatusCode.InternalServerError, errorMessage, ex.Message));
            }
        }

        /// <summary>
        /// Actualiza el estado de un pedido. 
        /// Estados válidos: Confirmed → Preparing → ReadyForPickup → Delivered
        /// Este endpoint es para uso del staff/administrador.
        /// </summary>
        [HttpPatch("{orderId}/status")]
        public IActionResult UpdateOrderStatus(string orderId, [FromBody] UpdateStatusRequest request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.NewStatus))
            {
                return StatusCode((int)HttpStatusCode.BadRequest, 
                    new Response<dynamic>(false, HttpStatusCode.BadRequest, "Estado requerido"));
            }

            // Validar que el estado sea uno de los permitidos
            var validStatuses = new[] { StatusConfirmed, StatusPreparing, StatusReadyForPickup, StatusDelivered };
            if (!validStatuses.Contains(request.NewStatus, StringComparer.OrdinalIgnoreCase))
            {
                return StatusCode((int)HttpStatusCode.BadRequest, 
                    new Response<dynamic>(false, HttpStatusCode.BadRequest, 
                        $"Estado inválido. Estados permitidos: {string.Join(", ", validStatuses)}"));
            }

            try
            {
                using var db = new ApplicationDbContext();
                var order = db.orders.FirstOrDefault(o => o.order_id == orderId);

                if (order == null)
                {
                    return StatusCode((int)HttpStatusCode.NotFound, 
                        new Response<dynamic>(false, HttpStatusCode.NotFound, "Orden no encontrada"));
                }

                // Validar transición de estado (no se puede retroceder)
                var currentIndex = Array.FindIndex(validStatuses, s => s.Equals(order.status, StringComparison.OrdinalIgnoreCase));
                var newIndex = Array.FindIndex(validStatuses, s => s.Equals(request.NewStatus, StringComparison.OrdinalIgnoreCase));

                if (newIndex < currentIndex && !request.ForceUpdate)
                {
                    return StatusCode((int)HttpStatusCode.BadRequest, 
                        new Response<dynamic>(false, HttpStatusCode.BadRequest, 
                            $"No se puede retroceder el estado de '{order.status}' a '{request.NewStatus}'"));
                }

                var previousStatus = order.status;
                order.status = validStatuses[newIndex]; // Normalizar el estado

                // Si se marca como entregado, registrar la fecha
                if (order.status == StatusDelivered && order.pickup_redeemed_at == null)
                {
                    order.pickup_redeemed_at = DateTime.UtcNow;
                    order.pickup_verified_by = request.UpdatedBy;
                }

                db.SaveChanges();

                _logger.LogInformation("Orden {OrderId} actualizada de '{PreviousStatus}' a '{NewStatus}' por {UpdatedBy}",
                    orderId, previousStatus, order.status, request.UpdatedBy ?? "Sistema");

                return StatusCode((int)HttpStatusCode.OK, 
                    new Response<dynamic>(true, HttpStatusCode.OK, new
                    {
                        orderId = order.order_id,
                        previousStatus,
                        newStatus = order.status,
                        updatedAt = DateTime.UtcNow
                    }));
            }
            catch (Exception ex)
            {
                const string errorMessage = "Error al actualizar el estado del pedido";
                _logger.LogError(ex, "{ErrorMessage}", errorMessage);
                return StatusCode((int)HttpStatusCode.InternalServerError, 
                    new Response<dynamic>(false, HttpStatusCode.InternalServerError, errorMessage, ex.Message));
            }
        }

        /// <summary>
        /// Obtiene todos los pedidos activos (no entregados) para el panel de administración.
        /// </summary>
        [HttpGet("Active")]
        public IActionResult GetActiveOrders()
        {
            try
            {
                using var db = new ApplicationDbContext();
                
                var activeOrders = db.orders
                    .Include(o => o.OrderLines)
                    .Where(o => o.status != StatusDelivered)
                    .OrderBy(o => o.order_date)
                    .Select(o => new
                    {
                        orderId = o.order_id,
                        status = o.status ?? StatusConfirmed,
                        total = o.total_net_price,
                        orderDate = o.order_date,
                        pickupCode = o.pickup_code,
                        itemCount = o.OrderLines != null ? o.OrderLines.Sum(ol => (int)ol.quantity) : 0,
                        items = o.OrderLines != null ? o.OrderLines.Select(ol => new
                        {
                            item = ol.item,
                            description = ol.description,
                            quantity = ol.quantity
                        }).ToList() : null
                    })
                    .ToList();

                return StatusCode((int)HttpStatusCode.OK, 
                    new Response<dynamic>(true, HttpStatusCode.OK, activeOrders));
            }
            catch (Exception ex)
            {
                const string errorMessage = "Error al obtener los pedidos activos";
                _logger.LogError(ex, "{ErrorMessage}", errorMessage);
                return StatusCode((int)HttpStatusCode.InternalServerError, 
                    new Response<dynamic>(false, HttpStatusCode.InternalServerError, errorMessage, ex.Message));
            }
        }
        
        /// <summary>
        /// Realiza el checkout del carrito actual, genera la orden y prepara el QR de retiro.
        /// </summary>
        [HttpPost("Checkout")]
        public IActionResult Checkout([FromBody] LibraryEntities.Models.CheckoutRequest body)
        {
            try
            {
                using var db = new ApplicationDbContext();

                var session = body?.session_token?.Trim();
                long? userId = null;

                if (User?.Identity?.IsAuthenticated == true && !string.IsNullOrWhiteSpace(User.Identity!.Name))
                {
                    if (long.TryParse(User.Identity.Name, NumberStyles.Integer, CultureInfo.InvariantCulture, out var parsedUserId))
                    {
                        userId = parsedUserId;
                    }
                }

                if (userId is null && string.IsNullOrWhiteSpace(session))
                {
                    return StatusCode((int)HttpStatusCode.BadRequest, new Response<dynamic>(false, HttpStatusCode.BadRequest, "No se pudo identificar la sesión del carrito"));
                }

                var cart = db.carts
                    .FirstOrDefault(c => (userId != null && c.user_id == userId) || (!string.IsNullOrWhiteSpace(session) && c.session_token == session));

                if (cart == null)
                {
                    return StatusCode((int)HttpStatusCode.NotFound, new Response<dynamic>(false, HttpStatusCode.NotFound, "Carrito no encontrado"));
                }

                var items = db.cart_items
                    .Include(i => i.Product)
                    .Where(i => i.cart_id == cart.id)
                    .ToList();

                if (items.Count == 0)
                {
                    return StatusCode((int)HttpStatusCode.BadRequest, new Response<dynamic>(false, HttpStatusCode.BadRequest, "Carrito vacío"));
                }

                foreach (var it in items)
                {
                    if (it.Product.stock < it.quantity)
                    {
                        return StatusCode((int)HttpStatusCode.Conflict, new Response<dynamic>(false, HttpStatusCode.Conflict, $"Stock insuficiente para {it.Product.name}"));
                    }
                }

                foreach (var it in items)
                {
                    it.Product.stock -= it.quantity;
                }

                var total = items.Sum(it => it.subtotal);
                var customerId = userId?.ToString(CultureInfo.InvariantCulture) ?? session!;

                EnsureClientRecord(db, customerId, userId);

                var orderId = _qrService.GenerateOrderId();
                var order = new Order
                {
                    order_id = orderId,
                    customer_id = customerId,
                    order_date = DateTime.UtcNow,
                    status = StatusConfirmed,
                    total_gross_amount = total,
                    total_net_price = total,
                    OrderLines = items.Select((it, index) => new OrderLine
                    {
                        lineNum = index + 1,
                        item = it.product_id.ToString(CultureInfo.InvariantCulture),
                        description = it.Product?.name,
                        gross_amount = it.subtotal,
                        net_price = it.subtotal,
                        tax_amount = 0,
                        quantity = it.quantity
                    }).ToList()
                };

                var pickupCode = _qrService.GeneratePickupCode();
                var artifacts = _qrService.GeneratePickupArtifacts(orderId, pickupCode, session, items);

                order.pickup_code = pickupCode;
                order.pickup_token_hash = _qrService.HashToken(artifacts.Token);
                order.pickup_payload_base64 = artifacts.PayloadBase64;
                order.pickup_qr_base64 = artifacts.QrImageBase64;
                order.pickup_generated_at = DateTime.UtcNow;

                db.orders.Add(order);

                db.cart_items.RemoveRange(items);
                cart.updated_at = DateTime.UtcNow;

                db.SaveChanges();

                var pickupInfo = new PickupInfoResponse
                {
                    orderId = orderId,
                    pickupCode = pickupCode,
                    pickupPayloadBase64 = order.pickup_payload_base64,
                    pickupQrImageBase64 = order.pickup_qr_base64,
                    generatedAtUtc = order.pickup_generated_at,
                    status = order.status
                };

                var result = new CheckoutResult
                {
                    orderId = orderId,
                    status = order.status ?? StatusConfirmed,
                    total = total,
                    pickup = pickupInfo
                };

                return StatusCode((int)HttpStatusCode.Created, new Response<dynamic>(true, HttpStatusCode.Created, result));
            }
            catch (Exception ex)
            {
                const string errorMessage = "Error en checkout";
                _logger.LogError(ex, "{ErrorMessage}", errorMessage);
                return StatusCode((int)HttpStatusCode.InternalServerError, new Response<dynamic>(false, HttpStatusCode.InternalServerError, errorMessage, ex.Message));
            }
        }

        /// <summary>
        /// Obtiene la información del QR de entrega para una orden.
        /// </summary>
        [HttpGet("{orderId}/pickup")]
        public IActionResult GetPickup(string orderId)
        {
            try
            {
                using var db = new ApplicationDbContext();
                var order = db.orders.AsNoTracking().FirstOrDefault(o => o.order_id == orderId);

                if (order == null)
                {
                    return StatusCode((int)HttpStatusCode.NotFound, new Response<dynamic>(false, HttpStatusCode.NotFound, "Orden no encontrada"));
                }

                if (string.IsNullOrWhiteSpace(order.pickup_code) || string.IsNullOrWhiteSpace(order.pickup_payload_base64))
                {
                    return StatusCode((int)HttpStatusCode.NotFound, new Response<dynamic>(false, HttpStatusCode.NotFound, "La orden no tiene QR de retiro disponible"));
                }

                var pickupInfo = new PickupInfoResponse
                {
                    orderId = order.order_id,
                    pickupCode = order.pickup_code,
                    pickupPayloadBase64 = order.pickup_payload_base64,
                    pickupQrImageBase64 = order.pickup_qr_base64,
                    generatedAtUtc = order.pickup_generated_at,
                    deliveredAtUtc = order.pickup_redeemed_at,
                    status = order.status
                };

                return StatusCode((int)HttpStatusCode.OK, new Response<dynamic>(true, HttpStatusCode.OK, pickupInfo));
            }
            catch (Exception ex)
            {
                const string errorMessage = "Error al obtener el QR de entrega";
                _logger.LogError(ex, "{ErrorMessage}", errorMessage);
                return StatusCode((int)HttpStatusCode.InternalServerError, new Response<dynamic>(false, HttpStatusCode.InternalServerError, errorMessage, ex.Message));
            }
        }

        /// <summary>
        /// Valida el token escaneado en el punto de entrega y marca la orden como entregada.
        /// </summary>
        [HttpPost("{orderId}/pickup/validate")]
        public IActionResult ValidatePickup(string orderId, [FromBody] PickupValidationRequest request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.token))
            {
                return StatusCode((int)HttpStatusCode.BadRequest, new Response<dynamic>(false, HttpStatusCode.BadRequest, "Token de validación requerido"));
            }

            try
            {
                using var db = new ApplicationDbContext();
                var order = db.orders.FirstOrDefault(o => o.order_id == orderId);

                if (order == null)
                {
                    return StatusCode((int)HttpStatusCode.NotFound, new Response<dynamic>(false, HttpStatusCode.NotFound, "Orden no encontrada"));
                }

                if (string.IsNullOrWhiteSpace(order.pickup_token_hash))
                {
                    return StatusCode((int)HttpStatusCode.NotFound, new Response<dynamic>(false, HttpStatusCode.NotFound, "La orden no tiene un token de retiro activo"));
                }

                if (!_qrService.ValidateToken(request.token!, order.pickup_token_hash))
                {
                    return StatusCode((int)HttpStatusCode.Forbidden, new Response<dynamic>(false, HttpStatusCode.Forbidden, "Token inválido para esta orden"));
                }

                if (order.pickup_redeemed_at != null)
                {
                    return StatusCode((int)HttpStatusCode.Conflict, new Response<dynamic>(false, HttpStatusCode.Conflict, "La orden ya fue marcada como entregada"));
                }

                order.pickup_redeemed_at = DateTime.UtcNow;
                order.pickup_verified_by = string.IsNullOrWhiteSpace(request.verified_by) ? null : request.verified_by;
                order.status = StatusDelivered;

                db.SaveChanges();

                var pickupInfo = new PickupInfoResponse
                {
                    orderId = order.order_id,
                    pickupCode = order.pickup_code,
                    pickupPayloadBase64 = order.pickup_payload_base64,
                    pickupQrImageBase64 = order.pickup_qr_base64,
                    generatedAtUtc = order.pickup_generated_at,
                    deliveredAtUtc = order.pickup_redeemed_at,
                    status = order.status
                };

                return StatusCode((int)HttpStatusCode.OK, new Response<dynamic>(true, HttpStatusCode.OK, pickupInfo));
            }
            catch (Exception ex)
            {
                const string errorMessage = "Error al validar el QR de entrega";
                _logger.LogError(ex, "{ErrorMessage}", errorMessage);
                return StatusCode((int)HttpStatusCode.InternalServerError, new Response<dynamic>(false, HttpStatusCode.InternalServerError, errorMessage, ex.Message));
            }
        }

        private static void EnsureClientRecord(ApplicationDbContext db, string customerId, long? userId)
        {
            var client = db.clients.FirstOrDefault(c => c.document == customerId);
            if (client != null)
            {
                return;
            }

            string? name = null;
            string? email = null;

            if (userId != null)
            {
                var user = db.users.FirstOrDefault(u => u.id == userId);
                if (user != null)
                {
                    name = $"{user.name} {user.last_name}".Trim();
                    email = user.email;
                }
            }

            client = new Client
            {
                document = customerId,
                name = string.IsNullOrWhiteSpace(name) ? "Invitado SnackGol" : name,
                emailAddress = email,
                docType = userId != null ? "USER" : "SESSION",
                status = "ACTIVE"
            };

            db.clients.Add(client);
        }

        /// <summary>
        /// Resetea el stock de todos los productos a 100 unidades (para desarrollo/testing)
        /// </summary>
        [HttpPost("ResetStock")]
        public IActionResult ResetStock()
        {
            try
            {
                using var db = new ApplicationDbContext();
                var products = db.products.ToList();
                
                foreach (var product in products)
                {
                    product.stock = 100;
                }
                
                db.SaveChanges();
                
                return Ok(new { success = true, productsUpdated = products.Count, newStock = 100 });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al resetear stock");
                return StatusCode(
                    (int)HttpStatusCode.InternalServerError,
                    new Response<dynamic>(false, HttpStatusCode.InternalServerError, "Error al resetear stock", ex.Message)
                );
            }
        }
    }
}
