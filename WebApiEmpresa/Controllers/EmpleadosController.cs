using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        public async Task<ActionResult<List<Empleado>>> GetAll()
        {
            return await dbContext.Empleados.ToListAsync();
        }
        [HttpGet("primero")]
        public async Task<ActionResult<Empleado>> PrimerEmpleado()
        {
            return await dbContext.Empleados.FirstOrDefaultAsync();
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Empleado>> Get(int id)
        {
            var empleado = await dbContext.Empleados.FirstOrDefaultAsync(x => x.Id == id);

            if (empleado == null)
            {
                return NotFound();
            }

            return empleado;
        }
        [HttpGet("{nombre}")]
        public async Task<ActionResult<Empleado>> GetNombre(String nombre)
        {
            var empleado = await dbContext.Empleados.FirstOrDefaultAsync(x => x.Nombre == nombre);

            if (empleado == null)
            {
                return NotFound();
            }

            return empleado;
        }
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] Empleado empleado)
        {
            
            dbContext.Add(empleado);
            await dbContext.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(Empleado empleado, int id)
        {
            var exist = await dbContext.Empleados.AnyAsync(x => x.Id == id);
            if (!exist)
            {
                return NotFound("Empleados no encontrada");
            }

            if (empleado.Id != id)
            {
                return BadRequest("Empleados sin id coincidente");
            }
            dbContext.Update(empleado);
            await dbContext.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var exist = await dbContext.Empleados.AnyAsync(x => x.Id == id);

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
    