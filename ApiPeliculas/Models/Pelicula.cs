using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiPeliculas.Models
{
    public class Pelicula
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Nombre { get; set; }

        public string RutaImagen { get; set; }

        public string Descripcion { get; set; }

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

        [ForeignKey("categoriaId")]
        public int CategoriaId { get; set; }

        public Categoria Categoria { get; set; }


    }
}
