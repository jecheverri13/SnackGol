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
        /*
          <summary>
          Procesa un pago validando el método de pago seleccionado.
          
          Flujo:
          1. Valida que los datos del formulario sean correctos
          2. Si es Nequi: valida que tenga 10 dígitos y empiece con 3
          3. Si es Tarjeta: valida el algoritmo de Luhn
          4. Genera un código de confirmación único
          5. Retorna el código junto con los detalles de la transacción
          
          Ejemplo de respuesta exitosa:
          {
            "success": true,
            "codigoConfirmacion": "PAGO-20251201120000-5432",
            "fecha": "2025-12-01T12:00:00",
            "monto": 10948.0,
            "metodoPago": "nequi"
          }
          </summary>
          <param name="request">Datos del pago a procesar (metodoPago, numeroCuentaNequi/numeroTarjeta, total)</param>
          <returns>Código de confirmación si es exitoso, o BadRequest con error específico</returns>
        */
        [HttpPost("procesar")]
        public IActionResult ProcesarPago([FromBody] ProcesarPagoRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { error = "Datos del pago inválidos." });
            }

            try
            {
                if (request.MetodoPago == "nequi")
                {
                    if (string.IsNullOrWhiteSpace(request.NumeroCuentaNequi))
                    {
                        return BadRequest(new { error = "Número de Nequi requerido." });
                    }

                    if (!_paymentValidationService.ValidateNequi(request.NumeroCuentaNequi))
                    {
                        return BadRequest(new { error = "Número de Nequi inválido." });
                    }

                    _logger.LogInformation($"Pago Nequi procesado: {request.NumeroCuentaNequi} - Total: {request.Total}");
                }
                else if (request.MetodoPago == "tarjeta")
                {
                    if (string.IsNullOrWhiteSpace(request.NumeroTarjeta))
                    {
                        return BadRequest(new { error = "Número de tarjeta requerido." });
                    }

                    string cardNumberClean = request.NumeroTarjeta.Replace(" ", "").Replace("-", "");
                    
                    if (!_paymentValidationService.ValidateCreditCardLuhn(cardNumberClean))
                    {
                        return BadRequest(new { error = "Número de tarjeta de crédito inválido." });
                    }

                    _logger.LogInformation($"Pago Tarjeta procesado: {cardNumberClean.Substring(cardNumberClean.Length - 4).PadLeft(cardNumberClean.Length, '*')} - Total: {request.Total}");
                }
                else
                {
                    return BadRequest(new { error = "Método de pago no válido." });
                }

                string codigoConfirmacion = GenerarCodigoConfirmacion();

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
        /*
          <summary>
          Genera un código de confirmación único con formato: PAGO-yyyyMMddHHmmss-xxxx
          
          Ejemplo:
          GenerarCodigoConfirmacion() → "PAGO-20251201120000-5432"
          
          Componentes:
          - Prefijo: "PAGO-"
          - Timestamp: Formato yyyyMMddHHmmss (año, mes, día, hora, minuto, segundo)
          - Sufijo: Número aleatorio entre 1000-9999
          </summary>
          <returns>Código de confirmación formateado</returns>
          */
        private string GenerarCodigoConfirmacion()
        {
            return $"PAGO-{DateTime.Now:yyyyMMddHHmmss}-{Random.Shared.Next(1000, 9999)}";
        }
    }
    /*
      <summary>
      Modelo para solicitud de procesamiento de pago
      </summary>
      */
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
