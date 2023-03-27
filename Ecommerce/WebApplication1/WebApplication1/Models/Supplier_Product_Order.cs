namespace WebApplication1.Models
{
    public class Supplier_Product_Order
    {
        public int ID { get; set; }
        public virtual Order Order { get; set; }
        public int OrderID { get; set; }
        public virtual Supplier_Product Supplier_Product { get; set; }
        public int Supplier_ProductID { get; set;}
        public int Quntity { get; set; }
        public double TotalPrice { get; set; } 

    }
}
