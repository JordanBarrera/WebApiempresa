﻿namespace WebApiEmpresa.Entidades
{
    public class EmpleadoEmpresas
    {
        public int EmpleadoId { get; set; }
        public int EmpresaId { get; set; }
        public  Empleado Empleado { get; set; }
        public Empresa Empresa { get; set; }
    }
}
