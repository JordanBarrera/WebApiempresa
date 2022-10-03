
using Microsoft.EntityFrameworkCore;
using WebApiEmpresa.Entidades;
namespace WebApiEmpresa
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<Empresa> Empresas { get; set; }
        public DbSet<Empleado> Empleados { get; set; }
    }
}