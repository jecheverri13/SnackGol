using LibraryConnection.Context;
using LibraryConnection.DbSet;
using LibraryConnection.Dtos;
using LibraryEntities;
using LibraryEntities.EntitiesSL;
using LibraryEntities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Sockets;
using B1ServiceLayerCSS;
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
        public static Response<List<OrderResponse>> GetAllOrders(string? startDate, string? endDate, int pageSize, int page, string status)
        {
            try
            {
                //Si startDate y endDate son nulos o vacíos, retornar todos los registros
                if (string.IsNullOrEmpty(startDate) && string.IsNullOrEmpty(endDate))
                {
                    startDate = DateTime.MinValue.ToString("yyyy-MM-dd");
                    endDate = DateTime.MaxValue.ToString("yyyy-MM-dd");
                }

                //Si solo uno de los dos es nulo retornar error
                if (string.IsNullOrEmpty(startDate) || string.IsNullOrEmpty(endDate))
                {
                    return new Response<List<OrderResponse>>(false, HttpStatusCode.BadRequest, "Debe especificar ambas fechas: startDate y endDate");
                }

                // Si startDate es mayor a endDate, retornar error
                if (DateTime.Parse(startDate) > DateTime.Parse(endDate))
                {
                    return new Response<List<OrderResponse>>(false, HttpStatusCode.BadRequest, "La fecha de inicio no puede ser mayor a la fecha de fin");
                }
                // Si starDate y endDate son iguales retornar los registros de ese día
                if (DateTime.Parse(startDate) == DateTime.Parse(endDate))
                {
                    endDate = DateTime.Parse(endDate).AddDays(1).ToString("yyyy-MM-dd");
                }

                // Convertir las fechas a DateTime con Kind UTC
                DateTime startUtc = DateTime.SpecifyKind(DateTime.Parse(startDate), DateTimeKind.Utc);
                DateTime endUtc = DateTime.SpecifyKind(DateTime.Parse(endDate), DateTimeKind.Utc);

                using (var contexto = new ApplicationDbContext())
                {
                    // Obtener todas las órdenes con sus líneas relacionadas
                    var orders = contexto.orders
                        .Include(o => o.OrderLines)  // Incluir las líneas de orden
                        .Select(o => new OrderResponse
                        {
                            order_id = o.order_id,
                            customer_id = o.customer_id,
                            order_date = o.order_date,
                            cufe = o.cufe,
                            status_sap = o.status_sap,
                            status_fe= o.status_fe,
                            total_gross_amount = o.total_gross_amount,
                            total_net_price = o.total_net_price,
                            OrderLines = o.OrderLines.Select(ol => new OrderLineResponse
                            {
                                lineNum = ol.lineNum,
                                item = ol.item,
                                netPrice = ol.net_price,
                                grossAmount = ol.gross_amount,
                                tax_code = ol.tax_code,
                                taxAmount = ol.tax_amount
                            }).ToList()
                        })
                        .Where(o => o.order_date >= startUtc 
                                && o.order_date <= endUtc)
                                //&& o.status == status)
                        .OrderByDescending(o => o.order_date)
                        .Take(pageSize)
                        .Skip((page - 1) * pageSize)
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
        /// Obtiene todas las órdenes del sistema con sus líneas asociadas
        /// </summary>
        /// <returns>Response con la lista de todas las órdenes o mensaje de error</returns>
        public static Response<List<OrderResponse>> GetAllOrdersWithCufe()
        {
            try
            {
                using (var contexto = new ApplicationDbContext())
                {
                    // Obtener todas las órdenes con sus líneas relacionadas
                    var orders = contexto.orders
                        .Include(o => o.OrderLines)  // Incluir las líneas de orden
                        .Include(o => o.resolution)
                        .Select(o => new OrderResponse
                        {
                            order_id = o.order_id,
                            customer_id = o.customer_id,
                            order_date = o.order_date,
                            cufe = o.cufe,
                            serial_number = o.serial_number,
                            url = o.url,
                            prefix = o.resolution != null ? o.resolution.prefix : null,
                            status_sap = o.status_sap,
                            status_fe = o.status_fe,
                            credit_card_number = o.credit_card_number,
                            credit_card_expiration = o.credit_card_expiration,
                            voucher_number = o.voucher_number,
                            total_net_price = o.total_net_price,
                            total_gross_amount = o.total_gross_amount,
                            OrderLines = o.OrderLines.Select(ol => new OrderLineResponse
                            {
                                lineNum = ol.lineNum,
                                item = ol.item,
                                description = ol.description,
                                netPrice = ol.net_price,
                                grossAmount = ol.gross_amount,
                                quantity = ol.quantity,
                                tax_code = ol.tax_code,

                            }).ToList()
                        })
                        .Where(o => !string.IsNullOrEmpty(o.cufe) && o.status_sap == "pending"
                                    && !string.IsNullOrEmpty(o.credit_card_number))
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
        /// Obtiene todas las órdenes del sistema con sus líneas asociadas
        /// </summary>
        /// <returns>Response con la lista de todas las órdenes o mensaje de error</returns>
        public static Response<List<OrderMOToFE>> GetAllOrdersToSendFE()
        {
            try
            {
                using (var contexto = new ApplicationDbContext())
                {
                    // Obtener todas las órdenes con sus líneas relacionadas y su respectivo cliente

                    var orders = contexto.orders
                        .Include(o => o.OrderLines)  // Incluir las líneas de orden
                        .Include(o => o.client)    // Incluir el cliente
                        .Include(o => o.resolution)
                        .Select(o => new OrderMOToFE
                        {
                            order_id = o.order_id,
                            customer_id = o.customer_id,
                            order_date = o.order_date,
                            cufe = o.cufe,
                            status_sap = o.status_sap,
                            status_fe = o.status_fe,
                            credit_card_number = o.credit_card_number,
                            credit_card_expiration = o.credit_card_expiration,
                            voucher_number = o.voucher_number,
                            doc_num = o.serial_number,
                            prefix = o.resolution != null ? o.resolution.prefix : null,
                            resolution = o.resolution != null ? o.resolution.resolution_number : null,
                            start_date = o.resolution != null ? o.resolution.init_date : null,
                            end_date = o.resolution != null ? o.resolution.final_date : null,
                            initial_number = o.resolution != null ? o.resolution.init_consecutive.ToString() : null,
                            final_number = o.resolution != null ? o.resolution.final_consecutive.ToString() : null,
                            currency = "COP",
                            total_gross_amount = o.total_gross_amount,
                            total_net_price = o.total_net_price,
                            OrderLines = o.OrderLines.Select(ol => new OrderLineMOToFE
                            {
                                lineNum = ol.lineNum,
                                item = ol.item,
                                item_description = ol.description,
                                net_price = ol.net_price,
                                gross_amount = ol.gross_amount,
                                quantity = ol.quantity,
                                tax_code = ol.tax_code
                            }).ToList(),
                            Client = new ClientResponse
                            {
                                document = o.client.document,
                                name = o.client.name,
                                emailAddress = o.client.emailAddress,
                                docType = o.client.docType,
                                status = o.client.status
                            }
                        })
                        .Where(o => string.IsNullOrEmpty(o.cufe) && o.status_fe == "pending")
                        .ToList();

                    if (orders != null && orders.Count > 0)
                    {
                        return new Response<List<OrderMOToFE>>(true, HttpStatusCode.OK, orders);
                    }
                    else
                    {
                        return new Response<List<OrderMOToFE>>(true, HttpStatusCode.NoContent, "No se encontraron órdenes registradas");
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
                return new Response<List<OrderMOToFE>>(false, HttpStatusCode.InternalServerError, "Error en GetAllOrdersToSendFE", errorMessage);
            }
        }

        /// <summary>
        /// Obtiene todas las órdenes filtrando por paginacion,estados y fehcas 
        /// </summary>
        /// <returns>Response con la lista de todas las órdenes o mensaje de error</returns>
        public static Response<PagedOrdersResponse> GetAllOrdersMonitor(OrderRequestMonitor oOrder)
        {
        
            try
            {
                using (var contexto = new ApplicationDbContext())
                {
                    var emptyResult = new PagedOrdersResponse
                    {
                        Total = 0,
                        Orders = new List<OrderResponseMonitor>()
                    };

                    var page = oOrder.pagination.PageNumber <= 0 ? 1 : oOrder.pagination.PageNumber;
                    int pageSize = Math.Clamp(oOrder.pagination.PageSize, 1, 50);
                    var query= contexto.orders.AsQueryable().AsNoTracking();
                    DateTime? start = oOrder.start.HasValue
                    ? DateTime.SpecifyKind(oOrder.start.Value.Date, DateTimeKind.Utc)
                    : (DateTime?)null;

                    DateTime? endInclusive = oOrder.end.HasValue
                        ? DateTime.SpecifyKind(oOrder.end.Value.Date, DateTimeKind.Utc)
                        : (DateTime?)null;

                    DateTime? endExclusive = endInclusive.HasValue
                        ? endInclusive.Value.AddDays(1)
                        : (start.HasValue ? start.Value.AddDays(1) : (DateTime?)null);
                    if (start.HasValue)
                    {
                        var s = start.Value;
                        var e = endExclusive ?? s.AddDays(1);

                        query = query.Where(o => o.order_date >= s && o.order_date < e);
                    }
                    if (!string.IsNullOrEmpty(oOrder.status_sap))
                    {
                        query = query.Where(o => o.status_sap == oOrder.status_sap);
                    }
                    if (!string.IsNullOrEmpty(oOrder.status_fe))
                    {
                        query = query.Where(o => o.status_fe == oOrder.status_fe);

                    }
                    var total = contexto.orders.Count();

                    var orders = query
                        .Skip((page - 1) * pageSize)    
                        .Take(pageSize)
                        .Include(o=>o.client)   
                        .Select(o => new OrderResponseMonitor
                        {
                            code =o.serial_number, 
                            id = o.order_id,
                            client = o.client.name,
                            date= o.order_date,
                            amount = o.total_gross_amount,
                            attempts =  contexto.errorsFe.Count(e => e.order_id == o.order_id),
                            status_fe = o.status_fe,
                            status_sap=o.status_sap

                        })
                        .ToList();
                  
                    if (orders != null && orders.Any()) 
                    {
                        var result = new PagedOrdersResponse
                        {
                            Total = total,
                            Orders = orders
                        };

                        return new Response<PagedOrdersResponse>(true, HttpStatusCode.OK, result);
                    }
                    else
                    {


                        return new Response<PagedOrdersResponse>(true, HttpStatusCode.NoContent, emptyResult);
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
                

                return new Response<PagedOrdersResponse>(false, HttpStatusCode.InternalServerError, "Error en GetAllOrdersMonitor " + ex.Message);
            }
        }
        
        /// <summary>
        /// Calcula la cantidad de documentos, la cantidad de errores fe y sap 
        /// </summary>  
        /// <returns>Response con la lista de todas las órdenes o mensaje de error</returns>
        public static Response<OrdersummaryResponse> CalculteStats(OrderSummaryRequest oDate)
        {
            try
            {
                using (var contexto = new ApplicationDbContext())
                {
                    var query = contexto.orders.AsQueryable().AsNoTracking();
                    DateTime? start = oDate.start.HasValue
                    ? DateTime.SpecifyKind(oDate.start.Value.Date, DateTimeKind.Utc)
                    : (DateTime?)null;

                    DateTime? endInclusive = oDate.end.HasValue
                        ? DateTime.SpecifyKind(oDate.end.Value.Date, DateTimeKind.Utc)
                        : (DateTime?)null;

                    DateTime? endExclusive = endInclusive.HasValue
                        ? endInclusive.Value.AddDays(1)
                        : (start.HasValue ? start.Value.AddDays(1) : (DateTime?)null);
                    if (start.HasValue)
                    {
                        var s = start.Value;
                        var e = endExclusive ?? s.AddDays(1);

                        query = query.Where(o => o.order_date >= s && o.order_date < e);
                    }

                    var orders = query
                        .Select(o => new { o.status_sap, o.status_fe })
                        .ToList();

                    int accepted = 0, errors = 0, pending = 0;
                    foreach (var s in orders)
                    {
                        if (s.status_sap == "error"||s.status_fe =="error") { errors++; continue; }
                        if (s.status_sap == "pending" || s.status_fe == "pending") { pending++; continue; }
                        accepted++;
                    }


                    var summary = new OrdersummaryResponse
                    {
                        TotalDocuments = orders.Count,
                        Accepted = accepted,
                        Errors = errors,
                        Pending = pending
                    };
                    return new Response<OrdersummaryResponse>(true, HttpStatusCode.OK, summary);


                }
            }
            catch (Exception ex)
            {
                string errorMessage = "Error al obtener al calcular las sats: " + ex.Message;
                if (ex.InnerException != null)
                {
                    errorMessage += " Inner exception: " + ex.InnerException.Message;
                }

                // Registrar el error (podrías inyectar ILogger si lo prefieres)
                // _logger.LogError(errorMessage);

                return new Response<OrdersummaryResponse>(false, HttpStatusCode.InternalServerError, "Error en CalculteStats ", errorMessage);
            }
        }

        /// <summary>
        /// Crear una nueva orden con sus líneas de orden
        /// </summary>
        /// <param name="orderRequest">Datos de la orden y sus líneas</param>
        /// <returns>Respuesta con el resultado de la operación</returns>
        public static Response<dynamic> CreateOrderWithLines(OrderRequest orderRequest)
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
                        resolution_id  = orderRequest.resolution_id,
                        serial_number = orderRequest.serial_number,
                        order_date = orderRequest.order_date,
                        cufe = orderRequest.cufe,
                        status_sap = "pending",
                        status_fe = "pending",
                        url = orderRequest.url,
                        credit_card_number = orderRequest.credit_card_number,
                        credit_card_expiration = orderRequest.credit_card_expiration,
                        voucher_number = orderRequest.voucher_number,
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
                            tax_code = ol.taxCode,
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
        public static Response<OrderResponse> GetOrderWithLines(string orderId)
        {
            try
            {
                using (var context = new ApplicationDbContext())
                {
                    var order = context.orders
                        .Include(o => o.OrderLines)
                        .FirstOrDefault(o => o.order_id == orderId);

                    if (order != null)
                    {
                        var orderResponse = new OrderResponse
                        {
                            order_id = order.order_id,
                            customer_id = order.customer_id,
                            order_date = order.order_date,
                            cufe = order.cufe,
                            status_sap = order.status_sap,
                            status_fe = order.status_fe,
                            OrderLines = order.OrderLines.Select(ol => new OrderLineResponse
                            {
                                lineNum = ol.lineNum,
                                item = ol.item,
                                description = ol.description,
                                netPrice = ol.net_price,
                                grossAmount = ol.gross_amount,
                                tax_code = ol.tax_code,
                                taxAmount = ol.tax_amount,
                                quantity = ol.quantity
                            }).ToList()
                        };
                        return new Response<OrderResponse>(true, HttpStatusCode.OK, orderResponse);
                    }

                    return new Response<OrderResponse>(false, HttpStatusCode.NotFound, "Orden no encontrada");
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
        /// Obtener una orden con sus líneas por ID
        /// </summary>
        /// <param name="orderId">ID de la orden</param>
        /// <returns>Orden con sus líneas</returns>
        public static Response<OrderResponse> GetOrderByReceiptId(string receiptId)
        {
            try
            {
                using (var context = new ApplicationDbContext())
                {
                    var order = context.orders
                        .Include(o => o.OrderLines)
                        .Include( r => r.resolution)
                        .FirstOrDefault(o => o.voucher_number == receiptId);

                    if (order != null)
                    {
                        var orderResponse = new OrderResponse
                        {
                            order_id = order.order_id,
                            customer_id = order.customer_id,
                            cufe = order.cufe,
                            serial_number = order.serial_number,
                            url = order.url,
                            Resolution = new ResolutionResponse { 
                                IdSerie = order.resolution.id_serie,
                                Prefix = order.resolution.prefix,
                                ResolutionNumber = order.resolution.resolution_number,
                                InitConsecutive = order.resolution.init_consecutive,
                                CurrentConsecutive = order.resolution.current_consecutive,
                                FinalConsecutive = order.resolution.final_consecutive,
                                InitDate = order.resolution.init_date,
                                FinalDate = order.resolution.final_date,
                                Status = order.resolution.status
                            },
                        };
                        return new Response<OrderResponse>(true, HttpStatusCode.OK, orderResponse);
                    }

                    return new Response<OrderResponse>(false, HttpStatusCode.NotFound, "Orden no encontrada");
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
        /// Obtener una orden con sus líneas por ID
        /// </summary>
        /// <param name="orderId">ID de la orden</param>
        /// <returns>Orden con sus líneas</returns>
        public static Response<OrderResponse> OrderExists(string vaucherNumber)
        {
            try
            {
                using (var context = new ApplicationDbContext())
                {

                    var order = context.orders.FirstOrDefault(o => o.voucher_number == vaucherNumber);
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
        /// Obtener una orden con sus líneas por ID
        /// </summary>
        /// <param name="orderId">ID de la orden</param>
        /// <returns>Orden con sus líneas</returns>
        public static Response<OrderResponse> ValidResolutionConsecutive(Resolution resolution)
        {
            try
            {
                using (var context = new ApplicationDbContext())
                {

                    var order = context.orders.FirstOrDefault(o => long.Parse(o.serial_number) == resolution.current_consecutive && o.resolution_id == resolution.id_serie);
                    if (order == null)
                        return new Response<OrderResponse>(true, HttpStatusCode.OK, "Puede utilizar este consecutivo");
                    return new Response<OrderResponse>(false, HttpStatusCode.BadRequest);
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
                        cufe = o.cufe,
                        status_sap = o.status_sap,
                        status_fe = o.status_fe,
                        OrderLines = o.OrderLines.Select(ol => new OrderLineResponse
                        {
                            lineNum = ol.lineNum,
                            item = ol.item,
                            description = ol.description,
                            netPrice = ol.net_price,
                            grossAmount = ol.gross_amount,
                            tax_code = ol.tax_code,
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

        public static Response<OrderResponse> UpdateOrderStatus(string orderId, string newStatus,string statusType)
        {
            try
            {
                using (var context = new ApplicationDbContext())
                {
                    var order = context.orders.FirstOrDefault(o => o.order_id == orderId);
                    if (order == null)
                    {
                        return new Response<OrderResponse>(false, HttpStatusCode.NotFound, "Orden no encontrada");
                    }
                    if(statusType=="status_sap") order.status_sap = newStatus;
                    else order.status_fe = newStatus;
                    context.SaveChanges();
                    return new Response<OrderResponse>(true, HttpStatusCode.NoContent, "Estado de la orden actualizado correctamente");
                }
            }
            catch (Exception ex)
            {
                string errorMessage = "Error al actualizar el estado de la orden: " + ex.Message;
                if (ex.InnerException != null)
                {
                    errorMessage += " Inner exception: " + ex.InnerException.Message;
                }
                return new Response<OrderResponse>(false, HttpStatusCode.InternalServerError, errorMessage);
            }
        }
        public static Response<OrderResponse> UpdateOrderSAP(OrderRequest order)
        {
            try
            {
                using (var context = new ApplicationDbContext())
                {
                    var existingOrder = context.orders
                        .Include(o => o.OrderLines)
                        .FirstOrDefault(o => o.order_id == order.order_id);
                    if (existingOrder == null)
                    {
                        return new Response<OrderResponse>(false, HttpStatusCode.NotFound, "Orden no encontrada");
                    }
                    existingOrder.status_sap = order.status_sap;
                    existingOrder.doc_entry_sap = order.doc_entry_sap;
                    context.SaveChanges();

                    return new Response<OrderResponse>(true, HttpStatusCode.NoContent, "Estado de la orden actualizado correctamente");
                }
            }
            catch (Exception ex)
            {
                string errorMessage = "Error al actualizar el estado de la orden: " + ex.Message;
                if (ex.InnerException != null)
                {
                    errorMessage += " Inner exception: " + ex.InnerException.Message;
                }
                return new Response<OrderResponse>(false, HttpStatusCode.InternalServerError, errorMessage);
            }
        }        
        public static Response<OrderResponse> UpdateOrderFE(OrderRequest order)
        {
            try
            {
                using (var context = new ApplicationDbContext())
                {
                    var existingOrder = context.orders
                        .Include(o => o.OrderLines)
                        .FirstOrDefault(o => o.order_id == order.order_id);
                    if (existingOrder == null)
                    {
                        return new Response<OrderResponse>(false, HttpStatusCode.NotFound, "Orden no encontrada");
                    }
                    existingOrder.status_fe = order.status_fe;
                    existingOrder.cufe = order.cufe;
                    existingOrder.url = order.url;
                    context.SaveChanges();

                    return new Response<OrderResponse>(true, HttpStatusCode.NoContent, "Estado de la orden actualizado correctamente");
                }
            }
            catch (Exception ex)
            {
                string errorMessage = "Error al actualizar el estado de la orden: " + ex.Message;
                if (ex.InnerException != null)
                {
                    errorMessage += " Inner exception: " + ex.InnerException.Message;
                }
                return new Response<OrderResponse>(false, HttpStatusCode.InternalServerError, errorMessage);
            }
        }
    }
}
