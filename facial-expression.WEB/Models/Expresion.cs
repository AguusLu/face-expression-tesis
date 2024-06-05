using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace facial_expression.WEB.Models
{
    public class Expresion

    {
        [Key]
        public int id { get; set; }

        [NotMapped]
        public IFormFile? ImageFile { get; set; }

        public string nombreImagen { get; set; }

        public String clasificacion { get; set; }

    }
}
