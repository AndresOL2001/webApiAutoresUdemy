using System.ComponentModel.DataAnnotations;
using WebApiAutores.Validations;

namespace WebApiAutores.Entity
{
    public class Libro
    {
        public int Id { get; set; }

        [Tittlecase]
        [StringLength(maximumLength:250)]
        [Required]
        public string titulo { get; set; }
        public DateTime? fechaPublicacion { get; set; }
        public List<Comentario> comentarios { get; set; }

        //referencia a autores
        public List<AutorLibro> Autoreslibros {  get; set; }

        // public int AutorId {  get; set; }

        //  public Autor Autor {  get; set; }

    }
}
