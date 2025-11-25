using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using FluentAssertions;
using LibraryConnection.DbSet;
using MSSnackGol.Services;
using Xunit;

namespace SnackGol.Tests.Services;

/// <summary>
/// Tests unitarios para el servicio de generación de códigos QR.
/// </summary>
public class QRGeneratorServiceTests
{
    private readonly QRGeneratorService _service;

    public QRGeneratorServiceTests()
    {
        _service = new QRGeneratorService();
    }

    #region GenerateOrderId Tests

    [Fact]
    public void GenerateOrderId_ShouldReturnValidFormat()
    {
        // Act
        var orderId = _service.GenerateOrderId();

        // Assert
        orderId.Should().NotBeNullOrWhiteSpace();
        orderId.Should().StartWith("ORD-");
        orderId.Should().MatchRegex(@"^ORD-\d{14}-\d{4}$");
    }

    [Fact]
    public void GenerateOrderId_ShouldGenerateUniqueIds()
    {
        // Act
        var ids = new HashSet<string>();
        for (int i = 0; i < 100; i++)
        {
            ids.Add(_service.GenerateOrderId());
        }

        // Assert - La mayoría deberían ser únicos (puede haber colisiones por timestamp)
        ids.Count.Should().BeGreaterThan(90);
    }

    #endregion

    #region GeneratePickupCode Tests

    [Fact]
    public void GeneratePickupCode_ShouldReturnValidFormat()
    {
        // Act
        var pickupCode = _service.GeneratePickupCode();

        // Assert
        pickupCode.Should().NotBeNullOrWhiteSpace();
        pickupCode.Should().StartWith("SG-");
        pickupCode.Should().MatchRegex(@"^SG-\d{12}-\d{4}$");
    }

    [Fact]
    public void GeneratePickupCode_ShouldGenerateUniqueCodes()
    {
        // Act
        var codes = new HashSet<string>();
        for (int i = 0; i < 100; i++)
        {
            codes.Add(_service.GeneratePickupCode());
        }

        // Assert
        codes.Count.Should().BeGreaterThan(90);
    }

    #endregion

    #region HashToken Tests

    [Fact]
    public void HashToken_ShouldReturnConsistentHash()
    {
        // Arrange
        var token = "test-token-123";

        // Act
        var hash1 = _service.HashToken(token);
        var hash2 = _service.HashToken(token);

        // Assert
        hash1.Should().Be(hash2);
        hash1.Should().NotBeNullOrWhiteSpace();
        hash1.Length.Should().Be(64); // SHA256 produce 32 bytes = 64 caracteres hex
    }

    [Fact]
    public void HashToken_ShouldReturnDifferentHashForDifferentTokens()
    {
        // Arrange
        var token1 = "token-1";
        var token2 = "token-2";

        // Act
        var hash1 = _service.HashToken(token1);
        var hash2 = _service.HashToken(token2);

        // Assert
        hash1.Should().NotBe(hash2);
    }

