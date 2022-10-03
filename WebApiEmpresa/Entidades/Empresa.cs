using System.ComponentModel.DataAnnotations;

namespace WebApiEmpresa.Entidades
{
    public class Empresa
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        public string Nombre { get; set; }
        public List<Empleado> Empleados { get; set; }
    }
}