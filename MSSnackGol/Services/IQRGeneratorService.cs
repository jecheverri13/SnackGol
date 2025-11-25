using LibraryConnection.DbSet;

namespace MSSnackGol.Services;

/// <summary>
/// Interfaz para el servicio de generación de códigos QR de pickup.
/// </summary>
public interface IQRGeneratorService
{
    /// <summary>
    /// Genera los artefactos del QR de pickup: token, payload en base64 y la imagen QR en base64.
    /// </summary>
    /// <param name="orderId">Identificador único de la orden.</param>
    /// <param name="pickupCode">Código de recogida.</param>
    /// <param name="sessionToken">Token de sesión del cliente (opcional).</param>
    /// <param name="items">Items del carrito para mostrar en el QR.</param>
    /// <returns>Tupla con Token, PayloadBase64 y QrImageBase64.</returns>
    QRPickupArtifacts GeneratePickupArtifacts(string orderId, string pickupCode, string? sessionToken, IEnumerable<CartItem>? items);

    /// <summary>
    /// Genera un ID de orden único.
    /// </summary>
    string GenerateOrderId();

    /// <summary>
    /// Genera un código de pickup único.
    /// </summary>
    string GeneratePickupCode();

    /// <summary>
    /// Genera el hash SHA256 de un token.
    /// </summary>
    /// <param name="token">Token a hashear.</param>
    /// <returns>Hash en formato hexadecimal.</returns>
    string HashToken(string token);

    /// <summary>
    /// Valida si un token coincide con su hash almacenado.
    /// </summary>
    /// <param name="token">Token a validar.</param>
    /// <param name="storedHash">Hash almacenado en la base de datos.</param>
    /// <returns>True si el token es válido.</returns>
    bool ValidateToken(string token, string storedHash);
}

/// <summary>
/// Artefactos generados para el QR de pickup.
/// </summary>
public record QRPickupArtifacts(
    string Token,
    string PayloadBase64,
    string QrImageBase64
);

/// <summary>
/// Payload que se codifica en el QR.
/// </summary>
public record PickupPayload(
    string OrderId,
    string PickupCode,
    string Token,
    string? SessionToken,
    DateTime GeneratedAtUtc
);
