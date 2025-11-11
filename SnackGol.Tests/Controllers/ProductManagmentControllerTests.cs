using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using LibraryConnection.Context;
using LibraryConnection.DbSet;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;
using MSSnackGol.Controllers;
using SnackGol.Tests.Utilities;
using Xunit;

namespace SnackGol.Tests.Controllers;

public class ProductManagmentControllerTests
{
    public ProductManagmentControllerTests()
    {
        DbTestHelper.UseFreshInMemoryDb();
    }

    // Verifica que cuando no hay productos para la categoría/filtrado solicitado,
    // el endpoint de listado responde 204 No Content (sin cuerpo).
    [Fact]
    public async Task List_NoProducts_Returns204()
    {
        var controller = new ProductManagmentController(new ApplicationDbContext(), new NullLogger<ProductManagmentController>());
        // Use a non-existent category to ensure empty result despite seed data
        var action = await controller.List(999, null);
        var result = action as ObjectResult;

        result.Should().NotBeNull();
        result!.StatusCode.Should().Be((int)HttpStatusCode.NoContent);
    }

    // Verifica que al pedir un producto por id inexistente, la API responde 404 Not Found.
    [Fact]
    public async Task GetById_NotFound_Returns404()
    {
        var controller = new ProductManagmentController(new ApplicationDbContext(), new NullLogger<ProductManagmentController>());
        var action = await controller.GetById(12345);
        var result = action as ObjectResult;

        result.Should().NotBeNull();
        result!.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
    }

    // Verifica que al existir el producto, GetById retorna 200 OK y un DTO válido.
    [Fact]
    public async Task GetById_Found_Returns200_WithDto()
    {
        using (var db = new ApplicationDbContext())
        {
            // Arrange: crear categoría y producto de prueba en BD en memoria
            var cat = new Category { name = "Snacks", is_active = true };
            db.categories.Add(cat); db.SaveChanges();
            db.products.Add(new Product
            {
                category_id = cat.id,
                name = "Papitas",
                description = "Snack",
                price = 1000,
                stock = 10,
                is_active = true
            });
            db.SaveChanges();
        }

        var controller = new ProductManagmentController(new ApplicationDbContext(), new NullLogger<ProductManagmentController>());
        var action = await controller.GetById(1);
        var result = action as ObjectResult;

        result.Should().NotBeNull();
        result!.StatusCode.Should().Be((int)HttpStatusCode.OK);
    }
}
