using AutoMapper;
using WebApiAutores.DTOs;
using WebApiAutores.Entity;

namespace WebApiAutores.Utilities
{
    public class AutoMapper : Profile
    {
        public AutoMapper()
        {
            //Autores
            CreateMap<AutorCreacionDTO, Autor>();

            CreateMap<Autor, AutorLecturaDTO>();

            //Mapeamos para obtener los autores
            CreateMap<Autor, AutorDTOConLibros>().ForMember(autorDTO => autorDTO.libros, opciones =>
            {
                opciones.MapFrom(MapAutorDTOLibros);
            });

            //Libros

            CreateMap<LibroCreacionDTO, Libro>().ForMember(libro => libro.Autoreslibros,opciones =>
            {
                opciones.MapFrom(MapAutoresLibros);
            });

            CreateMap<Libro, LibroLecturaDTO>();

            CreateMap<Libro, LibroLecturaDTOConAutores>().ForMember(libroDTO => libroDTO.autores , opciones =>
            {
                opciones.MapFrom(MapLibrosDTOAutores);
            });

            //patch
            CreateMap<LibroPatchDTO, Libro>().ReverseMap();

            //Comentarios
            CreateMap<ComentarioCreacion, Comentario>();

            CreateMap<Comentario, ComentarioLectura>();

        }
        //Mapeamos para llenar tabla AutoresLibros
    private List<AutorLibro> MapAutoresLibros(LibroCreacionDTO librodto,Libro libro)
        {
            var resultado = new List<AutorLibro>();

            if(librodto.AutoresIds == null)
            {
                return resultado;
            }

            foreach(var autorId in librodto.AutoresIds)
            {
                resultado.Add(new AutorLibro() { AutorId=autorId});
            }

            return resultado;
        }

        //mapeamos para obtener los autores de cada libro
        private List<AutorLecturaDTO> MapLibrosDTOAutores(Libro libro,LibroLecturaDTO libroDTO)
        {
            var resultado = new List<AutorLecturaDTO>();

            if(libro.Autoreslibros == null)
            {
                return resultado;
            }

            foreach  (var autorlibro in libro.Autoreslibros)
            {
                resultado.Add(new AutorLecturaDTO() { Id = autorlibro.AutorId, Name = autorlibro.autor.Name });
            }

            return resultado;

        }
        //Mapeamos para obtener los libros de cada autor
        private List<LibroLecturaDTO> MapAutorDTOLibros (Autor autor,AutorLecturaDTO autorDto)
        {
            var resultado = new List<LibroLecturaDTO>();

            if (autor.Autoreslibros == null)
            {
                return resultado;

            }

            foreach (var autorLibro in autor.Autoreslibros)
            {
                resultado.Add(new LibroLecturaDTO() { Id = autorLibro.LibroId, titulo = autorLibro.libro.titulo });
            }

            return resultado;
        }
    }
}
