using ApiPeliculas.Data;
using ApiPeliculas.Models;
using ApiPeliculas.Models.Dtos;
using ApiPeliculas.Repository.IRepository;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using XSystem.Security.Cryptography;

namespace ApiPeliculas.Repository
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly ApplicationDbContext _bd;
        private readonly IMapper _mapper;
        private string claveSecreta;
        private readonly UserManager<AppUsuario> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UsuarioRepository(ApplicationDbContext bd, IMapper mapper, IConfiguration configuration, UserManager<AppUsuario> userManager, RoleManager<IdentityRole> roleManager)
        {
            _bd = bd;
            _mapper = mapper;
            _userManager = userManager;
            claveSecreta = configuration.GetValue<string>("ApiSettings:Secreta");
            _roleManager = roleManager;
        }

        public AppUsuario GetUsuario(string usuarioId)
        {
            return _bd.AppUsuarios.FirstOrDefault(x => x.Id.Equals(usuarioId));
        }

        public ICollection<AppUsuario> GetUsuarios()
        {
            return _bd.AppUsuarios.OrderBy(u => u.UserName).ToList();
        }

        public bool IsUniqueUser(string nombre)
        {
            var usuarioBd = _bd.AppUsuarios.FirstOrDefault(x => x.UserName.Equals(nombre));

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
            //var passwordEncriptado = obtenerMD5(usuarioLoginDto.Password);

            var usuario = await _bd.AppUsuarios.FirstOrDefaultAsync(u => u.NormalizedEmail.Equals(usuarioLoginDto.NombreUsuario.ToUpper()));

            bool isValid = await _userManager.CheckPasswordAsync(usuario, usuarioLoginDto.Password);

            //validamos si el usuario no existe con la combianción correcta
            if (usuario == null || isValid is false)
            {
                return new UsuarioLoginRespuestaDto()
                {
                    Token = "",
                    Usuario = null
                };
            }

            //aqui si existe el usuario entocnes podemos procesar el login
            var roles = await _userManager.GetRolesAsync(usuario);

            var manejadorToken = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(claveSecreta);

            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, usuario.UserName.ToString()),
                    new Claim(ClaimTypes.Role, roles.FirstOrDefault())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new (new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = manejadorToken.CreateToken(tokenDescriptor);

            UsuarioLoginRespuestaDto usuarioLoginRespuestaDto = new UsuarioLoginRespuestaDto()
            {
                Token = manejadorToken.WriteToken(token),
                Usuario = _mapper.Map<UsuarioDatosDto>(usuario)
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
