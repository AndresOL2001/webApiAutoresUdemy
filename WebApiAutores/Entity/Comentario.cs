using Microsoft.AspNetCore.Identity;

namespace WebApiAutores.Entity
{
    public class Comentario
    {
        //Si un libro no existe comentarios tampoco

        public int Id {  get; set; }

        public string Contenido {  get; set; }

        public int LibroId {  get; set; }

        public string UsuarioId { get; set; }

        public IdentityUser Usuario {  get; set; }

        public Libro Libro {  get; set; }
        public Autor autor {  get; set; }

    }
}
