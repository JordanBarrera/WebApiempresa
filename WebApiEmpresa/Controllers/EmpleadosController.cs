using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiEmpresa;
using WebApiEmpresa.DTOs;
using WebApiEmpresa.Entidades;
namespace WebApiMascota2.Controllers
{
    [ApiController]
    [Route("empleado")]
    public class Empresa : ControllerBase
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IMapper mapper;

        public Empresa(ApplicationDbContext context, IMapper mapper, IConfiguration configuration)
        {
            this.dbContext = context;
            this.mapper = mapper;
        }


        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<List<GetEmpleadoDTO>>> Get()
        {
            var empleado = await dbContext.Empleado.ToListAsync();
            return mapper.Map<List<GetEmpleadoDTO>>(empleado);
        }


        [HttpGet("{id:int}", Name = "obtenerempleado")]
        public async Task<ActionResult<EmpleadoDTOConEmpresa>> Get(int id)
        {
            var empleado = await dbContext.Empleado
                .Include(empleadoDB => empleadoDB.EmpleadoEmpresas)
                .ThenInclude(empleadoEmpresaDB => empleadoEmpresaDB.Empresas)
                .FirstOrDefaultAsync(empleadoDB => empleadoDB.Id == id);

            if (empleado == null)
            {
                return NotFound();
            }

            return mapper.Map<EmpleadoDTOConEmpresa>(empleado);

        }

        [HttpGet("{nombre}")]
        public async Task<ActionResult<List<GetEmpleadoDTO>>> Get([FromRoute] string nombre)
        {
            var empleados = await dbContext.Empleado.Where(empleadosDB => empleadosDB.Nombre.Contains(nombre)).ToListAsync();

            return mapper.Map<List<GetEmpleadoDTO>>(empleados);

        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] EmpleadoDTO empleadoDto)
        {

            var existeEmpleadoMismoNombre = await dbContext.Empleado.AnyAsync(x => x.Nombre == empleadoDto.Nombre);

            if (existeEmpleadoMismoNombre)
            {
                return BadRequest($"Ya existe un autor con el nombre {empleadoDto.Nombre}");
            }

            var empleado = mapper.Map<Empleado>(empleadoDto);

            dbContext.Add(empleado);
            await dbContext.SaveChangesAsync();

            var empleadoDTO = mapper.Map<GetEmpleadoDTO>(empleado);

            return CreatedAtRoute("obtenerEmpleado", new { id = empleado.Id }, empleadoDTO);
        }

        [HttpPut("{id:int}")] // api/empleado/1
        public async Task<ActionResult> Put(EmpleadoDTO empleadoCreacionDTO, int id)
        {
            var exist = await dbContext.Empleado.AnyAsync(x => x.Id == id);
            if (!exist)
            {
                return NotFound();
            }

            var empleado = mapper.Map<Empleado>(empleadoCreacionDTO);
            empleado.Id = id;

            dbContext.Update(empleado);
            await dbContext.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var exist = await dbContext.Empleado.AnyAsync(x => x.Id == id);
            if (!exist)
            {
                return NotFound("El Recurso no fue encontrado.");
            }

            dbContext.Remove(new Empleado()
            {
                Id = id
            });
            await dbContext.SaveChangesAsync();
            return Ok();
        }
    }
}