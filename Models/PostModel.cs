using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Mameto.Data;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Mameto.Models
{
    public class PostModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PId { get; set; }
        public string? PContent { get; set; }
        public string UId { get; set;}
        [ForeignKey("UId")]
        public virtual ApplicationUser User { get; set; }
        public virtual ICollection<CommentModel> Comment { get; set; } = new HashSet<CommentModel>();
        public virtual IList<postImgsModel> PostImgs { get; set; }

    }
}
