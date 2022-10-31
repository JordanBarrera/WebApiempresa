using Microsoft.AspNetCore.Mvc;
using WebApiEmpresa.Entidades;
using Microsoft.EntityFrameworkCore;
using WebApiEmpresa.Service;
using WebApiEmpresa.Filtros;


namespace WebApiEmpresa.Controllers
{
    [ApiController]
    [Route("api/empresas")]
    public class EmpresasController : ControllerBase
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IService service;
        private readonly ServiceTransient serviceTransient;
        private readonly ServiceScoped serviceScoped;
        private readonly ServiceSingleton serviceSingleton;
        private readonly ILogger<EmpresasController> logger;
        private readonly IWebHostEnvironment env;
        private readonly string nuevosRegistros = "nuevosRegistros.txt";
        private readonly string registrosConsultados = "registrosConsultados.txt";


        public EmpresasController(ApplicationDbContext context, IService service,
            ServiceTransient serviceTransient, ServiceScoped serviceScoped,
            ServiceSingleton serviceSingleton, ILogger<EmpresasController> logger,
            IWebHostEnvironment env)
        {
            this.dbContext = context;
            this.service = service;
            this.serviceTransient = serviceTransient;
            this.serviceScoped = serviceScoped;
            this.serviceSingleton = serviceSingleton;
            this.logger = logger;
            this.env = env;
        }


        [HttpGet("GUID")]
        [ResponseCache(Duration = 10)]
        [ServiceFilter(typeof(FiltroDeAccion))]
        public ActionResult ObtenerGuid()
        {
            
            logger.LogInformation("Durante la ejecucion");
            return Ok(new
            {
                EmpresasControllerTransient = serviceTransient.guid,
                ServiceA_Transient = service.GetTransient(),
                EmpresasControllerScoped = serviceScoped.guid,
                ServiceA_Scoped = service.GetScoped(),
                EmpresasControllerSingleton = serviceSingleton.guid,
                ServiceA_Singleton = service.GetSingleton()
            });
        }


        [HttpGet]
        [HttpGet("listado")]
        [HttpGet("/listado")]
        public async Task<ActionResult<List<Empresa>>> GetAll(){
            logger.LogInformation("Se obtiene el listado de dueños");
            logger.LogWarning("Mensaje de prueba warning");
            service.EjecutarJob();
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
                logger.LogError("No se encuentra el deuño. ");
                return NotFound();
            }
            var ruta = $@"{env.ContentRootPath}\wwwroot\{registrosConsultados}";
            using (StreamWriter writer = new StreamWriter(ruta, append: true)) { writer.WriteLine(empresa.Id + " " + empresa.Nombre); }

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
