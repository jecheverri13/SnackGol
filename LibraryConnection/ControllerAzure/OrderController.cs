using LibraryConnection.Context;
using LibraryConnection.DbSet;
using LibraryConnection.Dtos;
using LibraryEntities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Net;
using OrderLine = LibraryConnection.DbSet.OrderLine;

namespace LibraryConnection.ControllerAzure
{
    public class OrderController
    {
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;

        public OrderController(IConfiguration configuration, HttpClient httpClient)
        {
            _configuration = configuration;
            _httpClient = httpClient;
        }
        /// <summary>
        /// Obtiene todas las órdenes del sistema con sus líneas asociadas
        /// </summary>
        /// <returns>Response con la lista de todas las órdenes o mensaje de error</returns>
        public static Response<List<OrderResponse>> GetAll()
        {
            try
            {
                using (var contexto = new ApplicationDbContext())
                {
                    // Obtener todas las órdenes con sus líneas relacionadas
                    var orders = contexto.orders
                        .Include(o => o.OrderLines)
                        .Select(o => new OrderResponse
                        {
                            order_id = o.order_id,
                            customer_id = o.customer_id,
                            order_date = o.order_date,
                            status = o.status,
                            total_net_price = o.total_net_price,
                            total_gross_amount = o.total_gross_amount,
                            pickup_code = o.pickup_code,
                            pickup_payload_base64 = o.pickup_payload_base64,
                            pickup_qr_base64 = o.pickup_qr_base64,
                            pickup_generated_at = o.pickup_generated_at,
                            pickup_redeemed_at = o.pickup_redeemed_at,
                            pickup_verified_by = o.pickup_verified_by,
                            OrderLines = o.OrderLines.Select(ol => new OrderLineResponse
                            {
                                lineNum = ol.lineNum,
                                item = ol.item,
                                description = ol.description,
                                netPrice = ol.net_price,
                                grossAmount = ol.gross_amount,
                                quantity = ol.quantity,

                            }).ToList()
                        })
                        .ToList();

                    if (orders != null && orders.Count != 0)
                    {
                        return new Response<List<OrderResponse>>(true, HttpStatusCode.OK, orders);
                    }
                    else
                    {
                        return new Response<List<OrderResponse>>(true, HttpStatusCode.NoContent, "No se encontraron órdenes registradas");
                    }
                }
            }
            catch (Exception ex)
            {
                string errorMessage = "Error al obtener las órdenes: " + ex.Message;
                if (ex.InnerException != null)
                {
                    errorMessage += " Inner exception: " + ex.InnerException.Message;
                }

                // Registrar el error (podrías inyectar ILogger si lo prefieres)
                // _logger.LogError(errorMessage);

                return new Response<List<OrderResponse>>(false, HttpStatusCode.InternalServerError, "Error en GetAllOrders", errorMessage);
            }
        }
        /// <summary>
        /// Crear una nueva orden con sus líneas de orden
        /// </summary>
        /// <param name="orderRequest">Datos de la orden y sus líneas</param>
        /// <returns>Respuesta con el resultado de la operación</returns>
        public static Response<dynamic> CreateOrder(OrderRequest orderRequest)
        {
            try
            {
                using (var context = new ApplicationDbContext())
                {
                    // Verificar si el cliente existe
                    var customerExists = context.clients.Any(c => c.document == orderRequest.customer_id);
                    if (!customerExists)
                    {
                        return new Response<dynamic>(false, HttpStatusCode.NotFound, "El cliente especificado no existe");
                    }

                    // Crear la orden
                    var order = new Order
                    {
                        customer_id = orderRequest.customer_id,
                        order_date = orderRequest.order_date,
                        total_gross_amount  = orderRequest.total_gross_amount,
                        total_net_price = orderRequest.total_net_price,
                        OrderLines = orderRequest.order_lines

                        .Select((ol, index) => new OrderLine
                        {
                            lineNum = index,  // Asigna el índice como número de línea
                            item = ol.item,
                            description = ol.description,
                            gross_amount = ol.grossAmount,
                            net_price = ol.netPrice,
                            tax_amount = ol.taxAmount,
                            quantity = ol.quantity
                        }).ToList()
                    };

                    context.orders.Add(order);
                    int affectedRows = context.SaveChanges();

                    if (affectedRows > 0)
                    {
                        return new Response<dynamic>(true, HttpStatusCode.Created, order);
                        //new { order_id = order.order_id });
                    }

                    return new Response<dynamic>(false, HttpStatusCode.BadRequest, "No se pudo crear la orden");
                }
            }
            catch (Exception ex)
            {
                string errorMessage = "Error al crear la orden: " + ex.Message;
                if (ex.InnerException != null)
                {
                    errorMessage += " Inner exception: " + ex.InnerException.Message;
                }

                return new Response<dynamic>(false, HttpStatusCode.InternalServerError, errorMessage);
            }
        }

