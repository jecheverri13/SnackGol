using System;
using System.Net;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using FluentAssertions;
using LibraryConnection.Context;
using LibraryConnection.DbSet;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MSSnackGol.Controllers;
using MSSnackGol.Services;
using SnackGol.Tests.Utilities;
using Xunit;
using Microsoft.EntityFrameworkCore;
using LibraryEntities.Models;
using Xunit.Sdk;

namespace SnackGol.Tests.Controllers;

public class OrderManagementControllerTests
{
    public OrderManagementControllerTests()
    {
        DbTestHelper.UseFreshInMemoryDb();
    }

    private static OrderManagementController CreateController()
    {
        var logger = new LoggerFactory().CreateLogger<OrderManagementController>();
        var qrService = new QRGeneratorService();
        var controller = new OrderManagementController(logger, qrService)
        {
            ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            }
        };
        return controller;
    }

    // Verifica que intentar finalizar compra sin carrito existente
    // retorna 404 Not Found.
    [Fact]
    public void Checkout_CartMissing_Returns404()
    {
        var controller = CreateController();
        var body = new LibraryEntities.Models.CheckoutRequest { session_token = "missing" };
        var result = controller.Checkout(body) as ObjectResult;
        result.Should().NotBeNull();
        result!.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
    }

    // Verifica el flujo feliz del checkout: responde 201 Created, vacía el carrito
    // y descuenta el stock del producto comprado.
    [Fact]
    public void Checkout_Success_EmptiesCart_And_DecrementsStock()
    {
        using (var db = new ApplicationDbContext())
        {
            // Arrange: datos mínimos (categoría, producto, carrito y un ítem)
            var cat = new Category { name = "Bebidas", is_active = true };
            db.categories.Add(cat); db.SaveChanges();

            var p = new Product { category_id = cat.id, name = "Agua_UnitTest", is_active = true, price = 1000, stock = 5 };
            db.products.Add(p); db.SaveChanges();

            var cart = new Cart { session_token = "sess-ok" };
            db.carts.Add(cart); db.SaveChanges();

            db.cart_items.Add(new CartItem { cart_id = cart.id, product_id = p.id, quantity = 2, unit_price = p.price, subtotal = p.price * 2 });
            db.SaveChanges();
        }
        var controller = CreateController();
        var body = new LibraryEntities.Models.CheckoutRequest { session_token = "sess-ok" };
        var result = controller.Checkout(body) as ObjectResult;

        AssertCreated(result);

        var envelope = result.Value as Response<dynamic>;
        envelope.Should().NotBeNull();
        envelope!.success.Should().BeTrue();

        var checkoutResult = envelope.response as CheckoutResult;
        checkoutResult.Should().NotBeNull();
        checkoutResult!.orderId.Should().NotBeNullOrWhiteSpace();
        checkoutResult.pickup.Should().NotBeNull();
        checkoutResult.pickup!.pickupCode.Should().NotBeNullOrWhiteSpace();
        checkoutResult.pickup.pickupQrImageBase64.Should().NotBeNullOrWhiteSpace();

        using var verify = new ApplicationDbContext();
        var cartVerify = verify.carts.Single(c => c.session_token == "sess-ok");
        verify.cart_items.Where(ci => ci.cart_id == cartVerify.id).Any().Should().BeFalse();
        verify.products.Single(x => x.name == "Agua_UnitTest").stock.Should().Be(3); // 5 - 2

        var storedOrder = verify.orders.Include(o => o.OrderLines).Single(o => o.order_id == checkoutResult.orderId);
        storedOrder.status.Should().Be("Confirmed"); // Estado inicial correcto: Confirmado
        storedOrder.pickup_code.Should().NotBeNullOrWhiteSpace();
        storedOrder.pickup_token_hash.Should().NotBeNullOrWhiteSpace();
        storedOrder.pickup_payload_base64.Should().NotBeNullOrWhiteSpace();
        storedOrder.pickup_qr_base64.Should().NotBeNullOrWhiteSpace();
        storedOrder.OrderLines.Should().HaveCount(1);
    }

    [Fact]
    public void Checkout_PickupPayloadContainsTokenAndMatchesHash()
    {
        var sessionToken = "sess-artifacts";
        SeedCartWithItem(sessionToken, quantity: 1);

        var controller = CreateController();
        var body = new LibraryEntities.Models.CheckoutRequest { session_token = sessionToken };
        var result = controller.Checkout(body) as ObjectResult;

        AssertCreated(result);

        var envelope = result.Value as Response<dynamic>;
        var checkoutResult = envelope!.response as CheckoutResult;
        checkoutResult.Should().NotBeNull();

        using var verify = new ApplicationDbContext();
        var storedOrder = verify.orders.Include(o => o.OrderLines).Single(o => o.order_id == checkoutResult!.orderId);
        storedOrder.pickup_payload_base64.Should().NotBeNullOrWhiteSpace();

        var payload = DecodePayload(storedOrder.pickup_payload_base64!);
        payload.Should().NotBeNull();
        payload!.OrderId.Should().Be(storedOrder.order_id);
        payload.PickupCode.Should().Be(storedOrder.pickup_code);
        payload.SessionToken.Should().Be(sessionToken);
        payload.Token.Should().NotBeNullOrWhiteSpace();

        var expectedHash = Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(payload.Token)));
        storedOrder.pickup_token_hash.Should().Be(expectedHash);
    }

    [Fact]
    public void ValidatePickup_SucceedsOnceAndPreventsReentry()
    {
        var sessionToken = "sess-validate";
        SeedCartWithItem(sessionToken, quantity: 2);

        var controller = CreateController();
        var checkoutBody = new LibraryEntities.Models.CheckoutRequest { session_token = sessionToken };
        var checkoutResult = controller.Checkout(checkoutBody) as ObjectResult;
        AssertCreated(checkoutResult);

        var envelope = checkoutResult!.Value as Response<dynamic>;
        var checkout = envelope!.response as CheckoutResult;
        checkout.Should().NotBeNull();

        var firstPayload = FetchOrderPayload(checkout!.orderId);
        var validateRequest = new PickupValidationRequest
        {
            token = firstPayload.Token,
            verified_by = "unit-tester"
        };

        var validateResult = controller.ValidatePickup(checkout.orderId, validateRequest) as ObjectResult;
        validateResult.Should().NotBeNull();
        validateResult!.StatusCode.Should().Be((int)HttpStatusCode.OK);

        using (var verify = new ApplicationDbContext())
        {
            var order = verify.orders.Single(o => o.order_id == checkout.orderId);
            order.status.Should().Be("Delivered");
            order.pickup_redeemed_at.Should().NotBeNull();
        }

        var secondAttempt = controller.ValidatePickup(checkout.orderId, validateRequest) as ObjectResult;
        secondAttempt.Should().NotBeNull();
        secondAttempt!.StatusCode.Should().Be((int)HttpStatusCode.Conflict);
    }

    private static void SeedCartWithItem(string sessionToken, int quantity)
    {
        using var db = new ApplicationDbContext();
        var category = new Category { name = $"Bebidas-{Guid.NewGuid()}" , is_active = true };
        db.categories.Add(category);
        db.SaveChanges();

        var product = new Product { category_id = category.id, name = $"Producto_{Guid.NewGuid()}", is_active = true, price = 1200, stock = 10 };
        db.products.Add(product);
        db.SaveChanges();

        var cart = new Cart { session_token = sessionToken };
        db.carts.Add(cart);
        db.SaveChanges();

        db.cart_items.Add(new CartItem { cart_id = cart.id, product_id = product.id, quantity = quantity, unit_price = product.price, subtotal = product.price * quantity });
        db.SaveChanges();
    }

    private static PickupPayloadDto FetchOrderPayload(string orderId)
    {
        using var db = new ApplicationDbContext();
        var order = db.orders.Single(o => o.order_id == orderId);
        order.pickup_payload_base64.Should().NotBeNull();
        return DecodePayload(order.pickup_payload_base64!)!;
    }

    private static PickupPayloadDto? DecodePayload(string payloadBase64)
    {
        var payloadBytes = Convert.FromBase64String(payloadBase64);
        var json = Encoding.UTF8.GetString(payloadBytes);
        return JsonSerializer.Deserialize<PickupPayloadDto>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
    }

    private sealed record PickupPayloadDto(string OrderId, string PickupCode, string Token, string? SessionToken, DateTime GeneratedAtUtc);

    private static void AssertCreated(ObjectResult? result)
    {
        result.Should().NotBeNull();
        if (result!.StatusCode != (int)HttpStatusCode.Created)
        {
            var envelope = result.Value as Response<dynamic>;
            var reason = envelope?.errors ?? "sin detalle";
            throw new XunitException($"Checkout devolvió {result.StatusCode}: {reason}");
        }
    }
}
