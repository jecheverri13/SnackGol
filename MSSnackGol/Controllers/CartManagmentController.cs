using LibraryConnection.Context;
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
        public IActionResult GetCart()
        {
            try
            {
                using var db = new ApplicationDbContext();
                var userId = GetUserId();
                var session = GetSessionToken();

                var cart = db.carts.FirstOrDefault(c => (userId != null && c.user_id == userId) || (session != null && c.session_token == session));
                if (cart == null)
                {
                    cart = new DbModels.Cart { user_id = userId, session_token = session };
                    db.carts.Add(cart);
                    db.SaveChanges();
                }

                var pairs = db.cart_items.Where(i => i.cart_id == cart.id)
                    .Join(db.products, i => i.product_id, p => p.id, (i, p) => new { i, p })
                    .ToList()
                    .Select(x => (x.i, x.p))
                    .ToList();

                var dto = MapCart(cart, pairs);
                return Ok(new Response<CartDto>(true, HttpStatusCode.OK, dto));
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new Response<dynamic>(false, HttpStatusCode.InternalServerError, "GetCart", ex.Message));
            }
        }

        [HttpPost("Items")]
        public IActionResult AddItem([FromBody] AddCartItemRequest body)
        {
            if (body == null || body.product_id <= 0 || body.quantity <= 0)
                return StatusCode((int)HttpStatusCode.BadRequest, new Response<dynamic>(false, HttpStatusCode.BadRequest, "Parámetros inválidos"));

            try
            {
                using var db = new ApplicationDbContext();
                var userId = GetUserId();
                var session = body.session_token ?? GetSessionToken();

                var cart = db.carts.FirstOrDefault(c => (userId != null && c.user_id == userId) || (session != null && c.session_token == session));
                if (cart == null)
                {
                    cart = new DbModels.Cart { user_id = userId, session_token = session };
                    db.carts.Add(cart);
                    db.SaveChanges();
                }

                var product = db.products.FirstOrDefault(p => p.id == body.product_id && p.is_active);
                if (product == null)
                    return StatusCode((int)HttpStatusCode.NotFound, new Response<dynamic>(false, HttpStatusCode.NotFound, "Producto no encontrado"));
                if (product.stock < body.quantity)
                    return StatusCode((int)HttpStatusCode.Conflict, new Response<dynamic>(false, HttpStatusCode.Conflict, "Stock insuficiente"));

                var item = db.cart_items.FirstOrDefault(i => i.cart_id == cart.id && i.product_id == product.id);
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
                    db.cart_items.Add(item);
                }
                else
                {
                    var newQty = item.quantity + body.quantity;
                    if (newQty > product.stock)
                        return StatusCode((int)HttpStatusCode.Conflict, new Response<dynamic>(false, HttpStatusCode.Conflict, "Stock insuficiente"));
                    item.quantity = newQty;
                    item.unit_price = product.price;
                    item.subtotal = item.unit_price * item.quantity;
                    db.cart_items.Update(item);
                }

                cart.updated_at = DateTime.UtcNow;
                db.SaveChanges();

                var pairs = db.cart_items.Where(i => i.cart_id == cart.id)
                    .Join(db.products, i => i.product_id, p => p.id, (i, p) => new { i, p })
                    .ToList()
                    .Select(x => (x.i, x.p))
                    .ToList();
                var dto = MapCart(cart, pairs);
                return Ok(new Response<CartDto>(true, HttpStatusCode.OK, dto));
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new Response<dynamic>(false, HttpStatusCode.InternalServerError, "AddItem", ex.Message));
            }
        }

        [HttpPatch("Items/{id:int}")]
        public IActionResult UpdateItem([FromRoute] int id, [FromBody] UpdateCartItemRequest body)
        {
            if (body == null || body.quantity < 0)
                return StatusCode((int)HttpStatusCode.BadRequest, new Response<dynamic>(false, HttpStatusCode.BadRequest, "Parámetros inválidos"));
            try
            {
                using var db = new ApplicationDbContext();
                var userId = GetUserId();
                var session = GetSessionToken();

                var item = db.cart_items.Include(i => i.Cart).FirstOrDefault(i => i.id == id);
                if (item == null)
                    return StatusCode((int)HttpStatusCode.NotFound, new Response<dynamic>(false, HttpStatusCode.NotFound, "Ítem no encontrado"));

                if (!((userId != null && item.Cart.user_id == userId) || (session != null && item.Cart.session_token == session)))
                    return StatusCode((int)HttpStatusCode.Forbidden, new Response<dynamic>(false, HttpStatusCode.Forbidden, "No autorizado"));

                if (body.quantity == 0)
                {
                    db.cart_items.Remove(item);
                }
                else
                {
                    var product = db.products.First(p => p.id == item.product_id);
                    if (product.stock < body.quantity)
                        return StatusCode((int)HttpStatusCode.Conflict, new Response<dynamic>(false, HttpStatusCode.Conflict, "Stock insuficiente"));
                    item.quantity = body.quantity;
                    item.unit_price = product.price;
                    item.subtotal = product.price * item.quantity;
                }

                item.Cart.updated_at = DateTime.UtcNow;
                db.SaveChanges();

                var cart = db.carts.Find(item.cart_id)!;
                var pairs = db.cart_items.Where(i => i.cart_id == cart.id)
                    .Join(db.products, i => i.product_id, p => p.id, (i, p) => new { i, p })
                    .ToList()
                    .Select(x => (x.i, x.p))
                    .ToList();
                var dto = MapCart(cart, pairs);
                return Ok(new Response<CartDto>(true, HttpStatusCode.OK, dto));
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new Response<dynamic>(false, HttpStatusCode.InternalServerError, "UpdateItem", ex.Message));
            }
        }

        [HttpDelete("Items/{id:int}")]
        public IActionResult DeleteItem([FromRoute] int id)
        {
            try
            {
                using var db = new ApplicationDbContext();
                var userId = GetUserId();
                var session = GetSessionToken();

                var item = db.cart_items.Include(i => i.Cart).FirstOrDefault(i => i.id == id);
                if (item == null)
                    return StatusCode((int)HttpStatusCode.NotFound, new Response<dynamic>(false, HttpStatusCode.NotFound, "Ítem no encontrado"));

                if (!((userId != null && item.Cart.user_id == userId) || (session != null && item.Cart.session_token == session)))
                    return StatusCode((int)HttpStatusCode.Forbidden, new Response<dynamic>(false, HttpStatusCode.Forbidden, "No autorizado"));

                var cartId = item.cart_id;
                db.cart_items.Remove(item);
                db.SaveChanges();

                var cart = db.carts.Find(cartId)!;
                var pairs = db.cart_items.Where(i => i.cart_id == cart.id)
                    .Join(db.products, i => i.product_id, p => p.id, (i, p) => new { i, p })
                    .ToList()
                    .Select(x => (x.i, x.p))
                    .ToList();
                var dto = MapCart(cart, pairs);
                return Ok(new Response<CartDto>(true, HttpStatusCode.OK, dto));
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new Response<dynamic>(false, HttpStatusCode.InternalServerError, "DeleteItem", ex.Message));
            }
        }

        [HttpPost("Sample")]
        public IActionResult AddSampleData()
        {
            try
            {
                var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production";
                if (!string.Equals(env, "Development", StringComparison.OrdinalIgnoreCase))
                {
                    return StatusCode((int)HttpStatusCode.Forbidden, new Response<dynamic>(false, HttpStatusCode.Forbidden, "Solo disponible en Development"));
                }

                using var db = new ApplicationDbContext();
                var userId = GetUserId();
                var session = GetSessionToken();

                var cart = db.carts.FirstOrDefault(c => (userId != null && c.user_id == userId) || (session != null && c.session_token == session));
                if (cart == null)
                {
                    cart = new DbModels.Cart { user_id = userId, session_token = session };
                    db.carts.Add(cart);
                    db.SaveChanges();
                }

                // Pick first 3 active products; if none exist, create sample categories/products
                var sampleProducts = db.products.AsNoTracking().Where(p => p.is_active).OrderBy(p => p.id).Take(3).ToList();
                if (sampleProducts.Count == 0)
                {
                    // Ensure some categories
                    if (!db.categories.Any())
                    {
                        db.categories.AddRange(new DbModels.Category { name = "Bebidas", is_active = true },
                                               new DbModels.Category { name = "Snacks", is_active = true },
                                               new DbModels.Category { name = "Dulces", is_active = true });
                        db.SaveChanges();
                    }

                    var catBeb = db.categories.First(c => c.name == "Bebidas");
                    var catSna = db.categories.First(c => c.name == "Snacks");
                    var catDul = db.categories.First(c => c.name == "Dulces");

                    db.products.AddRange(
                        new DbModels.Product { category_id = catBeb.id, name = "Agua mineral 600ml", description = "Botella de agua", price = 1.20, stock = 100, image_url = "https://images.unsplash.com/photo-1548833793-71ad3875f2ac?w=800&q=80", is_active = true },
                        new DbModels.Product { category_id = catSna.id, name = "Papitas clásicas 45g", description = "Snack salado", price = 1.00, stock = 120, image_url = "https://images.unsplash.com/photo-1596461404969-9ae70d18e7d5?w=800&q=80", is_active = true },
                        new DbModels.Product { category_id = catDul.id, name = "Chocolate barra 40g", description = "70% cacao", price = 1.10, stock = 50, image_url = "https://images.unsplash.com/photo-1548907040-4b7d48268e8b?w=800&q=80", is_active = true }
                    );
                    db.SaveChanges();

                    sampleProducts = db.products.AsNoTracking().Where(p => p.is_active).OrderBy(p => p.id).Take(3).ToList();
                }

                var qty = new[] { 1, 2, 1 };
                for (int i = 0; i < sampleProducts.Count; i++)
                {
                    var p = sampleProducts[i];
                    var q = qty[i % qty.Length];

                    var existing = db.cart_items.FirstOrDefault(ci => ci.cart_id == cart.id && ci.product_id == p.id);
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
                        db.cart_items.Add(toAdd);
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
                db.SaveChanges();

                var pairs = db.cart_items.Where(i => i.cart_id == cart.id)
                    .Join(db.products, i => i.product_id, p => p.id, (i, p) => new { i, p })
                    .ToList()
                    .Select(x => (x.i, x.p))
                    .ToList();
                var dto = MapCart(cart, pairs);
                return Ok(new Response<CartDto>(true, HttpStatusCode.OK, dto));
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new Response<dynamic>(false, HttpStatusCode.InternalServerError, "SampleData", ex.Message));
            }
        }
    }
}
