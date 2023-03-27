using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.ViewModel
{
    public class RegistrationVM
    {
        [Required]
        public string UserName { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }
        [DataType(DataType.Password)]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
        public string Address { get; set; }

        public string? ImageName { get; set; }
        public IFormFile ImageFile { get; set; }
        public string? RoleName { get; set; }
        public List<IdentityRole>? IdentityRoleS { get; set; }
        //public List<string> Roles { get; set; } = new List<string>() { "Customer", "Supplier", "Delivery" };
    }
}
