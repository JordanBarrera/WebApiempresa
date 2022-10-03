using Microsoft.AspNetCore.Mvc;
using WebApiEmpresa.Entidades;
using Microsoft.EntityFrameworkCore;


namespace WebApiEmpresa.Controllers
{
    [ApiController]
    [Route("api/empresas")]
    public class EmpresasController : ControllerBase
    {
        private readonly ApplicationDbContext dbContext;

        public EmpresasController(ApplicationDbContext context)
        {
            this.dbContext = context;
        }

        [HttpGet]
        [HttpGet("listado")]
        [HttpGet("/listado")]
        public async Task<ActionResult<List<Empresa>>> GetAll(){
            return await dbContext.Empresas.Include(x => x.Empleados).ToListAsync();
        }
        [HttpGet("primero")]
        public async Task<ActionResult<Empresa>> PrimerEmpresa()
        {
            return await dbContext.Empresas.Include(x => x.Empleados).FirstOrDefaultAsync();
        }
        [HttpGet("primero2")]
        public ActionResult<Empresa> PrimerEmpresaD()
        {
            return new Empresa() { Nombre = "FORD" };
        }

        [HttpGet("{id:int}/{param}")]
        public async Task<ActionResult<Empresa>> Get(int id, string param)
        {
            var empresa = await dbContext.Empresas.Include(x => x.Empleados).FirstOrDefaultAsync(x => x.Id == id);

            if (empresa == null)
            {
                return NotFound();
            }

            return empresa;
        }
        [HttpGet("{nombre}")]
        public async Task<ActionResult<Empresa>> GetNombre(String nombre)
        {
            var empresa = await dbContext.Empresas.FirstOrDefaultAsync(x => x.Nombre == nombre);

            if (empresa == null)
            {
                return NotFound();
            }

            return empresa;
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] Empresa empresa)
        {
            dbContext.Add(empresa);
            await dbContext.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(Empresa empresa, int id)
        {
            var exist = await dbContext.Empresas.AnyAsync(x => x.Id == id);
            if (!exist)
            {
                return NotFound();
            }

            if (empresa.Id != id)
            {
                return BadRequest("El id no coincide");
            }

            dbContext.Update(empresa);
            await dbContext.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var exist = await dbContext.Empresas.AnyAsync(x => x.Id == id);
            if (!exist)
            {
                return NotFound("Registro no encontrado");
            }

            dbContext.Remove(new Empresa() { Id = id });
            await dbContext.SaveChangesAsync();
            return Ok();
        }
    }
   
}
