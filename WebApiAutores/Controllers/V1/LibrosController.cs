using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiAutores.DTOs;
using WebApiAutores.Entity;

namespace WebApiAutores.Controllers.V1
{
    [ApiController]
    [Route("api/v1/libros")]
    public class LibrosController:ControllerBase
    {
        
        private readonly AplicationDbContext context;
        private readonly IMapper mapper;

        public LibrosController(AplicationDbContext context,IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        [HttpGet("{id:int}",Name ="obtenerLibro")]
        public async Task<ActionResult<LibroLecturaDTOConAutores>> Get(int id)
        {
            var libro = await context.Libros
           .Include(libroDB => libroDB.Autoreslibros)
           .ThenInclude(autorLibroDB => autorLibroDB.autor)
           .FirstOrDefaultAsync(x => x.Id == id);

            if (libro == null)
            {
                return NotFound("No existe libro con ese id");
            }

            libro.Autoreslibros = libro.Autoreslibros.OrderBy(x => x.Orden).ToList();

            return mapper.Map<LibroLecturaDTOConAutores>(libro);
        }

        
        [HttpPost(Name ="CrearLibro")]
        public async Task<ActionResult<Libro>> Post(LibroCreacionDTO libroDTO)
        {
            if(libroDTO.AutoresIds == null)
            {
                return BadRequest("No se puede crear un libro sin autores");
            }

            //Traeme el id de los autores que existan de los que te e enviado ej 1,2,3,4000
            //resultado autoresIds = 1,2,3 ya que no existe el 4000

            var autoresIds = await context.Autores.Where(autorDB => libroDTO.AutoresIds.Contains(autorDB.Id))
                 .Select(x => x.Id).ToListAsync();

            if(libroDTO.AutoresIds.Count != autoresIds.Count)
            {
                return BadRequest("No existe uno de los autores enviados");
            }

            var libro = mapper.Map<Libro>(libroDTO);

            asignarOrdenAutores(libro);

            context.Add(libro);
            await context.SaveChangesAsync();

            var libroDTOresp = mapper.Map<LibroLecturaDTO>(libro);
           
            return CreatedAtRoute("obtenerLibro",new {id = libro.Id},libroDTOresp);

        }

        [HttpPut("{id:int}",Name ="ActualizarLibro")]
        public async Task<IActionResult> Put(int id,LibroCreacionDTO libroCreacionDTO)
        {
            //cuando tu instancias un libro entity f mantiene un registro en memoria

            var libroDB = await context.Libros
                .Include(X => X.Autoreslibros)
                .FirstOrDefaultAsync(x => x.Id == id);
            
            if(libroDB == null)
            {
                return NotFound("No existe ese libro");
            }

            libroDB = mapper.Map(libroCreacionDTO, libroDB);
            asignarOrdenAutores(libroDB);
            await context.SaveChangesAsync();
            return NoContent();
        
        }

        private void asignarOrdenAutores(Libro libro)
        {
            if (libro.Autoreslibros != null)
            {
                for (int i = 0; i < libro.Autoreslibros.Count; i++)
                {
                    libro.Autoreslibros[i].Orden = i;
                }
            }
        }

        
        [HttpPatch("{id:int}",Name ="PatchLibro")]
        public async Task<IActionResult> Patch(int id,JsonPatchDocument<LibroPatchDTO> patchDocument)
        {
            if(patchDocument == null)
            {
                return BadRequest("Formato incorrecto");
            }

            var libroDB = await context.Libros.FirstOrDefaultAsync(x => x.Id == id);
            if(libroDB == null)
            {
                return NotFound("No existe un libro con ese Id");
            }

            var libroDTO = mapper.Map<LibroPatchDTO>(libroDB);

            patchDocument.ApplyTo(libroDTO,ModelState); //Aplicamos los cambios del patchDocument

            var esValido = TryValidateModel(libroDTO);

            if (!esValido)
            {
                return BadRequest(ModelState);
            }

            mapper.Map(libroDTO, libroDB);

            await context.SaveChangesAsync();

            return NoContent();

        }

        [HttpDelete("{id:int}",Name ="BorrarLibro")]
        public async Task<ActionResult> Delete(int id)
        {
            var existe = await context.Libros.AnyAsync(x => x.Id == id);

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
