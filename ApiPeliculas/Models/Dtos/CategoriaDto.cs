using System.ComponentModel.DataAnnotations;

namespace ApiPeliculas.Models.Dtos
{
    public class CategoriaDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [MaxLength(60, ErrorMessage = "El numero máximo de caracteres es de 100")]
        public string Nombre { get; set; }

    }
}
