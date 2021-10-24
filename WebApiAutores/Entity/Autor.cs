using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebApiAutores.Validations;

namespace WebApiAutores.Entity
{
    public class Autor //:IValidatableObject
    {

        public int Id {  get; set; }
        
        [Required(ErrorMessage="El campo nombre es requerido")]
        [StringLength(maximumLength:120, ErrorMessage="Campo menor a 16 caracteres")]
        [Tittlecase]
        public string Name {  get; set; }

        //referencia a libros
        public List<AutorLibro> Autoreslibros {  get; set; }
        


       // public List<Libro> Libros {  get; set; }
        //[Range(18,120)]
        //[NotMapped] //No tendra una columna en la tabla correspondiente
        //public int Edad {  get; set; }
        //[CreditCard]
        //[NotMapped] //No tendra una columna en la tabla correspondiente
        //public string TarjetaCredito {  get; set; }

        //[Url]
        //[NotMapped] //No tendra una columna en la tabla correspondiente
        //public string redSocial { get; set; }


        //public int Mayor { get; set; }
        //public int Menor { get; set; }  
        //public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        //{
        //    if (string.IsNullOrEmpty(Name)) {
        //        var primeraLetra = Name[0].ToString();

        //        if(primeraLetra != primeraLetra.ToUpper())
        //        {
        //            yield return new ValidationResult("La primera letra es mayuscula",
        //                new string[] { nameof(Name) });
        //        }
        //    }

        //    //if (Menor > Mayor)
        //    //{
        //    //    yield return new ValidationResult("Este valor no puede ser mas grande",
        //    //        new string[] { nameof(Menor) });
        //    //}

        //}
    }
}
