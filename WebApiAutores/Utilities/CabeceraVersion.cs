using Microsoft.AspNetCore.Mvc.ActionConstraints;

namespace WebApiAutores.Utilities
{
    public class CabeceraVersion : Attribute, IActionConstraint
    {
        private readonly string cabecera;
        private readonly string valor;

        public CabeceraVersion(string cabecera,string valor)
        {
            this.cabecera = cabecera;
            this.valor = valor;
        }

        public int Order => throw new NotImplementedException();

        public bool Accept(ActionConstraintContext context)
        {
            var cabeceras = context.RouteContext.HttpContext.Request.Headers;

            if (!cabeceras.ContainsKey(cabecera))
            {
                return false;
            }
            return string.Equals(cabeceras[cabecera], valor,StringComparison.OrdinalIgnoreCase);
        }
    }
}
