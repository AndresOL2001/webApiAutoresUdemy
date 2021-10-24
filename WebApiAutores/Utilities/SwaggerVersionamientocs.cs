using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace WebApiAutores.Utilities
{
    public class SwaggerVersionamientocs : IControllerModelConvention
    {
        public void Apply(ControllerModel controller)
        {
            var nameSpaceController = controller.ControllerType.Namespace;//Controllers.v1

            var versionApi = nameSpaceController.Split('.').Last().ToLower();//V1
            controller.ApiExplorer.GroupName = versionApi;
        }
    }
}
