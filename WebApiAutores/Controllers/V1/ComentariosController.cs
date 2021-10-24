using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiAutores.DTOs;
using WebApiAutores.Entity;

namespace WebApiAutores.Controllers.V1
{
    [Route("api/v1/libros/{libroId:int}/comentarios")]
    [ApiController]
    public class ComentariosController : ControllerBase
    {
        private readonly AplicationDbContext context;
        private readonly IMapper mapper;
        private readonly UserManager<IdentityUser> userManager;

        public ComentariosController(AplicationDbContext context,IMapper mapper,UserManager<IdentityUser> userManager)
        {
            this.context = context;
            this.mapper = mapper;
            this.userManager = userManager;
        }


        [HttpGet(Name ="ObtenerComentariosLibro")]
        public async Task<ActionResult<List<ComentarioLectura>>> Get(int libroId)
        {
            var existeLibro = await context.Libros.AnyAsync(libroDB => libroDB.Id == libroId);

            if (!existeLibro)
            {
                return NotFound();
            }

            var comentarios = await context.Comentarios.Where(comentarioDb => comentarioDb.Id == libroId).ToListAsync();

            if(comentarios == null)
            {
                return NotFound("No existen comentarios con ese id del libro");
            }

            return mapper.Map<List<ComentarioLectura>>(comentarios);

        }

        [HttpGet("{id:int}",Name = "ObtenerComentario")]
        public async Task<ActionResult<ComentarioLectura>> GetById(int id)
        {
            var comentario = await context.Comentarios.FirstOrDefaultAsync(comentarioDB => comentarioDB.Id == id);

            if(comentario == null)
            {
                return NotFound("No existe comentario con ese id");
            }
         
            return mapper.Map<ComentarioLectura>(comentario);

        }

        [HttpPost(Name ="CrearComentario")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> Post(int libroId,ComentarioCreacion comentarioDTO)
        {
            
            var emailClaim = HttpContext.User.Claims.Where(claim => claim.Type == "email").FirstOrDefault();

            var email = emailClaim.Value;

            var usuario = await userManager.FindByEmailAsync(email);

            var usuarioId = usuario.Id;
            
            var existeLibro = await context.Libros.AnyAsync(libroDB => libroDB.Id == libroId);

            if (!existeLibro)
            {
                return NotFound();
            }

            var comentario = mapper.Map<Comentario>(comentarioDTO);
            comentario.LibroId = libroId;
            comentario.UsuarioId = usuarioId;


            context.Add(comentario);
            await context.SaveChangesAsync();

            var comentarioDTOresp = mapper.Map<ComentarioLectura>(comentario);

            return CreatedAtRoute("ObtenerComentario", new {id = comentario.Id,libroId = libroId} ,comentarioDTOresp);
        }

        [HttpPut("{id:int}",Name ="ActualizarComentario")]
        public async Task<ActionResult> Put(int libroId,int id,ComentarioCreacion comentarioDTO)
        {
            var existeLibro = await context.Libros.AnyAsync(libroDB => libroDB.Id == libroId);

            if (!existeLibro)
            {
                return NotFound();
            }

            var existeComentario = await context.Comentarios.AnyAsync(comentarioDB => comentarioDB.Id == id);

            if (!existeComentario)
            {
                return NotFound();
            }

            var comentario = mapper.Map<Comentario>(comentarioDTO);
            comentario.Id = id;
            comentario.LibroId = libroId;
            context.Update(comentario);
            await context.SaveChangesAsync();
            return NoContent();

        }
            

    }
}
