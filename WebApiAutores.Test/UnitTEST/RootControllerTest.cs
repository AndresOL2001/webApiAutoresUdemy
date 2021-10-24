using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WebApiAutores.Controllers.V1;
using WebApiAutores.Test.Mocks;

namespace WebApiAutores.Test.UnitTEST
{
    [TestClass]
    public class RootControllerTest
    {
        [TestMethod]
        public async Task SiUsuarioEsAdmin_Obtenemos4links()
        {
            //preparacion
            var authorizationService = new AuthorizationServiceMock();
            authorizationService.resultado = AuthorizationResult.Success();
            var RootController = new RouteController(authorizationService);
            RootController.Url = new UrlHelperMock();

            //ejecucion
            var resultado = await RootController.Get();
            //verificaicon
            Assert.AreEqual(4, resultado.Value.Count());
        }

        [TestMethod]
        public async Task SiUsuarioNoEsAdmin_Obtenemos2links()
        {
            //preparacion
            var authorizationService = new AuthorizationServiceMock();
            authorizationService.resultado = AuthorizationResult.Failed();
            var RootController = new RouteController(authorizationService);
            RootController.Url = new UrlHelperMock();

            //ejecucion
            var resultado = await RootController.Get();
            //verificaicon
            Assert.AreEqual(2, resultado.Value.Count());
        }

        [TestMethod]
        public async Task SiUsuarioNoEsAdmin_Obtenemos2linksMoq()
        {
            //preparacion
            var mockAuthorizationService = new Mock<IAuthorizationService>();
            mockAuthorizationService.Setup(x => x.AuthorizeAsync(
                It.IsAny<ClaimsPrincipal>(),
                It.IsAny<object>(),
                It.IsAny<IEnumerable<IAuthorizationRequirement>>()
                )).Returns(Task.FromResult(AuthorizationResult.Failed()));

            mockAuthorizationService.Setup(x => x.AuthorizeAsync(
                It.IsAny<ClaimsPrincipal>(),
                It.IsAny<object>(),
                It.IsAny<string>()
                )).Returns(Task.FromResult(AuthorizationResult.Failed()));

            var mockUrlHekper = new Mock<IUrlHelper>();
            mockUrlHekper.Setup(x => x.Link(
                It.IsAny<string>(),
                It.IsAny<object>()))
                .Returns(string.Empty);
           

            var RootController = new RouteController(mockAuthorizationService.Object);
            RootController.Url = mockUrlHekper.Object;

            //ejecucion
            var resultado = await RootController.Get();
            //verificaicon
            Assert.AreEqual(2, resultado.Value.Count());
        }
    }
}
