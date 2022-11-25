namespace WebApiEmpresa.Entidades
{
    public class Ocupacion
    {
        public int Id { get; set; }
        public string Puesto { get; set; }
        public int EmpresaId { get; set; }
        public Empresas Empresa { get; set; }

    }
}
