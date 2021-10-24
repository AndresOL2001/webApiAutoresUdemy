using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApiAutores.DTOs;

namespace WebApiAutores.Controllers.V1
{
    [ApiController]
    [Route("api/v1/auth")]
    public class AuthController:ControllerBase
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly IConfiguration configuration;
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly IDataProtector dataprotector;

        public AuthController(UserManager<IdentityUser>userManager,
            IConfiguration configuration,SignInManager<IdentityUser> signInManager
            ,IDataProtectionProvider data)
        {
            this.userManager = userManager;
            this.configuration = configuration;
            this.signInManager = signInManager;
            dataprotector= data.CreateProtector("KAJSFDLKSADFKJLASKJLFKAFJLSLKASFKLAJKFDSKLJLKAFSJKJAFG");
        }

        //[HttpGet("encriptar")]
        //public ActionResult Encriptar()
        //{
        //    var textoPlano = "Felipe Gabilan";
        //    var textocifrado = dataprotector.Protect(textoPlano);
        //    var textoDescifrado = dataprotector.Unprotect(textocifrado);

        //    return Ok(new
        //    {
        //        textoPlano = textoPlano,
        //        textocifrado = textocifrado,
        //        textoDescifrado = textoDescifrado
        //    });
        //}

        //[HttpGet("encriptarPorTiempo")]
        //public ActionResult EncriptarPorTiempo()
        //{
        //    var protectorxtiempo = dataprotector.ToTimeLimitedDataProtector();
        //    var textoPlano = "Felipe Gabilan";
        //    var textocifrado = protectorxtiempo.Protect(textoPlano,lifetime:TimeSpan.FromSeconds(100));
        //    var textoDescifrado = protectorxtiempo.Unprotect(textocifrado);

        //    return Ok(new
        //    {
        //        textoPlano = textoPlano,
        //        textocifrado = textocifrado,
        //        textoDescifrado = textoDescifrado
        //    });
        //}

        [HttpPost("registrar",Name ="RegistrarUsuario")]
        public async Task<ActionResult<RespuestaAutenticacion>> registrar(CredencialesUsuario credencialUsuario)
        {
            var usuario = new IdentityUser { UserName = credencialUsuario.email, Email = credencialUsuario.email };

            var resultado = await userManager.CreateAsync(usuario, credencialUsuario.password);

            if (resultado.Succeeded)
            {
                return await ConstruirToken(credencialUsuario);
            }
            else
            {
                return BadRequest(resultado.Errors);
            }
        }

        [HttpPost("login",Name ="LoginUsuario")]
        public async Task<ActionResult<RespuestaAutenticacion>> login(CredencialesUsuario credencialusuario)
        {
            var resultado = await signInManager.PasswordSignInAsync(credencialusuario.email,credencialusuario.password,isPersistent:false,lockoutOnFailure:false);

            if (resultado.Succeeded)
            {
                return await ConstruirToken(credencialusuario);
            }
            else
            {
                return BadRequest("Login Incorrecto");
            }

        }

        [HttpGet("RenovarToken",Name ="RenovarToken")]
        [Authorize(AuthenticationSchemes=JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<RespuestaAutenticacion>> Renovar()
        {
            var emailClaim = HttpContext.User.Claims.Where(claim => claim.Type == "email").FirstOrDefault();

            var email = emailClaim.Value;

            var credencialesUsuario = new CredencialesUsuario()
            {
                email = email,

            };

            return await ConstruirToken(credencialesUsuario);
        }

        private async Task<RespuestaAutenticacion> ConstruirToken(CredencialesUsuario credencialesUsuario)
        {
            var claims = new List<Claim>()
            {
                new Claim("email",credencialesUsuario.email),
                new Claim("lo que yo quiera","cualquier otro valores")
            };

            var usuario = await userManager.FindByEmailAsync(credencialesUsuario.email);

            var claimsDB = await userManager.GetClaimsAsync(usuario);

            claims.AddRange(claimsDB);

            var llave = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["llavejwt"]));

            var creds = new SigningCredentials(llave, SecurityAlgorithms.HmacSha256);

            var expiracion = DateTime.UtcNow.AddMinutes(30);

            var securityToken = new JwtSecurityToken(issuer:null,audience:null,claims:claims,expires:expiracion,
                signingCredentials: creds);

            return new RespuestaAutenticacion()
            {
                token = new JwtSecurityTokenHandler().WriteToken(securityToken),
                expiracion = expiracion,

            };
        }

        [HttpPost("hacerAdmin",Name ="HacerAdmin")]
        public async Task<ActionResult> HacerAdmin(EditarAdminDTO editarAdminDTO)
        {
            var usuario = await userManager.FindByEmailAsync(editarAdminDTO.Email);

            await userManager.AddClaimAsync(usuario, new Claim("esAdmin","1"));

            return NoContent();

        }

        [HttpPost("removerAdmin",Name ="RemoverAdmin")]
        public async Task<ActionResult> RemoverAdmin(EditarAdminDTO editarAdminDTO)
        {
            var usuario = await userManager.FindByEmailAsync(editarAdminDTO.Email);

            await userManager.RemoveClaimAsync(usuario, new Claim("esAdmin", "1"));

            return NoContent();

        }

    }
}
