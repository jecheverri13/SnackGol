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
        private readonly ApplicationDbContext _db;
        private readonly ILogger<ProductManagmentController> _logger;

        public ProductManagmentController(ApplicationDbContext db, ILogger<ProductManagmentController> logger)
        {
            _db = db;
            _logger = logger;
        }

        /// <summary>
        /// Lista productos activos con filtros, búsqueda y paginación.
        /// </summary>
        [HttpGet("List")]
        public async Task<IActionResult> List([FromQuery] int? categoryId, [FromQuery] string? q, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
        {
            try
            {
                if (page < 1) page = 1;
                if (pageSize < 1) pageSize = 20;

                var query = _db.products.AsNoTracking().Where(p => p.is_active);
                if (categoryId.HasValue)
                    query = query.Where(p => p.category_id == categoryId.Value);
                if (!string.IsNullOrWhiteSpace(q))
                {
                    var term = q.Trim().ToLower();
                    query = query.Where(p => p.name.ToLower().Contains(term));
                }

                var total = await query.CountAsync();

                if (total == 0)
                    return StatusCode((int)HttpStatusCode.NoContent, new Response<List<ProductDto>>(true, HttpStatusCode.NoContent, "No products"));

                var items = await query
                    .OrderBy(p => p.name)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
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
                    .ToListAsync();

                var meta = new { totalItems = total, page, pageSize, totalPages = (int)Math.Ceiling(total / (double)pageSize) };
                var payload = new { items, meta };
                return Ok(new Response<dynamic>(true, HttpStatusCode.OK, payload));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error listing products");
                return StatusCode((int)HttpStatusCode.InternalServerError,
                    new Response<dynamic>(false, HttpStatusCode.InternalServerError, "ListProducts", ex.Message));
            }
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            try
            {
                var p = await _db.products.AsNoTracking().FirstOrDefaultAsync(p => p.id == id && p.is_active);
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
                _logger.LogError(ex, "Error getting product by id {ProductId}", id);
                return StatusCode((int)HttpStatusCode.InternalServerError,
                    new Response<dynamic>(false, HttpStatusCode.InternalServerError, "GetProduct", ex.Message));
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] LibraryConnection.Dtos.ProductCreateRequest body)
        {
            try
            {
                if (body == null)
                    return BadRequest(new Response<dynamic>(false, HttpStatusCode.BadRequest, "Invalid body"));

                var product = new LibraryConnection.DbSet.Product
                {
                    category_id = body.category_id,
                    name = body.name,
                    description = body.description,
                    price = body.price,
                    stock = body.stock,
                    image_url = body.image_url,
                    is_active = body.is_active
                };

                _db.products.Add(product);
                await _db.SaveChangesAsync();

                return StatusCode((int)HttpStatusCode.Created, new Response<ProductDto>(true, HttpStatusCode.Created, new ProductDto
                {
                    id = product.id,
                    category_id = product.category_id,
                    name = product.name,
                    description = product.description,
                    price = product.price,
                    stock = product.stock,
                    image_url = product.image_url,
                    is_active = product.is_active
                }));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating product");
                return StatusCode((int)HttpStatusCode.InternalServerError, new Response<dynamic>(false, HttpStatusCode.InternalServerError, "CreateProduct", ex.Message));
            }
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] LibraryConnection.Dtos.ProductUpdateRequest body)
        {
            try
            {
                var product = await _db.products.FirstOrDefaultAsync(p => p.id == id);
                if (product == null)
                    return StatusCode((int)HttpStatusCode.NotFound, new Response<dynamic>(false, HttpStatusCode.NotFound, "Product not found"));

                // Update fields
                product.category_id = body.category_id ?? product.category_id;
                product.name = body.name ?? product.name;
                product.description = body.description ?? product.description;
                product.price = body.price ?? product.price;
                product.stock = body.stock ?? product.stock;
                product.image_url = body.image_url ?? product.image_url;
                if (body.is_active.HasValue) product.is_active = body.is_active.Value;

                _db.products.Update(product);
                await _db.SaveChangesAsync();

                return Ok(new Response<ProductDto>(true, HttpStatusCode.OK, new ProductDto
                {
                    id = product.id,
                    category_id = product.category_id,
                    name = product.name,
                    description = product.description,
                    price = product.price,
                    stock = product.stock,
                    image_url = product.image_url,
                    is_active = product.is_active
                }));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating product {ProductId}", id);
                return StatusCode((int)HttpStatusCode.InternalServerError, new Response<dynamic>(false, HttpStatusCode.InternalServerError, "UpdateProduct", ex.Message));
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            try
            {
                var product = await _db.products.FirstOrDefaultAsync(p => p.id == id);
                if (product == null)
                    return StatusCode((int)HttpStatusCode.NotFound, new Response<dynamic>(false, HttpStatusCode.NotFound, "Product not found"));

                // Soft delete
                product.is_active = false;
                _db.products.Update(product);
                await _db.SaveChangesAsync();

                return Ok(new Response<dynamic>(true, HttpStatusCode.OK, "Deleted"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting product {ProductId}", id);
                return StatusCode((int)HttpStatusCode.InternalServerError, new Response<dynamic>(false, HttpStatusCode.InternalServerError, "DeleteProduct", ex.Message));
            }
        }
    }
}
