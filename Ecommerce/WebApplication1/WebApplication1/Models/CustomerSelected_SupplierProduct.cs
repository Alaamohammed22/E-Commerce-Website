namespace WebApplication1.Models
{
    public class CustomerSelected_SupplierProduct
    {
        public int ID { get; set; }
        public virtual Customer Customer { get; set; }
        public int CustomerID { get; set; }
        public Supplier_Product Supplier_Product { get; set; }
        public int Supplier_ProductID { get; set; }
        public int Quantity { get; set; }
        public double TotalPrice { get; set; } // *
    
    }
}
