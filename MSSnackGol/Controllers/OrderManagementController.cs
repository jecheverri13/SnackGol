using LibraryConnection;
using LibraryConnection.ControllerAzure;
using LibraryConnection.Dtos;
using LibraryEntities.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net;
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
        public IActionResult GetOrderById(string id)
        {
            try
            {
                var oResponse = OrderController.GetOrderById(id);
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
    }
}
    
