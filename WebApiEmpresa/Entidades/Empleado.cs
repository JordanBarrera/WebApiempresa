using System.ComponentModel.DataAnnotations;


namespace WebApiEmpresa.Entidades
{
    public class Empleado
    {
        public int Id { get; set; }

        public string Nombre { get; set; }
        
        public string apellido { get; set; }


        public int EmpresaId { get; set; }
    }
}