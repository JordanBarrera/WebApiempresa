using AutoMapper;
using WebApiEmpresa.DTOs;
using WebApiEmpresa.Entidades;

namespace WebApiEmpresa.Utilidades
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<EmpleadoDTO, Empleado>();
            CreateMap<Empleado, GetEmpleadoDTO>();
            CreateMap<Empleado, EmpleadoDTOConEmpresa>()
                .ForMember(empleadoDTO => empleadoDTO.Empresas, opciones => opciones.MapFrom(MapEmpleadoDTOEmpresas));
            CreateMap<EmpresaCreacionDTO, Empresas>()
                .ForMember(empresa => empresa.EmpleadoEmpresas, opciones => opciones.MapFrom(MapEmpleadoEmpresa));
            CreateMap<Empresas, EmpresaDTO>();
            CreateMap<Empresas, EmpresaDTOConEmpleados>()
                .ForMember(empresaDTO => empresaDTO.Empleados, opciones => opciones.MapFrom(MapEmpleadoDTOEmpresas));
            CreateMap<EmpresaPatchDTO, Empresas>().ReverseMap();
            CreateMap<OcupacionCreacionDTO, Ocupacion>();
            CreateMap<Ocupacion, OcupacionDTO>();
        }

        private List<EmpresaDTO> MapEmpleadoDTOEmpresas(Empleado empleado, GetEmpleadoDTO getEmpleadoDTO)
        {
            var result = new List<EmpresaDTO>();

            if (empleado.EmpleadoEmpresas == null) { return result; }

            foreach (var empleadoEmpresa in empleado.EmpleadoEmpresas)
            {
                result.Add(new EmpresaDTO()
                {
                    Id = empleadoEmpresa.EmpresaId,
                    Nombre = empleadoEmpresa.Empresas.Nombre
                });
            }

            return result;
        }

        private List<GetEmpleadoDTO> MapEmpleadoDTOEmpresas(Empresas empresa, EmpresaDTO empresaDTO)
        {
            var result = new List<GetEmpleadoDTO>();

            if (empresa.EmpleadoEmpresas == null)
            {
                return result;
            }

            foreach (var empleadoEmpresa in empresa.EmpleadoEmpresas)
            {
                result.Add(new GetEmpleadoDTO()
                {
                    Id = empleadoEmpresa.EmpleadoId,
                    Nombre = empleadoEmpresa.Empleado.Nombre
                });
            }

            return result;
        }

        private List<EmpleadoEmpresas> MapEmpleadoEmpresa(EmpresaCreacionDTO empresaCreacionDTO, Empresas empresa)
        {
            var resultado = new List<EmpleadoEmpresas>();

            if (empresaCreacionDTO.EmpleadosIds == null) { return resultado; }
            foreach (var empleadoId in empresaCreacionDTO.EmpleadosIds)
            {
                resultado.Add(new EmpleadoEmpresas() { EmpleadoId = empleadoId });
            }
            return resultado;
        }
    }
}
