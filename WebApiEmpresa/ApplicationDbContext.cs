
using Microsoft.EntityFrameworkCore;
using WebApiEmpresa.Entidades;
namespace WebApiEmpresa
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<Empresa> Empresa { get; set; }
        public DbSet<Empleado> Empleado { get; set; }
    }
}