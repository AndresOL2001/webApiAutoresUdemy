using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using WebApiAutores.DTOs;
using WebApiAutores.DTOs.HATEOAS;

namespace WebApiAutores.Services
{
    public class GeneradorEnlaces
    {
        private readonly IAuthorizationService authorizationService;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IActionContextAccessor actionContextAccessor;

        public GeneradorEnlaces(IAuthorizationService authorizationService,IHttpContextAccessor httpContextAccessor
            ,IActionContextAccessor actionContextAccessor)
        {
            this.authorizationService = authorizationService;
            this.httpContextAccessor = httpContextAccessor;
            this.actionContextAccessor = actionContextAccessor;
        }

        private IUrlHelper ConstruirURLHelper()
        {
            var factoria = httpContextAccessor.HttpContext.RequestServices.GetRequiredService<IUrlHelperFactory>();

            return factoria.GetUrlHelper(actionContextAccessor.ActionContext);

        }

        private async Task<bool> EsAdmin()
        {
            var httpContext = httpContextAccessor.HttpContext;
            var resultado = await authorizationService.AuthorizeAsync(httpContext.User, "esAdmin");
            return resultado.Succeeded;
        }

        public async Task GeneranEnlaces(AutorLecturaDTO autorDTO)
        {
            var esAdmin = await EsAdmin();
            var Url = ConstruirURLHelper();

            autorDTO.Enlaces.Add(new DatoHateoas(
                enlace: Url.Link("ObtenerAutor", new { id = autorDTO.Id })
                , desc: "obtenerAutor",
                metodo: "GET"));

           

            if (esAdmin)
            {

                autorDTO.Enlaces.Add(new DatoHateoas(
                    enlace: Url.Link("ActualizarAutor", new { id = autorDTO.Id })
                    , desc: "autor-actualizar",
                    metodo: "PUT"));

                autorDTO.Enlaces.Add(new DatoHateoas(
                    enlace: Url.Link("BorrarAutor", new { id = autorDTO.Id })
                    , desc: "autor-borrar",
                    metodo: "DELETE"));

            }

        }

        public async Task GeneranEnlacesGenerico(AutorLecturaDTO autorDTO)
        {
            var esAdmin = await EsAdmin();
            var Url = ConstruirURLHelper();

            autorDTO.Enlaces.Add(new DatoHateoas(
                    enlace: Url.Link("ObtenerAutores", new { }),
                    desc: "obtenerAutores",
                    metodo: "GET"));

            if (esAdmin)
            {


                autorDTO.Enlaces.Add(new DatoHateoas(
                       enlace: Url.Link("CrearAutor", new { }),
                       desc: "crear-autor",
                       metodo: "POST"));
            }

        }
    }
}
