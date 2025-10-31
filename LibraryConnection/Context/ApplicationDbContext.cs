using LibraryConnection.DbSet;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace LibraryConnection.Context
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Client> clients { get; set; }
        public DbSet<Order> orders { get; set; }
        public DbSet<User> users { get; set; }
        public DbSet<Role> roles { get; set; }
        public DbSet<Category> categories { get; set; }
        public DbSet<Product> products { get; set; }
        public DbSet<Cart> carts { get; set; }
        public DbSet<CartItem> cart_items { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production";
            var useInMemory = (Environment.GetEnvironmentVariable("USE_INMEMORY_DB") ?? "0").Trim() == "1";

            if (useInMemory)
            {
                // Fallback en Desarrollo: base de datos en memoria para pruebas rápidas
                optionsBuilder.UseInMemoryDatabase("SnackGolDev");
                return;
            }

            var configBuilder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env}.json", optional: true, reloadOnChange: true);

            IConfigurationRoot configuration = configBuilder.Build();

            // Prefer Development override when present
            var connectionString = configuration.GetConnectionString("LocalHostConnection");
            if (!string.IsNullOrWhiteSpace(connectionString) && !connectionString.Contains("Timeout=", StringComparison.OrdinalIgnoreCase))
            {
                connectionString += ";Timeout=3"; // reducir espera en dev si el servidor no responde
            }

            optionsBuilder.UseNpgsql(connectionString, o => o.CommandTimeout(5));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("dev");
            base.OnModelCreating(modelBuilder);
            // Role
            modelBuilder.Entity<Role>(entity =>
            {
                entity.ToTable("roles");

                entity.HasKey(r => r.id);
                entity.Property(r => r.id).ValueGeneratedOnAdd();

                // 1 Role -> N Users
                entity.HasMany(r => r.users)
                      .WithOne(u => u.Role)
                      .HasForeignKey(u => u.id_role)
                      .IsRequired();
            });

            // User
            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("users");
                
                entity.HasKey(u => u.id);
                entity.Property(u => u.id).ValueGeneratedOnAdd();

                // las demás propiedades
                entity.Property(u => u.name).IsRequired();
                entity.Property(u => u.last_name).IsRequired();
                entity.Property(u => u.email).IsRequired();
                entity.Property(u => u.password).IsRequired();
            });

            modelBuilder.Entity<Client>(entity =>
            {
                entity.ToTable("clients");
                entity.HasMany(e => e.orders)
                      .WithOne(o => o.client)
                      .HasForeignKey(o => o.customer_id);
                entity.Property(e => e.document).ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.ToTable("orders");
                entity.HasKey(o=> o.order_id);
                entity.Property(o=> o.order_id).ValueGeneratedOnAdd();
                entity.HasOne(d => d.client)
                      .WithMany(p => p.orders)
                      .HasForeignKey(d => d.customer_id);
            });
          
            modelBuilder.Entity<OrderLine>(entity =>
            {
                entity.ToTable("orderLines");
                entity.HasKey(e => new { e.orderId, e.lineNum });

            });

            // Category
            modelBuilder.Entity<Category>(entity =>
            {
                entity.ToTable("categories");
                entity.HasKey(e => e.id);
                entity.Property(e => e.id).ValueGeneratedOnAdd();
                entity.Property(e => e.name).IsRequired();

                entity.HasMany(e => e.products)
                      .WithOne(p => p.Category)
                      .HasForeignKey(p => p.category_id)
                      .IsRequired();
            });

            // Product
            modelBuilder.Entity<Product>(entity =>
            {
                entity.ToTable("products");
                entity.HasKey(e => e.id);
                entity.Property(e => e.id).ValueGeneratedOnAdd();
                entity.Property(e => e.name).IsRequired();
                entity.Property(e => e.price).IsRequired();
                entity.Property(e => e.stock).IsRequired();

                entity.HasIndex(e => e.category_id);
                entity.HasIndex(e => e.is_active);
            });

            // Cart
            modelBuilder.Entity<Cart>(entity =>
            {
                entity.ToTable("carts");
                entity.HasKey(e => e.id);

                entity.HasMany(e => e.items)
                      .WithOne(i => i.Cart)
                      .HasForeignKey(i => i.cart_id)
                      .IsRequired()
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasIndex(e => e.user_id);
                entity.HasIndex(e => e.session_token);
            });

            // CartItem
            modelBuilder.Entity<CartItem>(entity =>
            {
                entity.ToTable("cart_items");
                entity.HasKey(e => e.id);
                entity.Property(e => e.id).ValueGeneratedOnAdd();

                entity.HasOne(e => e.Product)
                      .WithMany()
                      .HasForeignKey(e => e.product_id)
                      .IsRequired()
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasIndex(e => new { e.cart_id, e.product_id }).IsUnique();
            });

            // Seed: Categories
            modelBuilder.Entity<Category>().HasData(
                new Category { id = 1, name = "Bebidas", is_active = true },
                new Category { id = 2, name = "Snacks", is_active = true },
                new Category { id = 3, name = "Dulces", is_active = true }
            );

            // Seed: Products
            modelBuilder.Entity<Product>().HasData(
                new Product { id = 1, category_id = 1, name = "Agua mineral 600ml", description = "Botella de agua", price = 2000, stock = 100, image_url = "https://images.unsplash.com/photo-1548833793-71ad3875f2ac?w=800&q=80", is_active = true },
                new Product { id = 2, category_id = 1, name = "Gaseosa cola 500ml", description = "Bebida gaseosa", price = 2500, stock = 80, image_url = "https://images.unsplash.com/photo-1623446043588-739e8f6daba1?w=800&q=80", is_active = true },
                new Product { id = 3, category_id = 1, name = "Jugo natural 350ml", description = "Sabor naranja", price = 2200, stock = 60, image_url = "https://images.unsplash.com/photo-1542444459-db63c0d2c979?w=800&q=80", is_active = true },

                new Product { id = 4, category_id = 2, name = "Papitas clásicas 45g", description = "Snack salado", price = 1800, stock = 120, image_url = "https://images.unsplash.com/photo-1596461404969-9ae70d18e7d5?w=800&q=80", is_active = true },
                new Product { id = 5, category_id = 2, name = "Maní salado 50g", description = "Bolsa de maní", price = 1500, stock = 90, image_url = "https://images.unsplash.com/photo-1601000924815-65f0f41cd3ed?w=800&q=80", is_active = true },
                new Product { id = 6, category_id = 2, name = "Nachos 70g", description = "Con queso", price = 2300, stock = 70, image_url = "https://images.unsplash.com/photo-1604908554049-69a3a9c7f4ec?w=800&q=80", is_active = true },

                new Product { id = 7, category_id = 3, name = "Chocolate barra 40g", description = "70% cacao", price = 2000, stock = 50, image_url = "https://images.unsplash.com/photo-1548907040-4b7d48268e8b?w=800&q=80", is_active = true },
                new Product { id = 8, category_id = 3, name = "Gomitas 90g", description = "Frutales", price = 1600, stock = 65, image_url = "https://images.unsplash.com/photo-1589712230557-cf5f38f3d001?w=800&q=80", is_active = true },
                new Product { id = 9, category_id = 3, name = "Caramelos surtidos 100g", description = "Mix sabores", price = 1500, stock = 150, image_url = "https://images.unsplash.com/photo-1581323361970-989d7a9b6eb9?w=800&q=80", is_active = true }
            );

        }
    }
}