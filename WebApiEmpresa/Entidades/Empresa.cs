using System.ComponentModel.DataAnnotations;

namespace WebApiEmpresa.Entidades
{
    public class Empresa
    {
        public int Id { get; set; }

        public string Nombre { get; set; }
        public List<Empleado> Empleados { get; set; }
    }
}