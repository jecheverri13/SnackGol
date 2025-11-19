using System.Net;
using System.Linq;
using FluentAssertions;
using LibraryConnection.Context;
using LibraryConnection.DbSet;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MSSnackGol.Controllers;
using SnackGol.Tests.Utilities;
using Xunit;
using Microsoft.EntityFrameworkCore;
using LibraryEntities.Models;

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
        var controller = new OrderManagementController(logger)
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

        result.Should().NotBeNull();
        result!.StatusCode.Should().Be((int)HttpStatusCode.Created);

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
        storedOrder.status.Should().Be("ReadyForPickup");
        storedOrder.pickup_code.Should().NotBeNullOrWhiteSpace();
        storedOrder.pickup_token_hash.Should().NotBeNullOrWhiteSpace();
        storedOrder.pickup_payload_base64.Should().NotBeNullOrWhiteSpace();
        storedOrder.pickup_qr_base64.Should().NotBeNullOrWhiteSpace();
        storedOrder.OrderLines.Should().HaveCount(1);
    }
}
