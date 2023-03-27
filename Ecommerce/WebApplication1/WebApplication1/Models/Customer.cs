namespace WebApplication1.Models
{
    public class Customer
    {
        public int ID { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
        public string ApplicationUserId { get; set; }
        public bool? IsDeleted { get; set; }
        public virtual List<Order> Orders { get; set; }
        public virtual List<CustomerSelected_SupplierProduct> SelectedItems { get; set; }

        public int TotalPoint { get; set; }

        public virtual List<Review> Reviews { get; set;}

    }
}
