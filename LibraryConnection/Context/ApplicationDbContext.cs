using LibraryConnection.DbSet;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore.Storage;

namespace LibraryConnection.Context
{
    public class ApplicationDbContext : DbContext
    {
        private static readonly InMemoryDatabaseRoot InMemoryRoot = new();
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
                // Fallback en Desarrollo: base de datos en memoria para pruebas rápidas (compartida entre contextos)
                optionsBuilder.UseInMemoryDatabase("SnackGolDev", InMemoryRoot);
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
                new Product { id = 1, category_id = 1, name = "Agua mineral 600ml", description = "Botella de agua", price = 2000, stock = 100, image_url = "https://images.unsplash.com/photo-1637774139107-b83aae618551?ixlib=rb-4.1.0&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&q=80&w=735", is_active = true },
                new Product { id = 2, category_id = 1, name = "Gaseosa cola 500ml", description = "Bebida gaseosa", price = 2500, stock = 80, image_url = "https://images.unsplash.com/photo-1622708862830-a026e3ef60bd?ixlib=rb-4.1.0&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&q=80&w=682", is_active = true },
                new Product { id = 3, category_id = 1, name = "Jugo natural 350ml", description = "Sabor naranja", price = 2200, stock = 60, image_url = "https://images.unsplash.com/photo-1621506289894-c3a62d6be8f3?ixlib=rb-4.1.0&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&q=80&w=687", is_active = true },

                new Product { id = 4, category_id = 2, name = "Papitas clásicas 45g", description = "Snack salado", price = 1800, stock = 120, image_url = "https://images.unsplash.com/photo-1741520150134-0d60d82dfac9?ixlib=rb-4.1.0&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&q=80&w=1406", is_active = true },
                new Product { id = 5, category_id = 2, name = "Maní salado 50g", description = "Bolsa de maní", price = 1500, stock = 90, image_url = "https://www.istockphoto.com/photo/roasted-salted-peanuts-in-bowl-on-wooden-table-gm2208003368-625205923?utm_source=unsplash&utm_medium=affiliate&utm_campaign=srp_photos_bottom&utm_content=https%3A%2F%2Funsplash.com%2Fes%2Fs%2Ffotos%2Froasted-peanuts-salted&utm_term=roasted+peanuts+salted%3A%3Alayout-below-fold-units-2%3Acontrol", is_active = true },
                new Product { id = 6, category_id = 2, name = "Nachos 70g", description = "Con queso", price = 2300, stock = 70, image_url = "https://unsplash.com/es/fotos/una-pizza-con-verduras-y-queso-RV4-buXKOS8", is_active = true },

                new Product { id = 7, category_id = 3, name = "Chocolate barra 40g", description = "70% cacao", price = 2000, stock = 50, image_url = "https://unsplash.com/es/fotos/barra-de-chocolate-hersheys-sobre-superficie-blanca-7pvYgmkqOzc", is_active = true },
                new Product { id = 8, category_id = 3, name = "Gomitas 90g", description = "Frutales", price = 1600, stock = 65, image_url = "https://unsplash.com/es/fotos/un-monton-de-caramelos-de-diferentes-colores-sobre-una-superficie-azul-1hRz-_MIwbk", is_active = true },
                new Product { id = 9, category_id = 3, name = "Cupcakes surtidos", description = "Bandeja de cupcakes", price = 1800, stock = 40, image_url = "https://media.istockphoto.com/id/171360702/es/foto/vista-a%C3%A9rea-de-la-bandeja-con-cupcakes.webp?a=1&b=1&s=612x612&w=0&k=20&c=y9htsC84M1g1lu03oQYNBMIWqxMmWQr9_T7nGdk8hvA=", is_active = true }
            );

        }
    }
}