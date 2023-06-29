﻿using System.ComponentModel.DataAnnotations;

namespace ApiPeliculas.Models.Dtos
{
    public class CrearCategoriaDto
    {
        public int Id { get; set; }

        //La validación es requerida para el nombre ya que si no se crearía vacía.
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [MaxLength(60, ErrorMessage = "El numero máximo de caracteres es de 100")]
        public string Nombre { get; set; }
    }
}
