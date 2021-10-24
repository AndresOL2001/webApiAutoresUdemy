using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApiAutores.DTOs.HATEOAS;

namespace WebApiAutores.Controllers.V1
{
    [ApiController]
    [Route("api/v1")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class RouteController:ControllerBase
    {
        private readonly IAuthorizationService authorizationService;

        public RouteController(IAuthorizationService authorizationService)
        {
            this.authorizationService = authorizationService;
        }

        [HttpGet(Name = "ObtenerRoot")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<DatoHateoas>>> Get()
        {
            var datosHateoas = new List<DatoHateoas>();

            var esAdmin = await authorizationService.AuthorizeAsync(User, "esAdmin");

            datosHateoas.Add(new DatoHateoas(enlace:Url.Link("ObtenerRoot", new {}),desc:"self",metodo:"GET"));

            datosHateoas.Add(new DatoHateoas(enlace: Url.Link("ObtenerAutores", new { }), desc: "autores", metodo: "GET"));

            if (esAdmin.Succeeded)
            {
            datosHateoas.Add(new DatoHateoas(enlace: Url.Link("CrearAutor", new { }), desc: "autor-crear", metodo: "POST"));

            datosHateoas.Add(new DatoHateoas(enlace: Url.Link("CrearLibro", new { }), desc: "libro-crear", metodo: "POST"));
            }


            return datosHateoas;

        }
    }
}
