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

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
            var connectionString = configuration.GetConnectionString("LocalHostConnection");
            optionsBuilder.UseNpgsql(connectionString);
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
                entity.HasOne(u => u.Role)
                      .WithMany(r => r.users)
                      .HasForeignKey(u => u.id_role);
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

        }
    }
}