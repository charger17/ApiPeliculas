using ApiPeliculas.Models;
using ApiPeliculas.Models.Dtos;

namespace ApiPeliculas.Repository.IRepository
{
    public interface IUsuarioRepository
    {
        ICollection<AppUsuario> GetUsuarios();

        AppUsuario GetUsuario(string usuarioId);

        bool IsUniqueUser(string nombre);

        Task<UsuarioLoginRespuestaDto> Login(UsuarioLoginDto usuarioLoginDto);

        Task<Usuario> Registro(UsuarioRegistroDto usuarioRegistroDto);

    }
}
