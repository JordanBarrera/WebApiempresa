
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebApiEmpresa.Entidades;
namespace WebApiEmpresa
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<EmpleadoEmpresas>()
                .HasKey(al => new { al.EmpleadoId, al.EmpresaId });
        }

        public DbSet<Empleado> Empleado { get; set; }
        public DbSet<Empresas> Empresas { get; set; }
        public DbSet<Ocupacion> Ocupacion { get; set; }

        public DbSet<EmpleadoEmpresas> EmpleadoEmpresa { get; set; }
    }
}