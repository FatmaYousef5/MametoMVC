using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mameto.Models
{
    public class commentImgsModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ImgId { get; set; }
        public string? Img { get; set; }
        public int commentId { get; set; }
        public virtual CommentModel comment { get; set; }
    }
}
