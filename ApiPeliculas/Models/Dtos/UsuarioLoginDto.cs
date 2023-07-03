﻿using System.ComponentModel.DataAnnotations;

namespace ApiPeliculas.Models.Dtos
{
    public class UsuarioLoginDto
    {
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        public string NombreUsuario { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        public string Password { get; set; }

    }
}
