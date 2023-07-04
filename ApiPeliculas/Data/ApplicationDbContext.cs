using ApiPeliculas.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ApiPeliculas.Data
{
    public class ApplicationDbContext : IdentityDbContext<AppUsuario>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            
        }

        //Agregar los modelos aquí
        public DbSet<Categoria> Categorias { get; set; }

        public DbSet<Pelicula> Pelicula { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<AppUsuario> AppUsuarios { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
