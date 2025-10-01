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
                var oResponse = OrderController.CreateOrderWithLines(orderRequest);
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
        public IActionResult UpdateOrder([FromBody] OrderRequest order, string type)
        {
            try
            {
                var oResponse = new Response<OrderResponse>();
                if (type == "FE")
                {
                    oResponse = OrderController.UpdateOrderFE(order);
                }
                else
                {
                    oResponse = OrderController.UpdateOrderSAP(order);
                }
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

        [HttpPatch("UpdateStatus")]
        public IActionResult UpdateOrderStatus(string orderId, string newStatus, string statusToUpdate)
        {
            try
            {
                var oResponse = OrderController.UpdateOrderStatus(orderId, newStatus, statusToUpdate);
                return StatusCode((int)oResponse.status, oResponse);
            }
            catch (Exception ex)
            {
                const string errorMessage = "Error al actualizar el estado de la orden";
                _logger.LogError(ex, "{ErrorMessage}", errorMessage);
                return StatusCode(
                    (int)HttpStatusCode.InternalServerError,
                    new Response<dynamic>(false, HttpStatusCode.InternalServerError, errorMessage, ex.Message)
                );
            }
        }

        [HttpGet("WithCufe")]
        public IActionResult GetAllOrdersWithCufe()
        {
            try
            {
                var oResponse = OrderController.GetAllOrdersWithCufe();
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
        /// <summary>
        /// Obtiene las ordenes filtradas por fecha si es el caso, y filtro en los estados de SAP y FE
        /// </summary>
        [HttpGet("monitor")]
        public IActionResult GetOrdersForMonitor([FromQuery]OrderRequestMonitor order) { 
            try
            {
                // Asumiendo que tenemos un método en OrderController para obtener todas las órdenes
         
                var oResponse = OrderController.GetAllOrdersMonitor(order);
                return StatusCode((int)oResponse.status, oResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener las órdenes");
                return StatusCode(
                    (int)HttpStatusCode.InternalServerError,
                    new Response<dynamic>(false, HttpStatusCode.InternalServerError, "Error al obtener las órdenes", ex.Message)
                );
            }
        }
        /// <summary>
        /// Obtiene las ordenes filtradas por fecha si es el caso, y filtro en los estados de SAP y FE
        /// </summary>
        [HttpGet("stats")]
        public IActionResult GetStats([FromQuery] OrderSummaryRequest order)
        {
            try
            {
                // Asumiendo que tenemos un método en OrderController para obtener todas las órdenes
                var oResponse = OrderController.CalculteStats(order);
                return StatusCode((int)oResponse.status, oResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener las órdenes");
                return StatusCode(
                    (int)HttpStatusCode.InternalServerError,
                    new Response<dynamic>(false, HttpStatusCode.InternalServerError, "Error al obtener las órdenes", ex.Message)
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
                var oResponse = OrderController.GetAllOrdersToSendFE();
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
        [HttpGet("ToSendFE")]
        public IActionResult GetAllOrdersToSendFE()
        {
            try
            {
                var oResponse = OrderController.GetAllOrdersToSendFE();
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

        // <summary>
        /// Obtiene una orden específica por su ReceiptId
        /// </summary>
        [HttpGet("ReceiptId/{receiptId}")]
        public IActionResult GetOrderByReceiptId(string receiptId)
        {
            try
            {
                var oResponse = OrderController.GetOrderByReceiptId(receiptId);
                return StatusCode((int)oResponse.status, oResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al obtener la orden con id de recibo: {receiptId}");
                return StatusCode(
                    (int)HttpStatusCode.InternalServerError,
                    new Response<dynamic>(false, HttpStatusCode.InternalServerError, $"Error al obtener la orden con id de recibo: {receiptId}", ex.Message)
                );
            }
        }
    }
}
    
