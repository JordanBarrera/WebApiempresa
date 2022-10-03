using System.ComponentModel.DataAnnotations;


namespace WebApiEmpresa.Entidades
{
    public class Empleado
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        public string apellido { get; set; }
        public int EmpresaId { get; set; }
    }
}