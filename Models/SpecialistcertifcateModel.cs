using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Mameto.Data;

namespace Mameto.Models
{
    public class SpecialistcertifcateModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ImgId { get; set; }
        public string? Img { get; set; }
        public string UId { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}
