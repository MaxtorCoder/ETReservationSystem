using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using ReserveringSysteem.Database.Models;

namespace ReserveringSysteem.Database
{
    public class ReserveringContext : DbContext
    {
        public DbSet<BedrijfsModel> Bedrijf { get; set; }
        public DbSet<ReserveringsModel> Reservering { get; set; }
        public DbSet<VestigingsModel> Vestiging { get; set; }

        public ReserveringContext(DbContextOptions<ReserveringContext> options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<BedrijfsModel>(entity =>
            {
                entity.ToTable("bedrijven");

                entity.HasKey(e => new { e.ID })
                    .HasName("PK_Bedrijven");

                entity.Property(e => e.Naam)
                    .HasColumnType("varchar")
                    .HasMaxLength(255);

                entity.Property(e => e.Adress)
                    .HasColumnType("varchar")
                    .HasMaxLength(255);

                entity.Property(e => e.BTWNummer)
                    .HasColumnType("varchar")
                    .HasMaxLength(255);

                entity.Property(e => e.KVKNummer)
                    .HasColumnType("varchar")
                    .HasMaxLength(255);
            });

            builder.Entity<ReserveringsModel>(entity =>
            {
                entity.ToTable("reserveringen");

                entity.HasKey(e => new { e.ReserveringID })
                    .HasName("PK_Reserveringen");

                entity.Property(e => e.NaamReserverende)
                    .HasColumnType("varchar")
                    .HasMaxLength(255);

                entity.Property(e => e.TelefoonNummer)
                    .HasColumnType("varchar")
                    .HasMaxLength(255);

                entity.Property(e => e.Tijd)
                    .HasColumnType("varchar")
                    .HasMaxLength(255);

                entity.HasOne(e => e.Vestiging)
                    .WithMany(d => d.Reservering)
                    .HasForeignKey(e => e.ID)
                    .HasConstraintName("FK__reservering_id__vestiging_id");
            });

            builder.Entity<VestigingsModel>(entity =>
            {
                entity.ToTable("vestigingen");

                entity.HasKey(e => new { e.ID })
                    .HasName("PK_Vestiging");

                entity.Property(e => e.Naam)
                    .HasColumnType("varchar")
                    .HasMaxLength(255);
            });

            base.OnModelCreating(builder);
        }
    }

    public class ReserveringContextFactory : IDesignTimeDbContextFactory<ReserveringContext>
    {
        public ReserveringContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ReserveringContext>()
                .UseSqlServer(DatabaseManager.ConnectionString);
            return new(optionsBuilder.Options);
        }
    }
}
