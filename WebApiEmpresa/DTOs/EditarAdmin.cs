using System.ComponentModel.DataAnnotations;

namespace WebApiEmpresa.DTOs
{
    public class EditarAdminDTO
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}