        /// <summary>
        /// Obtener una orden con sus líneas por ID
        /// </summary>
        /// <param name="orderId">ID de la orden</param>
        /// <returns>Orden con sus líneas</returns>
        public static Response<OrderResponse> GetOrderById(string id)
        {
            try
            {
                using (var context = new ApplicationDbContext())
                {

                    var order = context.orders.FirstOrDefault(o => o.order_id == id);
                    if (order == null)
                        return new Response<OrderResponse>(false, HttpStatusCode.NotFound, "Orden no encontrada");
                    return new Response<OrderResponse>(true, HttpStatusCode.OK);
                }
            }
            catch (Exception ex)
            {
                string errorMessage = "Error al obtener la orden: " + ex.Message;
                if (ex.InnerException != null)
                {
                    errorMessage += " Inner exception: " + ex.InnerException.Message;
                }

                return new Response<OrderResponse>(false, HttpStatusCode.InternalServerError, errorMessage);
            }
        }

        /// <summary>
        /// Obtener todas las órdenes de un cliente
        /// </summary>
        /// <param name="customerId">ID del cliente</param>
        /// <returns>Lista de órdenes del cliente</returns>
        public static Response<List<OrderResponse>> GetCustomerOrders(string customerId)
        {
            try
            {
                using (var context = new ApplicationDbContext())
                {
                    var orders = context.orders
                        .Where(o => o.customer_id == customerId)
                        .Include(o => o.OrderLines)
                        .ToList();

                    if (orders == null || orders.Count == 0)
                    {
                        return new Response<List<OrderResponse>>(true, HttpStatusCode.NoContent, "No se encontraron órdenes para el cliente especificado");
                    }
                    var orderResponses = orders.Select(o => new OrderResponse
                    {
                        order_id = o.order_id,
                        customer_id = o.customer_id,
                        order_date = o.order_date,
                        status = o.status,
                        total_gross_amount = o.total_gross_amount,
                        total_net_price = o.total_net_price,
                        pickup_code = o.pickup_code,
                        pickup_payload_base64 = o.pickup_payload_base64,
                        pickup_qr_base64 = o.pickup_qr_base64,
                        pickup_generated_at = o.pickup_generated_at,
                        pickup_redeemed_at = o.pickup_redeemed_at,
                        pickup_verified_by = o.pickup_verified_by,
                        OrderLines = o.OrderLines.Select(ol => new OrderLineResponse
                        {
                            lineNum = ol.lineNum,
                            item = ol.item,
                            description = ol.description,
                            netPrice = ol.net_price,
                            grossAmount = ol.gross_amount,
                            taxAmount = ol.tax_amount,
                            quantity = ol.quantity  
                        }).ToList()
                    }).ToList();

                    return new Response<List<OrderResponse>>(true, HttpStatusCode.OK, orderResponses);
                }
            }
            catch (Exception ex)
            {
                string errorMessage = "Error al obtener las órdenes: " + ex.Message;
                if (ex.InnerException != null)
                {
                    errorMessage += " Inner exception: " + ex.InnerException.Message;

                }

                return new Response<List<OrderResponse>>(false, HttpStatusCode.InternalServerError, errorMessage);
            }
        }

        public static Response<OrderResponse> UpdateOrder(OrderRequest orderRequest)
        {
            throw new NotImplementedException();
        }

    }
}
