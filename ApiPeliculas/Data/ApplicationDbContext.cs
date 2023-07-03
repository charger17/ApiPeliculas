using ApiPeliculas.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiPeliculas.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            
        }

        //Agregar los modelos aquí
        public DbSet<Categoria> Categorias { get; set; }

        public DbSet<Pelicula> Pelicula { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }

    }
}
