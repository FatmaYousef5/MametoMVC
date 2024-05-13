using Mameto.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mameto.Models
{
    public class CommentModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CId { get; set; }
        public string? CContent { get; set; }
        public string UId { get; set; }
        [ForeignKey("UId")]
        public virtual ApplicationUser User { get; set; }
        public int PId { get; set; }
        [ForeignKey("PId")]
        public virtual PostModel Post { get; set; }
        public virtual IList<commentImgsModel> CommentImgs { get; set; }
    }
}
