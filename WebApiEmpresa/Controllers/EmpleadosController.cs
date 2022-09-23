using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Scaffolding.Metadata;
using WebApiEmpresa.Entidades;



namespace WebApiEmpresa.Controllers
{
    [ApiController]
    [Route("api/empleados")]
    public class EmpleadosController : ControllerBase
    {
        private readonly ApplicationDbContext dbContext;
        public EmpleadosController(ApplicationDbContext context)
        {
            dbContext = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<Empleado>>> Get()
        {
            return await dbContext.Empleado.Include(x => x.Nombre).ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] Empleado empleados)
        {
            var exist = await dbContext.Empleado.AnyAsync(x => x.Id == empleados.Id);

            if (!exist)
            {
                return BadRequest($"No existe pais relacionado al id");
            }

            dbContext.Add(empleados);
            await dbContext.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(Empleado empleados, int id)
        {
            var exist = await dbContext.Empleado.AnyAsync(x => x.Id == id);
            if (!exist)
            {
                return NotFound("Empleados no encontrada");
            }

            if (empleados.Id != id)
            {
                return BadRequest("Empleados sin id coincidente");
            }
            dbContext.Update(empleados);
            await dbContext.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var exist = await dbContext.Empleado.AnyAsync(x => x.Id == id);

            if (!exist)
            {
                return NotFound("Registro no encontrado");
            }

            dbContext.Remove(new Empleado() { Id = id });

            await dbContext.SaveChangesAsync();
            return Ok();
        }
    }
}
    