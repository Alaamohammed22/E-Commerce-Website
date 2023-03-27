namespace WebApplication1.Models
{
    public class Supplier_Product
    {
        public int ID { get; set; }
        public virtual Product Product { get; set; }
        public int ProductID { get; set; }

        public virtual Supplier Supplier { get; set; }
        public int SupplierID { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }

        public int PointsForProduct { get; set; }
        public string Specifications { get; set; }
        public virtual Offer Offer { get; set; }
        public int OfferID { get; set; }
        public virtual List<Review> Reviews { get; set; }
        public virtual List<CustomerSelected_SupplierProduct> SelectedItems { get; set; }
        public virtual List<Supplier_Product_Order> Supplier_Product_Orders { get; set; }


    }
}
