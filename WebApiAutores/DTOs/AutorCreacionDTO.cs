using System.ComponentModel.DataAnnotations;
using WebApiAutores.Validations;

namespace WebApiAutores.DTOs
{
    public class AutorCreacionDTO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El campo nombre es requerido")]
        [StringLength(maximumLength: 120, ErrorMessage = "Campo menor a 16 caracteres")]
        [Tittlecase]
        public string Name { get; set; }


    }
}