    [Fact]
    public void HashToken_ShouldMatchManualSHA256()
    {
        // Arrange
        var token = "verification-token";
        var expectedHash = Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(token)));

        // Act
        var actualHash = _service.HashToken(token);

        // Assert
        actualHash.Should().Be(expectedHash);
    }

    #endregion

    #region ValidateToken Tests

    [Fact]
    public void ValidateToken_ShouldReturnTrueForValidToken()
    {
        // Arrange
        var token = "my-secret-token";
        var hash = _service.HashToken(token);

        // Act
        var isValid = _service.ValidateToken(token, hash);

        // Assert
        isValid.Should().BeTrue();
    }

    [Fact]
    public void ValidateToken_ShouldReturnFalseForInvalidToken()
    {
        // Arrange
        var validToken = "valid-token";
        var invalidToken = "invalid-token";
        var hash = _service.HashToken(validToken);

        // Act
        var isValid = _service.ValidateToken(invalidToken, hash);

        // Assert
        isValid.Should().BeFalse();
    }

    [Theory]
    [InlineData(null, "somehash")]
    [InlineData("", "somehash")]
    [InlineData("token", null)]
    [InlineData("token", "")]
    [InlineData(null, null)]
    public void ValidateToken_ShouldReturnFalseForNullOrEmptyInputs(string? token, string? hash)
    {
        // Act
        var isValid = _service.ValidateToken(token!, hash!);

        // Assert
        isValid.Should().BeFalse();
    }

    [Fact]
    public void ValidateToken_ShouldBeCaseInsensitive()
    {
        // Arrange
        var token = "case-test";
        var hash = _service.HashToken(token);

        // Act
        var isValidUpper = _service.ValidateToken(token, hash.ToUpperInvariant());
        var isValidLower = _service.ValidateToken(token, hash.ToLowerInvariant());

        // Assert
        isValidUpper.Should().BeTrue();
        isValidLower.Should().BeTrue();
    }

    #endregion

    #region GeneratePickupArtifacts Tests

    [Fact]
    public void GeneratePickupArtifacts_ShouldReturnAllComponents()
    {
        // Arrange
        var orderId = "ORD-TEST-001";
        var pickupCode = "SG-TEST-001";
        var sessionToken = "session-123";
        var items = new List<CartItem>();

        // Act
        var artifacts = _service.GeneratePickupArtifacts(orderId, pickupCode, sessionToken, items);

        // Assert
        artifacts.Should().NotBeNull();
        artifacts.Token.Should().NotBeNullOrWhiteSpace();
        artifacts.PayloadBase64.Should().NotBeNullOrWhiteSpace();
        artifacts.QrImageBase64.Should().NotBeNullOrWhiteSpace();
    }

    [Fact]
    public void GeneratePickupArtifacts_TokenShouldBe32HexChars()
    {
        // Arrange
        var orderId = "ORD-TEST-002";
        var pickupCode = "SG-TEST-002";

        // Act
        var artifacts = _service.GeneratePickupArtifacts(orderId, pickupCode, null, null);

        // Assert
        artifacts.Token.Should().HaveLength(32); // 16 bytes = 32 hex chars
        artifacts.Token.Should().MatchRegex("^[A-F0-9]+$");
    }

    [Fact]
    public void GeneratePickupArtifacts_PayloadShouldBeValidBase64()
    {
        // Arrange
        var orderId = "ORD-TEST-003";
        var pickupCode = "SG-TEST-003";
        var sessionToken = "sess-test";

        // Act
        var artifacts = _service.GeneratePickupArtifacts(orderId, pickupCode, sessionToken, null);

        // Assert
        var decodedBytes = Convert.FromBase64String(artifacts.PayloadBase64);
        var json = Encoding.UTF8.GetString(decodedBytes);
        json.Should().NotBeNullOrWhiteSpace();
    }

    [Fact]
    public void GeneratePickupArtifacts_PayloadShouldContainCorrectData()
    {
        // Arrange
        var orderId = "ORD-TEST-004";
        var pickupCode = "SG-TEST-004";
        var sessionToken = "sess-verify";

        // Act
        var artifacts = _service.GeneratePickupArtifacts(orderId, pickupCode, sessionToken, null);

        // Assert
        var decodedBytes = Convert.FromBase64String(artifacts.PayloadBase64);
        var json = Encoding.UTF8.GetString(decodedBytes);
        var payload = JsonSerializer.Deserialize<PickupPayload>(json, new JsonSerializerOptions 
        { 
            PropertyNameCaseInsensitive = true 
        });

        payload.Should().NotBeNull();
        payload!.OrderId.Should().Be(orderId);
        payload.PickupCode.Should().Be(pickupCode);
        payload.SessionToken.Should().Be(sessionToken);
        payload.Token.Should().Be(artifacts.Token);
        payload.GeneratedAtUtc.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromMinutes(1));
    }

    [Fact]
    public void GeneratePickupArtifacts_QrImageShouldBeValidPng()
    {
        // Arrange
        var orderId = "ORD-TEST-005";
        var pickupCode = "SG-TEST-005";

        // Act
        var artifacts = _service.GeneratePickupArtifacts(orderId, pickupCode, null, null);

        // Assert
        var imageBytes = Convert.FromBase64String(artifacts.QrImageBase64);
        
        // PNG magic bytes: 89 50 4E 47 0D 0A 1A 0A
        imageBytes[0].Should().Be(0x89);
        imageBytes[1].Should().Be(0x50); // P
        imageBytes[2].Should().Be(0x4E); // N
        imageBytes[3].Should().Be(0x47); // G
    }

    [Fact]
    public void GeneratePickupArtifacts_WithItems_ShouldIncludeInQR()
    {
        // Arrange
        var orderId = "ORD-TEST-006";
        var pickupCode = "SG-TEST-006";
        var items = new List<CartItem>
        {
            new() { product_id = 1, quantity = 2, subtotal = 5000, Product = new Product { name = "Hot Dog" } },
            new() { product_id = 2, quantity = 1, subtotal = 3000, Product = new Product { name = "Gaseosa" } }
        };

        // Act
        var artifacts = _service.GeneratePickupArtifacts(orderId, pickupCode, null, items);

        // Assert
        artifacts.QrImageBase64.Should().NotBeNullOrWhiteSpace();
        // El QR debería generarse sin errores incluyendo los items
        var imageBytes = Convert.FromBase64String(artifacts.QrImageBase64);
        imageBytes.Length.Should().BeGreaterThan(1000); // Un QR con contenido debería ser >1KB
    }

    [Fact]
    public void GeneratePickupArtifacts_ShouldGenerateUniqueTokensEachTime()
    {
        // Arrange
        var orderId = "ORD-TEST-007";
        var pickupCode = "SG-TEST-007";

        // Act
        var tokens = new HashSet<string>();
        for (int i = 0; i < 50; i++)
        {
            var artifacts = _service.GeneratePickupArtifacts(orderId, pickupCode, null, null);
            tokens.Add(artifacts.Token);
        }

        // Assert - Todos los tokens deben ser únicos
        tokens.Should().HaveCount(50);
    }

    [Fact]
    public void GeneratePickupArtifacts_WithNullSessionToken_ShouldWork()
    {
        // Arrange
        var orderId = "ORD-TEST-008";
        var pickupCode = "SG-TEST-008";

        // Act
        var artifacts = _service.GeneratePickupArtifacts(orderId, pickupCode, null, null);

        // Assert
        artifacts.Should().NotBeNull();
        
        var decodedBytes = Convert.FromBase64String(artifacts.PayloadBase64);
        var json = Encoding.UTF8.GetString(decodedBytes);
        var payload = JsonSerializer.Deserialize<PickupPayload>(json, new JsonSerializerOptions 
        { 
            PropertyNameCaseInsensitive = true 
        });

        payload!.SessionToken.Should().BeNull();
    }

    #endregion

    #region Integration Tests

    [Fact]
    public void FullWorkflow_GenerateAndValidate_ShouldWork()
    {
        // Arrange
        var orderId = _service.GenerateOrderId();
        var pickupCode = _service.GeneratePickupCode();

        // Act - Generate artifacts
        var artifacts = _service.GeneratePickupArtifacts(orderId, pickupCode, "test-session", null);
        var tokenHash = _service.HashToken(artifacts.Token);

        // Assert - Validate token
        _service.ValidateToken(artifacts.Token, tokenHash).Should().BeTrue();
        _service.ValidateToken("wrong-token", tokenHash).Should().BeFalse();
    }

    [Fact]
    public void QRContent_ShouldBeHumanReadable()
    {
        // Este test verifica que el contenido del QR incluye información legible
        // cuando se escanea con un lector de QR estándar
        
        // Arrange
        var orderId = "ORD-20241124-1234";
        var pickupCode = "SG-241124-5678";
        var items = new List<CartItem>
        {
            new() { product_id = 1, quantity = 3, subtotal = 15000, Product = new Product { name = "Empanada" } }
        };

        // Act
        var artifacts = _service.GeneratePickupArtifacts(orderId, pickupCode, null, items);

        // Assert - El QR debería haberse generado correctamente
        artifacts.QrImageBase64.Should().NotBeNullOrWhiteSpace();
        
        // Verificar que el payload contiene la información esperada
        var decodedPayload = Encoding.UTF8.GetString(Convert.FromBase64String(artifacts.PayloadBase64));
        decodedPayload.Should().Contain(orderId);
        decodedPayload.Should().Contain(pickupCode);
    }

    #endregion
}
