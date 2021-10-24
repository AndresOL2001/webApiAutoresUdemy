namespace WebApiAutores.DTOs.HATEOAS
{
    public class DatoHateoas
    {
        public string Enlace {  get; private set; }

        public string Desc {  get;private set; }

        public string Metodo {  get;private set; }

        public DatoHateoas(string enlace, string desc, string metodo)
        {
            Enlace = enlace;
            Desc = desc;
            Metodo = metodo;
        }

    }
}
