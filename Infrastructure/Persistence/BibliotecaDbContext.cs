using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Abstractions;

namespace Infrastructure.Persistence;

public class BibliotecaDbContext : DbContext
{
    public BibliotecaDbContext(DbContextOptions<BibliotecaDbContext> options) : base(options)
    {
    }

    public DbSet<Autor> Autores { get; set; }
    public DbSet<Libro> Libros { get; set; }
    public DbSet<Usuario> Usuarios { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Autor>(entity =>
        {
            entity.ToTable("Autores");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.NombreCompleto).IsRequired().HasMaxLength(200);
            entity.Property(e => e.CiudadProcedencia).IsRequired().HasMaxLength(100);
            entity.Property(e => e.CorreoElectronico).IsRequired().HasMaxLength(150);
            entity.Property(e => e.FechaNacimiento).IsRequired();
            entity.Property(e => e.FechaCreacion).HasDefaultValueSql("GETDATE()");
            entity.Property(e => e.FechaActualizacion).HasDefaultValueSql("GETDATE()");
            entity.Property(e => e.Activo).HasDefaultValue(true);

            entity.HasIndex(e => e.CorreoElectronico).IsUnique();
            entity.HasIndex(e => e.NombreCompleto);
            
            entity.HasMany(e => e.Libros)
                  .WithOne(l => l.Autor)
                  .HasForeignKey(l => l.AutorId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Libro>(entity =>
        {
            entity.ToTable("Libros");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Titulo).IsRequired().HasMaxLength(300);
            entity.Property(e => e.Genero).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Anio).IsRequired();
            entity.Property(e => e.NumeroPaginas).IsRequired();
            entity.Property(e => e.AutorId).IsRequired();
            entity.Property(e => e.FechaCreacion).HasDefaultValueSql("GETDATE()");
            entity.Property(e => e.FechaActualizacion).HasDefaultValueSql("GETDATE()");
            
            entity.HasIndex(e => e.AutorId);
            entity.HasIndex(e => e.Titulo);
            entity.HasIndex(e => e.Genero);
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.ToTable("Usuarios");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.NombreUsuario).IsRequired().HasMaxLength(100);
            entity.Property(e => e.CorreoElectronico).IsRequired().HasMaxLength(150);
            entity.Property(e => e.PasswordHash).IsRequired().HasMaxLength(500);
            entity.Property(e => e.Rol).IsRequired().HasMaxLength(50);
            entity.Property(e => e.FechaCreacion).HasDefaultValueSql("GETDATE()");
            entity.Property(e => e.FechaActualizacion).HasDefaultValueSql("GETDATE()");
            entity.Property(e => e.Activo).HasDefaultValue(true);
            
            entity.HasIndex(e => e.CorreoElectronico).IsUnique();
            entity.HasIndex(e => e.NombreUsuario).IsUnique();
        });
    }
}
