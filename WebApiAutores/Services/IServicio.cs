namespace WebApiAutores.Services
{
    public interface IServicio
    {
        Guid ObtenerScoped();
        Guid ObtenerSingleton();
        Guid ObtenerTransient();
        void RealizarTarea();
    }

    public class servicioA : IServicio
    {
        private readonly ILogger<servicioA> logger;
        private readonly servicioTransient transient;
        private readonly servicioSingleton singleton;
        private readonly servicioScoped scoped;

        public servicioA(ILogger<servicioA> logger,servicioTransient transient,servicioSingleton singleton,
            servicioScoped scoped)
        {
            this.logger = logger;
            this.transient = transient;
            this.singleton = singleton;
            this.scoped = scoped;
        }
        public Guid ObtenerTransient() { return transient.guid; }
        public Guid ObtenerSingleton() {  return singleton.guid; }
        public Guid ObtenerScoped() { return scoped.guid; }
        public void RealizarTarea()
        {
            
        }
    }

    public class servicioB : IServicio
    {
        public Guid ObtenerScoped()
        {
            throw new NotImplementedException();
        }

        public Guid ObtenerSingleton()
        {
            throw new NotImplementedException();
        }

        public Guid ObtenerTransient()
        {
            throw new NotImplementedException();
        }

        public void RealizarTarea()
        {

        }
    }

    public class servicioTransient
    {
        public Guid guid = Guid.NewGuid();
    }
    public class servicioScoped
    {
        public Guid guid = Guid.NewGuid();
    }
    public class servicioSingleton
    {
        public Guid guid = Guid.NewGuid();
    }

}
