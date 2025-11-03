using LibraryConnection;
using LibraryConnection.ControllerAzure;
using LibraryConnection.Context;
using LibraryConnection.Dtos;
using LibraryEntities.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Microsoft.EntityFrameworkCore;
namespace MSSnackGol.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderManagementController : ControllerBase
    {
        private readonly ILogger<OrderManagementController> _logger;

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
        /// Realiza checkout del carrito actual (por sesión o usuario) sin requerir cédula.
        /// Reduce stock y vacía el carrito.
        /// </summary>
    [HttpPost("Checkout")]
    public IActionResult Checkout([FromBody] LibraryEntities.Models.CheckoutRequest body)
        {
            try
            {
                using var db = new ApplicationDbContext();

                var session = body?.session_token;
                var userId = User?.Identity?.IsAuthenticated == true ? (long?)Convert.ToInt64(User.Identity!.Name) : null;

                var cart = db.carts
                    .FirstOrDefault(c => (userId != null && c.user_id == userId) || (session != null && c.session_token == session));

                if (cart == null)
                    return StatusCode((int)HttpStatusCode.NotFound, new Response<dynamic>(false, HttpStatusCode.NotFound, "Carrito no encontrado"));

                var items = db.cart_items
                    .Include(i => i.Product)
                    .Where(i => i.cart_id == cart.id)
                    .ToList();

                if (items.Count == 0)
                    return StatusCode((int)HttpStatusCode.BadRequest, new Response<dynamic>(false, HttpStatusCode.BadRequest, "Carrito vacío"));

                // Validar y descontar stock
                foreach (var it in items)
                {
                    if (it.Product.stock < it.quantity)
                        return StatusCode((int)HttpStatusCode.Conflict, new Response<dynamic>(false, HttpStatusCode.Conflict, $"Stock insuficiente para {it.Product.name}"));
                }

                foreach (var it in items)
                {
                    it.Product.stock -= it.quantity;
                }

                // Vaciar carrito
                db.cart_items.RemoveRange(items);
                cart.updated_at = DateTime.UtcNow;

                db.SaveChanges();

                return StatusCode((int)HttpStatusCode.Created, new Response<dynamic>(true, HttpStatusCode.Created, new { message = "Checkout completado" }));
            }
            catch (Exception ex)
            {
                const string errorMessage = "Error en checkout";
                _logger.LogError(ex, "{ErrorMessage}", errorMessage);
                return StatusCode((int)HttpStatusCode.InternalServerError, new Response<dynamic>(false, HttpStatusCode.InternalServerError, errorMessage, ex.Message));
            }
        }
    }
}
    
