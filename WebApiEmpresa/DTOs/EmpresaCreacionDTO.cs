using System.ComponentModel.DataAnnotations;
using WebApiEmpresa.Validaciones;

namespace WebApiEmpresa.DTOs
{
    public class EmpresaCreacionDTO
    {
        [Required]
        [StringLength(maximumLength: 250, ErrorMessage = "El campo {0} solo puede tener hasta 250 caracteres")]
        [PrimeraLetraMayuscula]
        public string Nombre { get; set; }

        public DateTime FechaCreacion { get; set; }

        public List<int> EmpleadosIds { get; set; }
    }
}
