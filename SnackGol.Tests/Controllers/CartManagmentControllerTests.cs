using System.Net;
using FluentAssertions;
using LibraryConnection.Context;
using LibraryConnection.DbSet;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MSSnackGol.Controllers;
using SnackGol.Tests.Utilities;
using Xunit;

namespace SnackGol.Tests.Controllers;

public class CartManagmentControllerTests
{
    public CartManagmentControllerTests()
    {
        DbTestHelper.UseFreshInMemoryDb();
    }

    private static CartManagmentController CreateControllerWithSession(string session)
    {
        var controller = new CartManagmentController
        {
            ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            }
        };
        controller.Request.Headers["X-Session-Token"] = session;
        return controller;
    }

    // Verifica que si no existe carrito para el token, GetCart lo cree y
    // responda 200 OK con el contenido.
    [Fact]
    public void GetCart_CreatesCart_WhenMissing()
    {
        var controller = CreateControllerWithSession("s1");
        var result = controller.GetCart() as ObjectResult;
        result.Should().NotBeNull();
        result!.StatusCode.Should().Be((int)HttpStatusCode.OK);
    }

    // Verifica que al intentar agregar más unidades que el stock disponible,
    // el endpoint responde 409 Conflict.
    [Fact]
    public void AddItem_InsufficientStock_Returns409()
    {
        int createdProductId;
        using (var db = new ApplicationDbContext())
        {
            // Arrange: producto con stock insuficiente
            var cat = new Category { name = "Snacks", is_active = true };
            db.categories.Add(cat); db.SaveChanges();
            var prod = new Product
            {
                category_id = cat.id,
                name = "Papitas",
                description = "Snack",
                price = 1000,
                stock = 1,
                is_active = true
            };
            db.products.Add(prod);
            db.SaveChanges();
            createdProductId = prod.id;
        }

        var controller = CreateControllerWithSession("s2");
        var body = new LibraryConnection.Dtos.AddCartItemRequest
        {
            product_id = createdProductId,
            quantity = 5
        };
        var result = controller.AddItem(body) as ObjectResult;
        result.Should().NotBeNull();
        result!.StatusCode.Should().Be((int)HttpStatusCode.Conflict);
    }
}
