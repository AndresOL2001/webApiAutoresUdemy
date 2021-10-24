namespace WebApiAutores.Middlewares
{
    public static class LoguearRespuestaExtensions
    {
        public static IApplicationBuilder UseLoguearRespuesta(this IApplicationBuilder app)
        {
            return app.UseMiddleware<LoguearRespuesta>();
        }
    }


    public class LoguearRespuesta
    {
        private readonly RequestDelegate next;
        private readonly ILogger<LoguearRespuesta> logger;

        public LoguearRespuesta(RequestDelegate next,ILogger<LoguearRespuesta> logger)
        {
            this.next = next;
            this.logger = logger;
        }

        //Invoke o InvokeAsync
        public async Task InvokeAsync(HttpContext contexto)
        {
            
                using (var ms = new MemoryStream())
                {
                    var cuerpoOriginal = contexto.Response.Body;

                    contexto.Response.Body = ms;

                    await next(contexto);

                    ms.Seek(0, SeekOrigin.Begin);
                    string respuesta = new StreamReader(ms).ReadToEnd();

                    ms.Seek(0, SeekOrigin.Begin);

                    await ms.CopyToAsync(cuerpoOriginal);
                    contexto.Response.Body = cuerpoOriginal;

                    logger.LogInformation(respuesta);
                }
            
        }
    }
}
