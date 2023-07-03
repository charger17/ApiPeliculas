using ApiPeliculas.Models;
using ApiPeliculas.Models.Dtos;
using AutoMapper;

namespace ApiPeliculas.PeliculasMapper
{
    public class PeliculasMapper : Profile
    {
        public PeliculasMapper()
        {
            //Categoria mapper
            CreateMap<Categoria, CategoriaDto>().ReverseMap();
            CreateMap<Categoria, CrearCategoriaDto>().ReverseMap();

            //Peliculas mapper
            CreateMap<Pelicula, PeliculaDto>().ReverseMap();

            //Usuario mapper
            CreateMap<Usuario, UsuarioDto>().ReverseMap();
            CreateMap<Usuario, UsuarioRegistroDto>().ReverseMap();
            CreateMap<Usuario, UsuarioLoginDto>().ReverseMap();

        }
    }
}
