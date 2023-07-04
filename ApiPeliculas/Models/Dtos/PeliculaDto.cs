using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiPeliculas.Models.Dtos
{
    public class PeliculaDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        public string Nombre { get; set; }

        public byte[] RutaImagen { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        public string Descripcion { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        public int Duracion { get; set; }

        public enum tipoClasificacion
        {
            Siete,
            Trece,
            Dieciseis,
            Dieciocho
        }

        public tipoClasificacion Clasificacion { get; set; }

        public DateTime FechaCreacion { get; set; }

        public int CategoriaId { get; set; }

    }
}
