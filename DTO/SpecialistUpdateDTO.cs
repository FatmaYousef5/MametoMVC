namespace Mameto.DTO
{
    public class SpecialistUpdateDTO
    {
        public string Phone { get; set; } = "";
        public string Country { get; set; } = "";
        public string City { get; set; } = "";
        public string Address { get; set; } = "";
        public string Bio { get; set; } = "";
        public List<IFormFile> Img { get; set; }
    }
}
