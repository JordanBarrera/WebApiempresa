using System.ComponentModel.DataAnnotations;

namespace WebApiEmpresa.Entidades
{
    public class Empresas
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        public string Nombre { get; set; }
        public List<EmpleadoEmpresas> EmpleadoEmpresas { get; set; }
        public List<Ocupacion> Ocupacion { get; set; }
    }
}
