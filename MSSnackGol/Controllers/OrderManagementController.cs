using LibraryConnection.ControllerAzure;
using LibraryConnection.Context;
using LibraryConnection.Dtos;
using LibraryConnection.DbSet;
using LibraryEntities.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Linq;
using QRCoder;
using SkiaSharp;

namespace MSSnackGol.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderManagementController : ControllerBase
    {
        private readonly ILogger<OrderManagementController> _logger;
        private static readonly JsonSerializerOptions PickupJsonOptions = new(JsonSerializerDefaults.Web)
        {
            PropertyNameCaseInsensitive = true
        };

        private const string StatusReadyForPickup = "ReadyForPickup";
        private const string StatusDelivered = "Delivered";

        public OrderManagementController(ILogger<OrderManagementController> logger)
        {
            _logger = logger;
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

                var orderId = GenerateOrderId();
                var order = new Order
                {
                    order_id = orderId,
                    customer_id = customerId,
                    order_date = DateTime.UtcNow,
                    status = StatusReadyForPickup,
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

                var pickupCode = GeneratePickupCode();
                var artifacts = BuildPickupArtifacts(orderId, pickupCode, session, items);

                order.pickup_code = pickupCode;
                order.pickup_token_hash = HashToken(artifacts.Token);
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
                    status = order.status ?? StatusReadyForPickup,
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

                if (!HashToken(request.token!).Equals(order.pickup_token_hash, StringComparison.OrdinalIgnoreCase))
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

        private static string GenerateOrderId()
        {
            return $"ORD-{DateTime.UtcNow:yyyyMMddHHmmss}-{RandomNumberGenerator.GetInt32(1000, 9999)}";
        }

        private static string GeneratePickupCode()
        {
            return $"SG-{DateTime.UtcNow:yyMMddHHmmss}-{RandomNumberGenerator.GetInt32(1000, 9999)}";
        }

        private static string HashToken(string token)
        {
            var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(token));
            return Convert.ToHexString(bytes);
        }

        private static (string Token, string PayloadBase64, string QrImageBase64) BuildPickupArtifacts(string orderId, string pickupCode, string? sessionToken, List<CartItem> items)
        {
            items ??= new List<CartItem>();

            var token = Convert.ToHexString(RandomNumberGenerator.GetBytes(16));
            var payload = new PickupPayload(orderId, pickupCode, token, sessionToken, DateTime.UtcNow);
            var payloadJson = JsonSerializer.Serialize(payload, PickupJsonOptions);
            var payloadBase64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(payloadJson));

            // Construir contenido legible para el QR (lista resumida que cualquier escáner interpreta)
            var sb = new StringBuilder();
            sb.AppendLine("SnackGol - Pedido confirmado");
            sb.AppendLine($"Orden: {orderId}");
            sb.AppendLine($"Código: {pickupCode}");
            sb.AppendLine("----------------");
            if (items.Count == 0)
            {
                sb.AppendLine("Detalle no disponible");
            }
            else
            {
                foreach (var item in items)
                {
                    var label = string.IsNullOrWhiteSpace(item.Product?.name)
                        ? $"Producto #{item.product_id}"
                        : item.Product!.name;
                    sb.AppendLine($"{item.quantity}x {label}");
                }
            }

            sb.AppendLine("----------------");
            var totalFormatted = (items.Sum(i => i.subtotal)).ToString("C0", new CultureInfo("es-CO"));
            sb.AppendLine($"Total: {totalFormatted}");
            sb.AppendLine("Presenta este QR en SnackGol para retirar tu pedido.");

            var qrContent = sb.ToString();

            var qrGenerator = new QRCodeGenerator();
            var qrData = qrGenerator.CreateQrCode(qrContent, QRCodeGenerator.ECCLevel.H); // Mayor corrección para soportar el logo

            // Color oscuro personalizado manteniendo alto contraste
            var brandDark = new byte[] { 32, 35, 45 };
            var pngQr = new PngByteQRCode(qrData).GetGraphic(20, brandDark, new byte[] { 255, 255, 255 }, true);

            using var logoBitmap = GenerateLogoBitmap(120, 120);
            var qrWithLogo = OverlayLogoOnQr(pngQr, logoBitmap);

            var qrImageBase64 = Convert.ToBase64String(qrWithLogo);

            return (token, payloadBase64, qrImageBase64);
        }

        private static SKBitmap GenerateLogoBitmap(int width, int height)
        {
            var bitmap = new SKBitmap(width, height);
            using var canvas = new SKCanvas(bitmap);
            canvas.Clear(SKColors.Transparent);

            // Dibujar círculo de fondo
            var paint = new SKPaint { Color = new SKColor(255, 107, 53), IsAntialias = true };
            canvas.DrawCircle(width / 2, height / 2, width / 2 - 5, paint);

            // Dibujar dona
            paint.Color = SKColors.Gold;
            canvas.DrawCircle(width / 2, height / 2 - 10, 15, paint);
            paint.Color = SKColors.OrangeRed;
            canvas.DrawCircle(width / 2, height / 2 - 10, 10, paint);

            // Ojos
            paint.Color = SKColors.White;
            canvas.DrawCircle(width / 2 - 5, height / 2 - 15, 2, paint);
            canvas.DrawCircle(width / 2 + 5, height / 2 - 15, 2, paint);
            canvas.DrawCircle(width / 2, height / 2 - 5, 2, paint);

            // Texto
            paint.Color = SKColors.White;
            paint.TextSize = 12;
            paint.TextAlign = SKTextAlign.Center;
            canvas.DrawText("SnackGol", width / 2, height / 2 + 20, paint);
            paint.TextSize = 8;
            canvas.DrawText("¡Tu snack favorito!", width / 2, height / 2 + 35, paint);

            return bitmap;
        }

        private static byte[] OverlayLogoOnQr(byte[] qrBytes, SKBitmap logo)
        {
            using var qrStream = new MemoryStream(qrBytes);
            using var qrBitmap = SKBitmap.Decode(qrStream);
            using var canvas = new SKCanvas(qrBitmap);

            var logoSize = Math.Min(qrBitmap.Width, qrBitmap.Height) / 5f; // Logo ocupa 20% del QR
            var logoRect = new SKRect(
                (qrBitmap.Width - logoSize) / 2f,
                (qrBitmap.Height - logoSize) / 2f,
                (qrBitmap.Width + logoSize) / 2f,
                (qrBitmap.Height + logoSize) / 2f
            );

            var cornerRadius = logoSize * 0.25f;

            using (var backgroundPaint = new SKPaint
            {
                Color = SKColors.White,
                IsAntialias = true
            })
            {
                canvas.DrawRoundRect(logoRect, cornerRadius, cornerRadius, backgroundPaint);
            }

            var inset = logoSize * 0.08f;
            var innerRect = new SKRect(
                logoRect.Left + inset,
                logoRect.Top + inset,
                logoRect.Right - inset,
                logoRect.Bottom - inset
            );

            using (var borderPaint = new SKPaint
            {
                Color = new SKColor(255, 107, 53),
                Style = SKPaintStyle.Stroke,
                StrokeWidth = Math.Max(2f, logoSize * 0.06f),
                IsAntialias = true
            })
            {
                canvas.DrawRoundRect(innerRect, cornerRadius * 0.9f, cornerRadius * 0.9f, borderPaint);
            }

            canvas.DrawBitmap(logo, innerRect);
            canvas.Flush();

            using var outputStream = new MemoryStream();
            qrBitmap.Encode(outputStream, SKEncodedImageFormat.Png, 100);
            return outputStream.ToArray();
        }

        private record PickupPayload(string OrderId, string PickupCode, string Token, string? SessionToken, DateTime GeneratedAtUtc);
    }
}

