using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mameto.Models
{
    public class postImgsModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ImgId { get; set; }
        public string? Img { get; set; }
        public int PostId { get; set; }
        public virtual PostModel Post { get; set; }
    }
}
