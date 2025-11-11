using LibraryConnection.Context;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LibraryConnection.Dtos;
using LibraryEntities.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Security.Claims;
using DbModels = LibraryConnection.DbSet;

namespace MSSnackGol.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartManagmentController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private readonly ILogger<CartManagmentController> _logger;

        public CartManagmentController(ApplicationDbContext db, ILogger<CartManagmentController> logger)
        {
            _db = db;
            _logger = logger;
        }

        private string? GetSessionToken() =>
            Request.Headers.TryGetValue("X-Session-Token", out var v) ? v.ToString() : null;

        private long? GetUserId()
        {
            if (User?.Identity?.IsAuthenticated == true)
            {
                var idClaim = User.FindFirst(ClaimTypes.NameIdentifier) ?? User.FindFirst("sub");
                if (idClaim != null && long.TryParse(idClaim.Value, out var id))
                    return id;
            }
            return null;
        }

    private static double SumTotal(IEnumerable<CartItemDto> items) => items.Sum(i => i.subtotal);

        private static CartDto MapCart(DbModels.Cart cart, List<(DbModels.CartItem ci, DbModels.Product p)> pairs)
        {
            var items = pairs.Select(x => new CartItemDto
            {
                id = x.ci.id,
                product_id = x.p.id,
                name = x.p.name,
                description = x.p.description,
                unit_price = x.ci.unit_price,
                quantity = x.ci.quantity,
                subtotal = x.ci.subtotal
            }).ToList();
            return new CartDto
            {
                id = cart.id,
                items = items,
                count = items.Sum(i => i.quantity),
                total = SumTotal(items)
            };
        }

        [HttpGet]
        public async Task<IActionResult> GetCart()
        {
            var userId = GetUserId();
            var session = GetSessionToken();

            var cart = await _db.carts.FirstOrDefaultAsync(c => (userId != null && c.user_id == userId) || (session != null && c.session_token == session));
            if (cart == null)
            {
                cart = new DbModels.Cart { user_id = userId, session_token = session };
                _db.carts.Add(cart);
                await _db.SaveChangesAsync();
            }

            var pairs = await _db.cart_items.Where(i => i.cart_id == cart.id)
                .Join(_db.products, i => i.product_id, p => p.id, (i, p) => new { i, p })
                .ToListAsync();

            var tupleList = pairs.Select(x => (x.i, x.p)).ToList();
            var dto = MapCart(cart, tupleList);
            return Ok(new Response<CartDto>(true, HttpStatusCode.OK, dto));
        }

        [HttpPost("Items")]
        public async Task<IActionResult> AddItem([FromBody] AddCartItemRequest body)
        {
            if (body == null || body.product_id <= 0 || body.quantity <= 0)
                return StatusCode((int)HttpStatusCode.BadRequest, new Response<dynamic>(false, HttpStatusCode.BadRequest, "Parámetros inválidos"));

            var userId = GetUserId();
            var session = body.session_token ?? GetSessionToken();

            var cart = await _db.carts.FirstOrDefaultAsync(c => (userId != null && c.user_id == userId) || (session != null && c.session_token == session));
            if (cart == null)
            {
                cart = new DbModels.Cart { user_id = userId, session_token = session };
                _db.carts.Add(cart);
                await _db.SaveChangesAsync();
            }

            var product = await _db.products.FirstOrDefaultAsync(p => p.id == body.product_id && p.is_active);
            if (product == null)
                return StatusCode((int)HttpStatusCode.NotFound, new Response<dynamic>(false, HttpStatusCode.NotFound, "Producto no encontrado"));
            if (product.stock < body.quantity)
                return StatusCode((int)HttpStatusCode.Conflict, new Response<dynamic>(false, HttpStatusCode.Conflict, "Stock insuficiente"));

            var item = await _db.cart_items.FirstOrDefaultAsync(i => i.cart_id == cart.id && i.product_id == product.id);
            if (item == null)
            {
                item = new DbModels.CartItem
                {
                    cart_id = cart.id,
                    product_id = product.id,
                    quantity = body.quantity,
                    unit_price = product.price,
                    subtotal = product.price * body.quantity
                };
                _db.cart_items.Add(item);
            }
            else
            {
                var newQty = item.quantity + body.quantity;
                if (newQty > product.stock)
                    return StatusCode((int)HttpStatusCode.Conflict, new Response<dynamic>(false, HttpStatusCode.Conflict, "Stock insuficiente"));
                item.quantity = newQty;
                item.unit_price = product.price;
                item.subtotal = item.unit_price * item.quantity;
                _db.cart_items.Update(item);
            }

            cart.updated_at = DateTime.UtcNow;
            await _db.SaveChangesAsync();

            var pairs = await _db.cart_items.Where(i => i.cart_id == cart.id)
                .Join(_db.products, i => i.product_id, p => p.id, (i, p) => new { i, p })
                .ToListAsync();
            var tupleList = pairs.Select(x => (x.i, x.p)).ToList();
            var dto = MapCart(cart, tupleList);
            return Ok(new Response<CartDto>(true, HttpStatusCode.OK, dto));
        }

        [HttpPatch("Items/{id:int}")]
        public async Task<IActionResult> UpdateItem([FromRoute] int id, [FromBody] UpdateCartItemRequest body)
        {
            if (body == null || body.quantity < 0)
                return StatusCode((int)HttpStatusCode.BadRequest, new Response<dynamic>(false, HttpStatusCode.BadRequest, "Parámetros inválidos"));

            var userId = GetUserId();
            var session = GetSessionToken();

            var item = await _db.cart_items.Include(i => i.Cart).FirstOrDefaultAsync(i => i.id == id);
            if (item == null)
                return StatusCode((int)HttpStatusCode.NotFound, new Response<dynamic>(false, HttpStatusCode.NotFound, "Ítem no encontrado"));

            if (!((userId != null && item.Cart.user_id == userId) || (session != null && item.Cart.session_token == session)))
                return StatusCode((int)HttpStatusCode.Forbidden, new Response<dynamic>(false, HttpStatusCode.Forbidden, "No autorizado"));

            if (body.quantity == 0)
            {
                _db.cart_items.Remove(item);
            }
            else
            {
                var product = await _db.products.FirstAsync(p => p.id == item.product_id);
                if (product.stock < body.quantity)
                    return StatusCode((int)HttpStatusCode.Conflict, new Response<dynamic>(false, HttpStatusCode.Conflict, "Stock insuficiente"));
                item.quantity = body.quantity;
                item.unit_price = product.price;
                item.subtotal = product.price * item.quantity;
            }

            item.Cart.updated_at = DateTime.UtcNow;
            await _db.SaveChangesAsync();

            var cart = await _db.carts.FindAsync(item.cart_id)!;
            var pairs = await _db.cart_items.Where(i => i.cart_id == cart.id)
                .Join(_db.products, i => i.product_id, p => p.id, (i, p) => new { i, p })
                .ToListAsync();
            var tupleList = pairs.Select(x => (x.i, x.p)).ToList();
            var dto = MapCart(cart, tupleList);
            return Ok(new Response<CartDto>(true, HttpStatusCode.OK, dto));
        }

        [HttpDelete("Items/{id:int}")]
        public async Task<IActionResult> DeleteItem([FromRoute] int id)
        {
            var userId = GetUserId();
            var session = GetSessionToken();

            var item = await _db.cart_items.Include(i => i.Cart).FirstOrDefaultAsync(i => i.id == id);
            if (item == null)
                return StatusCode((int)HttpStatusCode.NotFound, new Response<dynamic>(false, HttpStatusCode.NotFound, "Ítem no encontrado"));

            if (!((userId != null && item.Cart.user_id == userId) || (session != null && item.Cart.session_token == session)))
                return StatusCode((int)HttpStatusCode.Forbidden, new Response<dynamic>(false, HttpStatusCode.Forbidden, "No autorizado"));

            var cartId = item.cart_id;
            _db.cart_items.Remove(item);
            await _db.SaveChangesAsync();

            var cart = await _db.carts.FindAsync(cartId)!;
            var pairs = await _db.cart_items.Where(i => i.cart_id == cart.id)
                .Join(_db.products, i => i.product_id, p => p.id, (i, p) => new { i, p })
                .ToListAsync();
            var tupleList = pairs.Select(x => (x.i, x.p)).ToList();
            var dto = MapCart(cart, tupleList);
            return Ok(new Response<CartDto>(true, HttpStatusCode.OK, dto));
        }

        [HttpPost("Sample")]
        public async Task<IActionResult> AddSampleData()
        {
            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production";
            if (!string.Equals(env, "Development", StringComparison.OrdinalIgnoreCase))
            {
                return StatusCode((int)HttpStatusCode.Forbidden, new Response<dynamic>(false, HttpStatusCode.Forbidden, "Solo disponible en Development"));
            }

            var userId = GetUserId();
            var session = GetSessionToken();

            var cart = await _db.carts.FirstOrDefaultAsync(c => (userId != null && c.user_id == userId) || (session != null && c.session_token == session));
            if (cart == null)
            {
                cart = new DbModels.Cart { user_id = userId, session_token = session };
                _db.carts.Add(cart);
                await _db.SaveChangesAsync();
            }

            // Pick first 3 active products; if none exist, create sample categories/products
            var sampleProducts = await _db.products.AsNoTracking().Where(p => p.is_active).OrderBy(p => p.id).Take(3).ToListAsync();
            if (sampleProducts.Count == 0)
            {
                // Ensure some categories
                if (!await _db.categories.AnyAsync())
                {
                    _db.categories.AddRange(new DbModels.Category { name = "Bebidas", is_active = true },
                                           new DbModels.Category { name = "Snacks", is_active = true },
                                           new DbModels.Category { name = "Dulces", is_active = true });
                    await _db.SaveChangesAsync();
                }

                var catBeb = await _db.categories.FirstAsync(c => c.name == "Bebidas");
                var catSna = await _db.categories.FirstAsync(c => c.name == "Snacks");
                var catDul = await _db.categories.FirstAsync(c => c.name == "Dulces");

                _db.products.AddRange(
                    new DbModels.Product { category_id = catBeb.id, name = "Agua mineral 600ml", description = "Botella de agua", price = 1.20, stock = 100, image_url = "https://images.unsplash.com/photo-1548833793-71ad3875f2ac?w=800&q=80", is_active = true },
                    new DbModels.Product { category_id = catSna.id, name = "Papitas clásicas 45g", description = "Snack salado", price = 1.00, stock = 120, image_url = "https://images.unsplash.com/photo-1596461404969-9ae70d18e7d5?w=800&q=80", is_active = true },
                    new DbModels.Product { category_id = catDul.id, name = "Chocolate barra 40g", description = "70% cacao", price = 1.10, stock = 50, image_url = "https://images.unsplash.com/photo-1548907040-4b7d48268e8b?w=800&q=80", is_active = true }
                );
                await _db.SaveChangesAsync();

                sampleProducts = await _db.products.AsNoTracking().Where(p => p.is_active).OrderBy(p => p.id).Take(3).ToListAsync();
            }

            var qty = new[] { 1, 2, 1 };
            for (int i = 0; i < sampleProducts.Count; i++)
            {
                var p = sampleProducts[i];
                var q = qty[i % qty.Length];

                var existing = await _db.cart_items.FirstOrDefaultAsync(ci => ci.cart_id == cart.id && ci.product_id == p.id);
                if (existing == null)
                {
                    var toAdd = new DbModels.CartItem
                    {
                        cart_id = cart.id,
                        product_id = p.id,
                        quantity = Math.Min(q, p.stock > 0 ? p.stock : q),
                        unit_price = p.price,
                        subtotal = p.price * Math.Min(q, p.stock > 0 ? p.stock : q)
                    };
                    _db.cart_items.Add(toAdd);
                }
                else
                {
                    var newQty = existing.quantity + q;
                    if (p.stock > 0 && newQty > p.stock) newQty = p.stock;
                    existing.quantity = newQty;
                    existing.unit_price = p.price;
                    existing.subtotal = existing.unit_price * existing.quantity;
                }
            }

            cart.updated_at = DateTime.UtcNow;
            await _db.SaveChangesAsync();

            var pairs = await _db.cart_items.Where(i => i.cart_id == cart.id)
                .Join(_db.products, i => i.product_id, p => p.id, (i, p) => new { i, p })
                .ToListAsync();
            var tupleList = pairs.Select(x => (x.i, x.p)).ToList();
            var dto = MapCart(cart, tupleList);
            return Ok(new Response<CartDto>(true, HttpStatusCode.OK, dto));
        }
    }
}
