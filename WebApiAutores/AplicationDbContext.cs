using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebApiAutores.Entity;

namespace WebApiAutores
{
    public class AplicationDbContext : IdentityDbContext
    {
        public AplicationDbContext(DbContextOptions opions) : base(opions)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<AutorLibro>().HasKey(al => new {al.AutorId,al.LibroId});
        }

        //crear tabla Autor
        public DbSet<Autor> Autores { get; set; }

        //crear tabla libro
        public DbSet<Libro> Libros { get; set; }

        //crear tabla comentario
        public DbSet<Comentario> Comentarios { get; set; }

        //crear tabla de join relacion muchos a muchos 
        public DbSet<AutorLibro> AutoresLibros { get; set; }
    }
}
