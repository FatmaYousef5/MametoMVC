using Mameto.Data;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mameto.viewModels
{
    public class PostvievModel
    {
        public string? PContent { get; set; }
        public List<IFormFile> Img { get; set; }
    }
}
