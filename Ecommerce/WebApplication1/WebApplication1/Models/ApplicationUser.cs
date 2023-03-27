using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{
    public class ApplicationUser:IdentityUser
    {
        public string Address { get; set; }
        public string ImageName { get; set; }
        public bool? IsDeleted { get; set; }
        public virtual List<Supplier> Suppliers { get; set; }
        public virtual List<Delivary> Delivaries { get; set; }
        public virtual List<Customer> Customers { get; set; }

    }
}
