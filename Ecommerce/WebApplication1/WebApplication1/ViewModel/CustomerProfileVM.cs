using WebApplication1.Models;
namespace WebApplication1.ViewModel
{
    public class CustomerProfileVM
    {
        public int CustomId { get; set; }
       //public string ApplicationUserId { get; set; }
        public string userName { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string? Image { get; set; }
        public IFormFile ImageFile { get; set; }
    }
}
