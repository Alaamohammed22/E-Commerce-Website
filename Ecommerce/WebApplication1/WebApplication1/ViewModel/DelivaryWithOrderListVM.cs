using Microsoft.EntityFrameworkCore.Metadata.Internal;
using WebApplication1.Models;

namespace WebApplication1.ViewModel
{
    public class DelivaryWithOrderListVM
    {
        public int ID { get; set; }
        public string UserName { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
         public string? Image { get; set; }
        public IFormFile ImageFile { get; set; }  
        public string SSN { get; set; }

        public string? SSNImageName { get; set; }
        public IFormFile SSNImageFile { get; set; }
        public bool IsBusy { get; set; }
        public string AccountNumber { get; set; }
        public string? ApplicationUserId { get; set; }
        public virtual List<Order>? Orders { get; set; }

       

    }
}
