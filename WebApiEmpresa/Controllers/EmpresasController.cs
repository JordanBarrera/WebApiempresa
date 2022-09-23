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

        public EmpresasController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        public async Task<ActionResult<List<Empresa>>> GetAll(){
            return await dbContext.Empresa.ToListAsync();
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
            var exist = await dbContext.Empresa.AnyAsync(x => x.Id == id);
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
            var exist = await dbContext.Empresa.AnyAsync(x => x.Id == id);
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
