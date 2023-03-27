namespace WebApplication1.Models
{
    public class Review
    {
        public int ID { get; set; }
        public virtual Supplier_Product Supplier_Product { get; set; }
        public int Supplier_ProductID { get; set; }
        public string ReviewText { get; set; }
        public DateTime Date { get; set; }

        public virtual Customer Customer { get; set; }
        public int CustomerId { get; set; }
    }
}
