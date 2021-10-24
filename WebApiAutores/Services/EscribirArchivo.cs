namespace WebApiAutores.Services
{
    public class EscribirArchivo : IHostedService
    {
        private readonly IWebHostEnvironment env;
        private readonly string nombreArchivo="Archivo 1.txt";
        private Timer timer;
        public EscribirArchivo(IWebHostEnvironment env)
        {
            this.env = env;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            //Cuando cargue api
            timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromSeconds(5));
            Escribir("Proceso Iniciado");
            return Task.CompletedTask;

        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            timer.Dispose();
            Escribir("Proceso terminado");
            return Task.CompletedTask;
        }

        public void Escribir(string mensaje)
        {
            var ruta = $@"{env.ContentRootPath}\wwwroot\{nombreArchivo}";
            using(StreamWriter sw = new StreamWriter(ruta, append: true))
            {
                sw.WriteLine(mensaje);
            }
        }

        private void DoWork(object state)
        {
            Escribir("Proceso en ejecución: " + DateTime.Now.ToString("dd/MM/yyyy"));
        }
    }
}
