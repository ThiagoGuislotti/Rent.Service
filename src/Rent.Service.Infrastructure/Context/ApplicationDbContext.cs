using Microsoft.EntityFrameworkCore;
using NetToolsKit.Core.Domain.Events;
using NetToolsKit.Core.Domain.Notifications;
using NetToolsKit.Core.EntityFramework.Context;
using Rent.Service.Domain.Entities;

namespace Rent.Service.Infrastructure.Context
{
    public class ApplicationDbContext : DbContextBase
    {
        #region Propriedades DbSets
        public DbSet<DeliveryPerson> DeliveryPersons { get; set; }
        public DbSet<Motorcycle> Motorcycles { get; set; }
        public DbSet<MotorcycleEvent> MotorcycleEvents { get; set; }
        public DbSet<Rental> Rentals { get; set; }
        #endregion

        #region Construtores
        public ApplicationDbContext()
        {
        }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        #endregion

        #region Métodos/Operadores Públicos
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
            => base.ConcurrencySaveChangesAsync(true, cancellationToken);
        #endregion

        #region Métodos/Operadores Protegidos
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
                optionsBuilder.UseNpgsql("Host=localhost;Database=RentTestDb;Username=postgres;Password=NetToolsKit.Pass!;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region Entity Configuration
            modelBuilder.Ignore<Notification>();
            modelBuilder.Ignore<Event>();

            modelBuilder.Entity<DeliveryPerson>(builder =>
            {
                // Properties
                builder.Property(x => x.Cnpj)
                       .IsUnicode();
                builder.Property(x => x.DriverLicenseNumber)
                       .IsUnicode();

                // Unique Index
                builder.HasIndex(x => x.Cnpj)
                       .IsUnique();
                builder.HasIndex(x => x.DriverLicenseNumber)
                       .IsUnique();
            });

            modelBuilder.Entity<Motorcycle>(builder =>
            {
                // Properties
                builder.Property(x => x.LicensePlate)
                       .IsUnicode();

                // Unique Index
                builder.HasIndex(x => x.LicensePlate)
                       .IsUnique();
            });

            modelBuilder.Entity<Rental>(builder =>
            {
                // Foreign Key(s)
                builder.HasOne<DeliveryPerson>()
                       .WithMany()
                       .HasForeignKey(r => r.DeliveryPersonId)
                       .OnDelete(DeleteBehavior.Cascade);

                builder.HasOne<Motorcycle>()
                       .WithMany()
                       .HasForeignKey(r => r.MotorcycleId)
                       .OnDelete(DeleteBehavior.Cascade);
            });
            #endregion

            #region Timestamp Configuration
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                var properties = entityType.ClrType.GetProperties()
                    .Where(p => p.PropertyType == typeof(DateTime) 
                        || p.PropertyType == typeof(DateTime?) 
                        || p.PropertyType == typeof(DateTimeOffset) 
                        || p.PropertyType == typeof(DateTimeOffset?));

                foreach (var property in properties)
                        modelBuilder
                            .Entity(entityType.Name)
                            .Property(property.Name)
                            .HasColumnType("timestamp");
            }
            #endregion
        }
        #endregion
    }
}