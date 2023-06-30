using ApiPeliculas.Data;
using ApiPeliculas.Models;
using ApiPeliculas.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ApiPeliculas.Repository
{
    public class PeliculaRepository : IPeliculaRepository
    {
        private readonly ApplicationDbContext _bd;

        public PeliculaRepository(ApplicationDbContext bd)
        {
            _bd = bd;
        }

        public bool ActualizarPelicula(Pelicula pelicula)
        {
            pelicula.FechaCreacion = DateTime.Now;
            _bd.Pelicula.Update(pelicula);
            return Guardar();
        }

        public bool BorrarPelicula(Pelicula pelicula)
        {
            _bd.Pelicula.Remove(pelicula);
            return Guardar();
        }

        

        public bool CrearPelicula(Pelicula pelicula)
        {
            pelicula.FechaCreacion = DateTime.Now;
            _bd.Pelicula.Add(pelicula);
            return Guardar();
        }

        public bool ExistePelicula(string nombre)
        {
            return _bd.Pelicula.Any(x => x.Nombre.ToLower().Trim() == nombre.ToLower().Trim());
        }

        public bool ExistePelicula(int id)
        {
            return _bd.Pelicula.Any(x => x.Id.Equals(id));
        }

        public Pelicula GetPelicula(int PeliculaId)
        {
            return _bd.Pelicula.FirstOrDefault(c => c.Id.Equals(PeliculaId));
        }

        public ICollection<Pelicula> GetPeliculas()
        {
            return _bd.Pelicula.OrderBy(c => c.Nombre).ToList();
        }

        public ICollection<Pelicula> BuscarPelicula(string nombre)
        {
            IQueryable<Pelicula> query = _bd.Pelicula;

            if (!string.IsNullOrEmpty(nombre))
            {
                query = query.Where(e => e.Nombre.Contains(nombre) || e.Descripcion.Contains(nombre));
            }

            return query.ToList();
        }

        public ICollection<Pelicula> GetPeliculasEnCategoria(int categoriaId)
        {
            return _bd.Pelicula.Include(ca => ca.Categoria).Where(ca => ca.CategoriaId.Equals(categoriaId)).ToList();
        }

        public bool Guardar()
        {
            return _bd.SaveChanges() >= 0;
        }
    }
}
