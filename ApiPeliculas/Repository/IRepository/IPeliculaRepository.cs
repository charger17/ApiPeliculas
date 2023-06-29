using ApiPeliculas.Models;

namespace ApiPeliculas.Repository.IRepository
{
    public interface IPeliculaRepository
    {
        ICollection<Pelicula> GetPeliculas();

        Pelicula GetPelicula(int peliculaId);

        bool ExistePelicula(string nombre);

        bool ExistePelicula(int id);

        bool CrearPelicula(Pelicula pelicula);
        
        bool ActualizarPelicula(Pelicula pelicula);

        bool BorrarPelicula(Pelicula pelicula);

        //Métodos para buscar peliculas en categorias y buscar pelicula por nombre
        ICollection<Pelicula> GetPeliculasEnCategoria(int categoriaId);
        ICollection<Pelicula> BuscarPelicula(string nombre);


        bool Guardar();
    }
}
