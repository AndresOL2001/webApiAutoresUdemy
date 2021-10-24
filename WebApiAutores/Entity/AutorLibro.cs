namespace WebApiAutores.Entity
{
    public class AutorLibro
    {
        public int LibroId {  get; set; }
        public int AutorId {  get; set; }
        public int Orden { get; set; }

        //Propiedades de busqueda
        public Libro libro { get; set; }

        public Autor autor {  get; set; }


    }
}
