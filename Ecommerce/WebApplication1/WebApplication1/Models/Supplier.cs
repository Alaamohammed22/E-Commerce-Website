namespace WebApplication1.Models
{
    public class Supplier
    {
        public int ID { get; set; }
        public string SSN { get; set; } 
        public string SSNImageName { get; set; }
        public string VerifecationState { get; set; }
        public double TotalSales { get; set; }
        public string AccountNumber { get; set; }
        public bool? IsDeleted { get; set; }
        public virtual ApplicationUser? ApplicationUser { get; set; }
        public string? ApplicationUserId { get; set; }

        public virtual List<Supplier_Product>? Supplier_Products { get; set; }

    }
}
