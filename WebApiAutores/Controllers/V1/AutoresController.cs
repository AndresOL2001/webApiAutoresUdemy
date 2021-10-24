using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiAutores.DTOs;
using WebApiAutores.DTOs.PAGINATION;
using WebApiAutores.Entity;
using WebApiAutores.Utilities;

namespace WebApiAutores.Controllers.V1
{

    // [Authorize]
    [ApiController]
    [Route("api/v1/autores")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "EsAdmin")]
    public class AutoresController : ControllerBase
    {

        //private readonly IServicio servicio;
        //private readonly servicioTransient transient;
        //private readonly servicioSingleton singleton;
        //private readonly servicioScoped scoped;
        //private readonly ILogger<AutoresController> logger;

        private readonly AplicationDbContext context;
        private readonly IMapper mapper;
        private readonly IConfiguration configuration;
        private readonly IAuthorizationService authorizationService;

        public AutoresController(AplicationDbContext context, IMapper mapper, IConfiguration configuration,
            IAuthorizationService authorizationService)
        //IServicio servicio,servicioTransient transient,
        //servicioSingleton singleton,servicioScoped scoped,ILogger<AutoresController> logger
        {

            this.context = context;
            this.mapper = mapper;
            this.configuration = configuration;
            this.authorizationService = authorizationService;
            //this.servicio = servicio;
            //this.transient = transient;
            //this.singleton = singleton;
            //this.scoped = scoped;
            //this.logger = logger;
        }

        //[ResponseCache(Duration =10)] //Esto les va a responder durante los proximos 10 segundos
        //[HttpGet("GUID")]
        //[ServiceFilter(typeof(FiltroDeAccion))]
        //public ActionResult ObtenerGuids()
        //{
        //    return Ok(new
        //    {
        //        AutoresController_Transient = transient.guid,
        //        ServicioA_Transient = servicio.ObtenerTransient(),
        //        AutoresController_Singleton = singleton.guid,
        //        ServicioA_Singleton = servicio.ObtenerSingleton(),
        //        AutoresController_Scoped = scoped.guid,
        //        ServicioA_Scoped = servicio.ObtenerScoped()

        //    }) ;


        //}


        //[HttpGet("primero")]
        //public async Task<ActionResult<Autor>> PrimerAutor([FromHeader] int miValor,[FromQuery] string nombre)
        //{
        //    return await context.Autores.FirstOrDefaultAsync();
        //}

        [HttpGet("{nombre}", Name = "ObtenerAutorPorNombre")]
        public async Task<ActionResult<List<AutorLecturaDTO>>> GetByName([FromRoute] string nombre)
        {
            var autores = await context.Autores.Where(autorBD => autorBD.Name.Contains(nombre)).
                ToListAsync();

            if (autores == null)
            {
                return NotFound();
            }

            return mapper.Map<List<AutorLecturaDTO>>(autores);

        }

        [HttpGet("{id:int}", Name = "ObtenerAutor")]
        [AllowAnonymous]
        [ServiceFilter(typeof(HATEOASAutorFiltroAttribute))]
        public async Task<ActionResult<AutorDTOConLibros>> Get([FromRoute] int id,[FromHeader] string incluirHATEOAS)
        {
            var autor = await context.Autores
                .Include(autorDB => autorDB.Autoreslibros)
                .ThenInclude(autorlibro => autorlibro.libro)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (autor == null)
            {
                return NotFound();
            }

            var dto = mapper.Map<AutorDTOConLibros>(autor);

            return Ok(dto); 

        }

       // [HttpGet("listado")] api/autores/listado
      //  [HttpGet("/listado")] listado
      //  [Authorize(AuthenticationSchemes =JwtBearerDefaults.AuthenticationScheme)]
      [HttpGet(Name ="ObtenerAutores")]//api/autores
      [AllowAnonymous]
      [ServiceFilter(typeof(HATEOASAutorFiltroAttribute))]
        public async Task<ActionResult<List<AutorLecturaDTO>>> Get([FromHeader] string incluirHATEOAS ,
          [FromQuery] PaginationDTO paginationDTO)
        {
            //throw new NotImplementedException();

            //logger.LogInformation("Estamos obteniendo los autores");
            //logger.LogWarning("Estamos obteniendo los autores");

            //servicio.RealizarTarea();
            var queryable = context.Autores.AsQueryable();
            await HttpContext.InsertarParametrosPaginacionEnCabecera(queryable);
            var autores = await queryable.OrderBy(autor => autor.Name).Paginar(paginationDTO).ToListAsync();
                        
            return mapper.Map<List<AutorLecturaDTO>>(autores);
     
        }

        [HttpPost(Name ="CrearAutor")]
        public async Task<ActionResult> Post([FromBody]AutorCreacionDTO autorcreacion)
        {
            var existeAutor = await context.Autores.AnyAsync(x=> x.Name == autorcreacion.Name);

            if(existeAutor)
            {
                return BadRequest($"Ya existe un autor con ese nombre:{autorcreacion.Name}");
            }

            var autor = mapper.Map<Autor>(autorcreacion);


            context.Add(autor);
            await context.SaveChangesAsync();

            var autorDTO = mapper.Map<AutorLecturaDTO>(autor);

            return CreatedAtRoute("ObtenerAutor",new {id = autor.Id},autorDTO);
        }

        [HttpPut("{id:int}",Name ="ActualizarAutor")]
        public async Task<ActionResult> Put(AutorCreacionDTO autorDTO,int id)
        {

            var existe = await context.Autores.AnyAsync(x => x.Id == id);

            if (!existe)
            {
                return NotFound();
            }

            var autor = mapper.Map<Autor>(autorDTO);
            autor.Id = id;

            context.Update(autor);
            await context.SaveChangesAsync();
            return NoContent();

        }

        [HttpDelete("{id:int}",Name ="BorrarAutor")]
        public async Task<ActionResult> Delete(int id)
        {
            var existe = await context.Autores.AnyAsync(x => x.Id==id);

            if (!existe)
            {
                return NotFound();
            }

            context.Remove(new Autor() { Id = id });
            await context.SaveChangesAsync();
            return NoContent();
        }

    }
}
