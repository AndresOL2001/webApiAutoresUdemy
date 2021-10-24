using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using WebApiAutores.DTOs;
using WebApiAutores.Services;

namespace WebApiAutores.Utilities
{
    public class HATEOASAutorFiltroAttribute:HATEOASFiltroAttribute
    {
        private readonly GeneradorEnlaces generadorEnlaces;

        public HATEOASAutorFiltroAttribute(GeneradorEnlaces generadorEnlaces)
        {
            this.generadorEnlaces = generadorEnlaces;
        }

        public override async Task OnResultExecutionAsync(ResultExecutingContext context,ResultExecutionDelegate next)
        {


            var debeIncluir = DebeIncluirHATEOAS(context);

            if (!debeIncluir)
            {
                await next();
                return;
            }
            var resultado = context.Result as ObjectResult;
            var autorDTO = resultado.Value as AutorLecturaDTO;
            if(autorDTO == null)
            {
                var autoresDTO = resultado.Value as List<AutorLecturaDTO> ?? throw new ArgumentException("Se esperaba una instancia" +
                    "de autorDTO o List<AutorDTO>");
                autoresDTO.ForEach(async autor => await generadorEnlaces.GeneranEnlacesGenerico(autor));
                resultado.Value = autoresDTO;
            }
            else
            {

            await generadorEnlaces.GeneranEnlaces(autorDTO);
            }
            await next();
        }
    }
}
