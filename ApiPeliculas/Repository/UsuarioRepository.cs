using ApiPeliculas.Data;
using ApiPeliculas.Models;
using ApiPeliculas.Models.Dtos;
using ApiPeliculas.Repository.IRepository;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using XSystem.Security.Cryptography;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ApiPeliculas.Repository
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly ApplicationDbContext _bd;
        private readonly IMapper _mapper;

        public UsuarioRepository(ApplicationDbContext bd, IMapper mapper)
        {
            _bd = bd;
            _mapper = mapper;
        }

        public Usuario GetUsuario(int usuarioId)
        {
            return _bd.Usuarios.FirstOrDefault(x => x.Id.Equals(usuarioId));
        }

        public ICollection<Usuario> GetUsuarios()
        {
            return _bd.Usuarios.OrderBy(u => u.NombreUsuario).ToList();
        }

        public bool IsUniqueUser(string nombre)
        {
            var usuarioBd = _bd.Usuarios.FirstOrDefault(x => x.NombreUsuario.Equals(nombre));

            return usuarioBd is null ? true : false;
        }

        public async Task<Usuario> Registro(UsuarioRegistroDto usuarioRegistroDto)
        {
            var passwordEncriptado = obtenerMD5(usuarioRegistroDto.Password);

            Usuario usuario = new Usuario();
            
            usuario = _mapper.Map<Usuario>(usuarioRegistroDto);
            usuario.Password = passwordEncriptado;

            await _bd.Usuarios.AddAsync(usuario);
            await _bd.SaveChangesAsync();

            usuario.Password = passwordEncriptado;
            return usuario;
        }

        public Task<UsuarioLoginRespuestaDto> Login(UsuarioLoginDto usuarioLoginDto)
        {
            throw new NotImplementedException();
        } 
        

        /// <summary>
        /// Metodo para encriptar contraseña
        /// </summary>
        /// <param name="password">Contraseña del usuario</param>
        /// <returns>Contraseña encryptada</returns>
        private string obtenerMD5(string password)
        {
            MD5CryptoServiceProvider x = new MD5CryptoServiceProvider();
            byte[] data = System.Text.Encoding.UTF8.GetBytes(password);
            data = x.ComputeHash(data);
            string resp = "";
            for (int i = 0; i < data.Length; i++)
            {
                resp += data[i].ToString("x2").ToLower();
            }
            return resp;
        }
    }
}
