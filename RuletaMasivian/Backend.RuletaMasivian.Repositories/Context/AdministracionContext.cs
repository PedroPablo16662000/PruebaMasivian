using Backend.RuletaMasivian.Entities.ModelsAdmin;
using Microsoft.EntityFrameworkCore;

namespace Backend.RuletaMasivian.Repositories
{
    public partial class AdministracionContext : DbContext
    {
        public AdministracionContext()
        { 
        }

        public AdministracionContext(DbContextOptions<AdministracionContext> options): base(options)
        {
        }

        public virtual DbSet<Rol> Rol { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Rol>(entity =>
            {
                entity.HasKey(e => e.IdRol);

                entity.ToTable("Rol", "Seguridad");

                entity.Property(e => e.IdRol)
                    .HasColumnName("idRol")
                    .ValueGeneratedNever();

                entity.Property(e => e.Estado)
                    .IsRequired()
                    .HasColumnName("estado")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.FechaCrea).HasColumnType("datetime");

                entity.Property(e => e.FechaModi).HasColumnType("datetime");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasColumnName("nombre")
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });
        }

    }
}
