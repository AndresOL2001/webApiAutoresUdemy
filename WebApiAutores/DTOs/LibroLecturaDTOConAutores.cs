namespace WebApiAutores.DTOs
{
    public class LibroLecturaDTOConAutores:LibroLecturaDTO
    {
        public List<AutorLecturaDTO> autores { get; set; }

    }
}
