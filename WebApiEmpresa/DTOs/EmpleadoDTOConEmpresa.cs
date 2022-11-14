namespace WebApiEmpresa.DTOs
{
    public class EmpleadoDTOConEmpresa: GetEmpleadoDTO
    {
        public List<EmpresaDTO> Empresas { get; set; }
    }
}
