using Microsoft.AspNetCore.Mvc;
using MSSnackGol.Services;

namespace MSSnackGol.Controllers.API
{
    [ApiController]
    [Route("api/[controller]")]
    public class PagoController : ControllerBase
    {
        private readonly IPaymentValidationService _paymentValidationService;
        private readonly ILogger<PagoController> _logger;

        public PagoController(IPaymentValidationService paymentValidationService, ILogger<PagoController> logger)
        {
            _paymentValidationService = paymentValidationService;
            _logger = logger;
        }

        /// <summary>
        /// Procesa un pago validando el método de pago
        /// </summary>
        /// <param name="request">Datos del pago a procesar</param>
        /// <returns>Resultado de la transacción</returns>
        [HttpPost("procesar")]
        public IActionResult ProcesarPago([FromBody] ProcesarPagoRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { error = "Datos del pago inválidos." });
            }

            try
            {
                // Validar según el método de pago
                if (request.MetodoPago == "nequi")
                {
                    if (string.IsNullOrWhiteSpace(request.NumeroCuentaNequi))
                    {
                        return BadRequest(new { error = "Número de Nequi requerido." });
                    }

                    if (!_paymentValidationService.ValidateNequi(request.NumeroCuentaNequi))
                    {
                        return BadRequest(new { error = "Número de Nequi inválido. Debe ser 10 dígitos comenzando con 3." });
                    }

                    _logger.LogInformation($"Pago Nequi procesado: {request.NumeroCuentaNequi} - Total: {request.Total}");
                }
                else if (request.MetodoPago == "tarjeta")
                {
                    if (string.IsNullOrWhiteSpace(request.NumeroTarjeta))
                    {
                        return BadRequest(new { error = "Número de tarjeta requerido." });
                    }

                    // Limpiar espacios y validar
                    string cardNumberClean = request.NumeroTarjeta.Replace(" ", "").Replace("-", "");
                    
                    if (!_paymentValidationService.ValidateCreditCardLuhn(cardNumberClean))
                    {
                        return BadRequest(new { error = "Número de tarjeta de crédito inválido (validación Luhn fallida)." });
                    }

                    _logger.LogInformation($"Pago Tarjeta procesado: {cardNumberClean.Substring(cardNumberClean.Length - 4).PadLeft(cardNumberClean.Length, '*')} - Total: {request.Total}");
                }
                else
                {
                    return BadRequest(new { error = "Método de pago no válido." });
                }

                // Generar código de confirmación
                string codigoConfirmacion = GenerarCodigoConfirmacion();

                // Aquí iría la lógica para guardar la transacción en BD
                // Por ahora, retornamos el código de confirmación

                return Ok(new
                {
                    success = true,
                    codigoConfirmacion = codigoConfirmacion,
                    fecha = DateTime.Now,
                    monto = request.Total,
                    metodoPago = request.MetodoPago
                });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al procesar pago: {ex.Message}");
                return StatusCode(500, new { error = "Error al procesar el pago. Intente más tarde." });
            }
        }

        /// <summary>
        /// Genera un código de confirmación único
        /// </summary>
        private string GenerarCodigoConfirmacion()
        {
            return $"PAGO-{DateTime.Now:yyyyMMddHHmmss}-{Random.Shared.Next(1000, 9999)}";
        }
    }

    /// <summary>
    /// Modelo para solicitud de procesamiento de pago
    /// </summary>
    public class ProcesarPagoRequest
    {
        public string MetodoPago { get; set; } = string.Empty;
        public string? NumeroCuentaNequi { get; set; }
        public string? NumeroTarjeta { get; set; }
        public decimal Subtotal { get; set; }
        public decimal IVA { get; set; }
        public decimal Total { get; set; }
    }
}
