using System.ComponentModel.DataAnnotations;
using WebApiEmpresa.Validaciones;
namespace WebApiEmpresa.DTOs
{
    public class EmpleadoDTO
    {

        [Required(ErrorMessage = "El campo {0} es requerido")] //
        [StringLength(maximumLength: 150, ErrorMessage = "El campo {0} solo puede tener hasta 150 caracteres")]
        [PrimeraLetraMayuscula]
        public string Nombre { get; set; }
    }
}
