using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FShop6.Areas.Admin.Models
{

    [Table("HinhAnh")]
    public class AnhBienTheModel
    {
        [Key]
        public int MaHinhAnh { get; set; }
        [Required]
        public int MaBienThe { get; set; }
        [Required]
        public string URL { get; set; }
        [Required]
        public string mota { get; set; }

        [ForeignKey("MaBienThe")]
        public BienTheModels BienThe { get; set; }
    }
}
