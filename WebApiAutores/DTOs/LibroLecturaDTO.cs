namespace WebApiAutores.DTOs
{
    public class LibroLecturaDTO
    {
        public int Id { get; set; }

        public string titulo { get; set; }

        public DateTime? fechaPublicacion {  get; set; }

       // public List<ComentarioLectura> comentarios {  get; set; }
    }
}
