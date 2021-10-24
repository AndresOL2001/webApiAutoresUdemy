using System.ComponentModel.DataAnnotations;
using WebApiAutores.DTOs.HATEOAS;
using WebApiAutores.Validations;

namespace WebApiAutores.DTOs
{
    public class AutorLecturaDTO:Recurso
    {
        public int Id {  get; set; }
     
        public string Name {  get; set; }

    }
}
