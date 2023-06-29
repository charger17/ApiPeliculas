using ApiPeliculas.Data;
using ApiPeliculas.Models;
using ApiPeliculas.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ApiPeliculas.Repository
{
    public class CategoriaRepository : ICategoriaRepository
    {
        private readonly ApplicationDbContext _bd;

        public CategoriaRepository(ApplicationDbContext bd)
        {
            _bd = bd;
        }

        public bool ActualizarCategoria(Categoria categoria)
        {
            categoria.FechaCreacion = DateTime.Now;
            _bd.Categorias.Update(categoria);
            return Guardar();
        }

        public bool BorrarCategoria(Categoria categoria)
        {
            _bd.Categorias.Remove(categoria);
            return Guardar();
        }

        public bool CrearCategoria(Categoria categoria)
        {
            categoria.FechaCreacion = DateTime.Now;
            _bd.Categorias.Add(categoria);
            return Guardar();
        }

        public bool ExisteCategoria(string nombre)
        {
            return _bd.Categorias.Any(x => x.Nombre.ToLower().Trim() == nombre.ToLower().Trim());
        }

        public bool ExisteCategoria(int id)
        {
            return _bd.Categorias.Any(x => x.Id.Equals(id));
        }

        public Categoria GetCategoria(int categoriaId)
        {
            return _bd.Categorias.FirstOrDefault(c => c.Id.Equals(categoriaId));
        }

        public ICollection<Categoria> GetCategorias()
        {
            throw new NotImplementedException();
        }

        public bool Guardar()
        {
            throw new NotImplementedException();
        }
    }
}
