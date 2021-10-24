using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.ComponentModel.DataAnnotations;
using WebApiAutores.Validations;

namespace WebApiAutores.Test
{
    [TestClass]
    public class TittlecaseTest
    {
        [TestMethod]
        public void PrimeraLetraMinisculaDevuelveError()
        {
            //Preparacion
            var tittlecase = new Tittlecase();
            var valor = "felipe";
            var context = new ValidationContext(new {Nombre = valor});

            //Ejecucion
            var resultado = tittlecase.GetValidationResult(valor, context);

            //Verificacion
            Assert.AreEqual("La primera letra debe ser mayuscula",resultado.ErrorMessage);
        }
        [TestMethod]
        public void ValorNuloNoDevuelveError()
        {
            //Preparacion
            var tittlecase = new Tittlecase();
            string valor = null;
            var context = new ValidationContext(new { Nombre = valor });

            //Ejecucion
            var resultado = tittlecase.GetValidationResult(valor, context);

            //Verificacion
            Assert.IsNull(resultado);
        }

        [TestMethod]
        public void ValorConPrimeraLetra_NoDevuelveError()
        {
            //Preparacion
            var tittlecase = new Tittlecase();
            string valor = "Felipe";
            var context = new ValidationContext(new { Nombre = valor });

            //Ejecucion
            var resultado = tittlecase.GetValidationResult(valor, context);

            //Verificacion
            Assert.IsNull(resultado);
        }
    }
}