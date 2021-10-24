namespace WebApiAutores.DTOs.HATEOAS
{
    public class ColeccionDeRecursos<T>:Recurso where T:Recurso // tiene que ser un dato que herede de recurso
    {
        public List<T> Valores { get; set; }


    }
}
