using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using LibraryConnection.DbSet;
using QRCoder;
using SkiaSharp;

namespace MSSnackGol.Services;

/// <summary>
/// Implementación del servicio de generación de códigos QR para pickup de órdenes.
/// </summary>
public class QRGeneratorService : IQRGeneratorService
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web)
    {
        PropertyNameCaseInsensitive = true
    };

    // Colores de marca SnackGol
    private static readonly byte[] BrandDarkColor = { 32, 35, 45 };
    private static readonly SKColor BrandOrange = new(255, 107, 53);

    /// <inheritdoc />
    public QRPickupArtifacts GeneratePickupArtifacts(string orderId, string pickupCode, string? sessionToken, IEnumerable<CartItem>? items)
    {
        var itemsList = items?.ToList() ?? new List<CartItem>();

        // Generar token seguro
        var token = Convert.ToHexString(RandomNumberGenerator.GetBytes(16));
        
        // Crear payload
        var payload = new PickupPayload(orderId, pickupCode, token, sessionToken, DateTime.UtcNow);
        var payloadJson = JsonSerializer.Serialize(payload, JsonOptions);
        var payloadBase64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(payloadJson));

        // Construir contenido legible para el QR
        var qrContent = BuildQRContent(orderId, pickupCode, itemsList);

        // Generar imagen QR con logo
        var qrImageBase64 = GenerateQRImageWithLogo(qrContent);

        return new QRPickupArtifacts(token, payloadBase64, qrImageBase64);
    }

    /// <inheritdoc />
    public string GenerateOrderId()
    {
        return $"ORD-{DateTime.UtcNow:yyyyMMddHHmmss}-{RandomNumberGenerator.GetInt32(1000, 9999)}";
    }

    /// <inheritdoc />
    public string GeneratePickupCode()
    {
        return $"SG-{DateTime.UtcNow:yyMMddHHmmss}-{RandomNumberGenerator.GetInt32(1000, 9999)}";
    }

    /// <inheritdoc />
    public string HashToken(string token)
    {
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(token));
        return Convert.ToHexString(bytes);
    }

    /// <inheritdoc />
    public bool ValidateToken(string token, string storedHash)
    {
        if (string.IsNullOrWhiteSpace(token) || string.IsNullOrWhiteSpace(storedHash))
            return false;

        var computedHash = HashToken(token);
        return string.Equals(computedHash, storedHash, StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>
    /// Construye el contenido textual que se codificará en el QR.
    /// </summary>
    private static string BuildQRContent(string orderId, string pickupCode, List<CartItem> items)
    {
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
        var totalFormatted = items.Sum(i => i.subtotal).ToString("C0", new CultureInfo("es-CO"));
        sb.AppendLine($"Total: {totalFormatted}");
        sb.AppendLine("Presenta este QR en SnackGol para retirar tu pedido.");

        return sb.ToString();
    }

    /// <summary>
    /// Genera la imagen del QR con el logo de SnackGol superpuesto.
    /// </summary>
    private static string GenerateQRImageWithLogo(string content)
    {
        using var qrGenerator = new QRCodeGenerator();
        var qrData = qrGenerator.CreateQrCode(content, QRCodeGenerator.ECCLevel.H);
        var pngQr = new PngByteQRCode(qrData).GetGraphic(20, BrandDarkColor, new byte[] { 255, 255, 255 }, true);

        using var logoBitmap = GenerateLogoBitmap(120, 120);
        var qrWithLogo = OverlayLogoOnQr(pngQr, logoBitmap);

        return Convert.ToBase64String(qrWithLogo);
    }

    /// <summary>
    /// Genera el bitmap del logo de SnackGol.
    /// </summary>
    private static SKBitmap GenerateLogoBitmap(int width, int height)
    {
        var bitmap = new SKBitmap(width, height);
        using var canvas = new SKCanvas(bitmap);
        canvas.Clear(SKColors.Transparent);

        using var paint = new SKPaint { IsAntialias = true };

        // Círculo de fondo naranja
        paint.Color = BrandOrange;
        canvas.DrawCircle(width / 2, height / 2, width / 2 - 5, paint);

        // Dona decorativa
        paint.Color = SKColors.Gold;
        canvas.DrawCircle(width / 2, height / 2 - 10, 15, paint);
        paint.Color = SKColors.OrangeRed;
        canvas.DrawCircle(width / 2, height / 2 - 10, 10, paint);

        // Ojos
        paint.Color = SKColors.White;
        canvas.DrawCircle(width / 2 - 5, height / 2 - 15, 2, paint);
        canvas.DrawCircle(width / 2 + 5, height / 2 - 15, 2, paint);
        canvas.DrawCircle(width / 2, height / 2 - 5, 2, paint);

        // Texto SnackGol
        paint.Color = SKColors.White;
        paint.TextSize = 12;
        paint.TextAlign = SKTextAlign.Center;
        canvas.DrawText("SnackGol", width / 2, height / 2 + 20, paint);
        paint.TextSize = 8;
        canvas.DrawText("¡Tu snack favorito!", width / 2, height / 2 + 35, paint);

        return bitmap;
    }

    /// <summary>
    /// Superpone el logo en el centro del código QR.
    /// </summary>
    private static byte[] OverlayLogoOnQr(byte[] qrBytes, SKBitmap logo)
    {
        using var qrStream = new MemoryStream(qrBytes);
        using var qrBitmap = SKBitmap.Decode(qrStream);
        using var canvas = new SKCanvas(qrBitmap);

        var logoSize = Math.Min(qrBitmap.Width, qrBitmap.Height) / 5f;
        var logoRect = new SKRect(
            (qrBitmap.Width - logoSize) / 2f,
            (qrBitmap.Height - logoSize) / 2f,
            (qrBitmap.Width + logoSize) / 2f,
            (qrBitmap.Height + logoSize) / 2f
        );

        var cornerRadius = logoSize * 0.25f;

        // Fondo blanco con bordes redondeados
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

        // Borde naranja
        using (var borderPaint = new SKPaint
        {
            Color = BrandOrange,
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
}
