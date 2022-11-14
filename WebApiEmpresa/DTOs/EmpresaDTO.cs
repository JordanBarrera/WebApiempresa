namespace WebApiEmpresa.DTOs
{
    public class EmpresaDTO
    {
        public int Id { get; set; }

        public string Nombre { get; set; }

        public DateTime FechaCreacion { get; set; }
        public List<OcupacionDTO> Ocupaciones { get; set; }
    }
}
