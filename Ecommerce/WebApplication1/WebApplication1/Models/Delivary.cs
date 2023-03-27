namespace WebApplication1.Models
{
    public class Delivary
    {
        public int ID { get; set; }
        public string SSN { get; set; }
        public string SSNImageName { get; set; }
        public bool IsBusy { get; set; }
        public string AccountNumber { get; set; }
        public bool? IsDeleted { get; set; }
        public virtual ApplicationUser? ApplicationUser { get; set; }
        public string? ApplicationUserId { get; set; }

        public virtual List<Order>? Orders { get; set; }

    }
}
