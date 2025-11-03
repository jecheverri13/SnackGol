using LibraryConnection.Context;
using LibraryConnection.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LibraryEntities.Models;
using System.Net;

namespace MSSnackGol.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductManagmentController : ControllerBase
    {
        [HttpGet("List")]
        public IActionResult List([FromQuery] int? categoryId, [FromQuery] string? q)
        {
            try
            {
                using var db = new ApplicationDbContext();

                var query = db.products.AsNoTracking().Where(p => p.is_active);
                if (categoryId.HasValue)
                    query = query.Where(p => p.category_id == categoryId.Value);
                if (!string.IsNullOrWhiteSpace(q))
                {
                    var term = q.Trim().ToLower();
                    query = query.Where(p => p.name.ToLower().Contains(term));
                }

                var items = query
                    .OrderBy(p => p.name)
                    .Select(p => new ProductDto
                    {
                        id = p.id,
                        category_id = p.category_id,
                        name = p.name,
                        description = p.description,
                        price = p.price,
                        stock = p.stock,
                        image_url = p.image_url,
                        is_active = p.is_active
                    })
                    .ToList();

                if (items.Count == 0)
                    return StatusCode((int)HttpStatusCode.NoContent, new Response<List<ProductDto>>(true, HttpStatusCode.NoContent, "No products"));

                return Ok(new Response<List<ProductDto>>(true, HttpStatusCode.OK, items));
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError,
                    new Response<dynamic>(false, HttpStatusCode.InternalServerError, "ListProducts", ex.Message));
            }
        }

        [HttpGet("{id:int}")]
        public IActionResult GetById([FromRoute] int id)
        {
            try
            {
                using var db = new ApplicationDbContext();
                var p = db.products.AsNoTracking().FirstOrDefault(p => p.id == id && p.is_active);
                if (p == null)
                    return StatusCode((int)HttpStatusCode.NotFound, new Response<ProductDto>(false, HttpStatusCode.NotFound, "Product not found"));

                var dto = new ProductDto
                {
                    id = p.id,
                    category_id = p.category_id,
                    name = p.name,
                    description = p.description,
                    price = p.price,
                    stock = p.stock,
                    image_url = p.image_url,
                    is_active = p.is_active
                };
                return Ok(new Response<ProductDto>(true, HttpStatusCode.OK, dto));
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError,
                    new Response<dynamic>(false, HttpStatusCode.InternalServerError, "GetProduct", ex.Message));
            }
        }
    }
}
