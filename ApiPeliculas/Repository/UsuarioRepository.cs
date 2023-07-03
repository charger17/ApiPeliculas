using ApiPeliculas.Data;
using ApiPeliculas.Models;
using ApiPeliculas.Models.Dtos;
using ApiPeliculas.Repository.IRepository;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using XSystem.Security.Cryptography;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ApiPeliculas.Repository
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly ApplicationDbContext _bd;
        private readonly IMapper _mapper;
        private string claveSecreta;

        public UsuarioRepository(ApplicationDbContext bd, IMapper mapper, IConfiguration configuration)
        {
            _bd = bd;
            _mapper = mapper;
            claveSecreta = configuration.GetValue<string>("ApiSettings:Secreta");
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

        public async Task<UsuarioLoginRespuestaDto> Login(UsuarioLoginDto usuarioLoginDto)
        {
            var passwordEncriptado = obtenerMD5(usuarioLoginDto.Password);

            var usuario = await _bd.Usuarios.FirstOrDefaultAsync(u => u.NombreUsuario.ToLower().Equals(usuarioLoginDto.NombreUsuario.ToLower()) && u.Password.Equals(passwordEncriptado));

            //validamos si el usuario no existe con la combianción correcta
            if (usuario == null)
            {
                return new UsuarioLoginRespuestaDto()
                {
                    Token = "",
                    Usuario = null
                };
            }

            //aqui si existe el usuario entocnes podemos procesar el login
            var manejadorToken = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(claveSecreta);

            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, usuario.NombreUsuario.ToString()),
                    new Claim(ClaimTypes.Role, usuario.Role)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new (new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = manejadorToken.CreateToken(tokenDescriptor);

            UsuarioLoginRespuestaDto usuarioLoginRespuestaDto = new UsuarioLoginRespuestaDto() 
            {
                Token = manejadorToken.WriteToken(token),
                Usuario = usuario
            };

            return usuarioLoginRespuestaDto;
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
