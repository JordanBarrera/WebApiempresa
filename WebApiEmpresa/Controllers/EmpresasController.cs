using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiEmpresa;
using WebApiEmpresa.DTOs;
using WebApiEmpresa.Entidades;

namespace WebApiEmpresa.Controllers
{
    [ApiController]
    [Route("empresas")]
    public class EmpresasController : ControllerBase
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IMapper mapper;

        public EmpresasController(ApplicationDbContext context, IMapper mapper)
        {
            this.dbContext = context;
            this.mapper = mapper;
        }

        [HttpGet]
        [HttpGet("/listadoEmpresa")]
        public async Task<ActionResult<List<Empresas>>> GetAll()
        {
            return await dbContext.Empresas.ToListAsync();
        }

        [HttpGet("{id:int}", Name = "obtenerEmpresa")]
        public async Task<ActionResult<EmpresaDTOConEmpleados>> GetById(int id)
        {
            var empresa = await dbContext.Empresas
                .Include(empleadoDB => empleadoDB.EmpleadoEmpresas)
                .ThenInclude(empleadoEmpresaDB => empleadoEmpresaDB.Empleado)
                .Include(ocupacionDB => ocupacionDB.Ocupacion)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (empresa == null)
            {
                return NotFound();
            }


            return mapper.Map<EmpresaDTOConEmpleados>(empresa);
        }

        [HttpPost]
        public async Task<ActionResult> Post(EmpresaCreacionDTO empresaCreacionDTO)
        {

            if (empresaCreacionDTO.EmpleadosIds == null)
            {
                return BadRequest("No se puede crear una empresa sin empleados.");
            }

            var empleadosIds = await dbContext.Empleado
                .Where(empleadoBD => empresaCreacionDTO.EmpleadosIds.Contains(empleadoBD.Id)).Select(x => x.Id).ToListAsync();

            if (empresaCreacionDTO.EmpleadosIds.Count != empleadosIds.Count)
            {
                return BadRequest("No existe uno de los empleados enviados");
            }

            var empresa = mapper.Map<Empresas>(empresaCreacionDTO);


            dbContext.Add(empresa);
            await dbContext.SaveChangesAsync();

            var empresaDTO = mapper.Map<EmpresaDTO>(empresa);

            return CreatedAtRoute("obtenerEmpresa", new { id = empresa.Id }, empresaDTO);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int id, EmpresaCreacionDTO empresaCreacionDTO)
        {
            var empresaDB = await dbContext.Empresas
                .Include(x => x.EmpleadoEmpresas)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (empresaDB == null)
            {
                return NotFound();
            }

            empresaDB = mapper.Map(empresaCreacionDTO, empresaDB);


            await dbContext.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var exist = await dbContext.Empresas.AnyAsync(x => x.Id == id);
            if (!exist)
            {
                return NotFound("El Recurso no fue encontrado.");
            }

            dbContext.Remove(new Empresas { Id = id });
            await dbContext.SaveChangesAsync();
            return Ok();
        }

    }
}