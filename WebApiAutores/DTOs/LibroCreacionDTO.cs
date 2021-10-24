using System.ComponentModel.DataAnnotations;
using WebApiAutores.Validations;

namespace WebApiAutores.DTOs
{
    public class LibroCreacionDTO
    {

        [Tittlecase]
        [StringLength(maximumLength: 250)]
        [Required]
        public string titulo {  get; set; }
        public DateTime? fechaPublicacion { get; set; }

        public List<int> AutoresIds {  get; set; }
    }
}
