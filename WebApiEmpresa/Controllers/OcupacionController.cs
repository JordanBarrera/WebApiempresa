using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.HttpSys;
using Microsoft.EntityFrameworkCore;
using WebApiEmpresa;
using WebApiEmpresa.DTOs;
using WebApiEmpresa.Entidades;
namespace WebApiMascota2.Controllers
{
    [ApiController]
    [Route("veterinarias/{veterinariaId:int}/ocupacion")]
    public class ServiciosController : ControllerBase
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IMapper mapper;
        private readonly UserManager<IdentityUser> userManager;

        public ServiciosController(ApplicationDbContext dbContext, IMapper mapper,
            UserManager<IdentityUser> userManager)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
            this.userManager = userManager;
        }

        [HttpGet]
        public async Task<ActionResult<List<OcupacionDTO>>> Get(int empresaId)
        {
            var existeEmpresa = await dbContext.Empresas.AnyAsync(empresaDB => empresaDB.Id == empresaId);
            if (!existeEmpresa)
            {
                return NotFound();
            }

            var ocupacion = await dbContext.Ocupacion.Where(ocupacionDB => ocupacionDB.EmpresaId == empresaId).ToListAsync();

            return mapper.Map<List<OcupacionDTO>>(ocupacion);
        }

        [HttpGet("{id:int}", Name = "obtenerOcupacion")]
        public async Task<ActionResult<OcupacionDTO>> GetById(int id)
        {
            var ocupacion = await dbContext.Ocupacion.FirstOrDefaultAsync(servicioDB => servicioDB.Id == id);

            if (ocupacion == null)
            {
                return NotFound();
            }

            return mapper.Map<OcupacionDTO>(ocupacion);
        }

        [HttpPost]
        public async Task<ActionResult> Post(int empresaId, OcupacionCreacionDTO ocupacionCreacionDTO)
        {
            var existeEmpresa = await dbContext.Ocupacion.AnyAsync(empDB => empDB.Id == empresaId);
            if (!existeEmpresa)
            {
                return NotFound();
            }

            var ocupacion = mapper.Map<Ocupacion>(ocupacionCreacionDTO);
            ocupacion.EmpresaId = empresaId;
            dbContext.Add(ocupacion);
            await dbContext.SaveChangesAsync();

            var ocupacionDTO = mapper.Map<OcupacionDTO>(ocupacion);

            return CreatedAtRoute("obtenerOcupacion", new { id = ocupacion.Id, claseId = empresaId }, ocupacionDTO);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int empresaId, int id, OcupacionCreacionDTO ocupacionCreacionDTO)
        {
            var existeEmpresa = await dbContext.Empresas.AnyAsync(empDB => empDB.Id == empresaId);
            if (!existeEmpresa)
            {
                return NotFound();
            }

            var existeOcupacion = await dbContext.Ocupacion.AnyAsync(ocupacionDB => ocupacionDB.Id == id);
            if (!existeOcupacion)
            {
                return NotFound();
            }

            var ocupacion = mapper.Map<Ocupacion>(ocupacionCreacionDTO);
            ocupacion.Id = id;
            ocupacion.EmpresaId = empresaId;

            dbContext.Update(ocupacion);
            await dbContext.SaveChangesAsync();
            return NoContent();
        }
    }
}