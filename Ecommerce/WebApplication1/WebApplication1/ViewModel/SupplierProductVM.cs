using WebApplication1.Models;
namespace WebApplication1.ViewModel
{
    public class SupplierProductVM
    {
        public int ID { get; set; }

        public int ProductID { get; set; }
        public int SupplierID { get; set; }
        public int PointsForProduct { get; set; }
        public string Specifications { get; set; }
        public string NameProduct { get; set; }
        public string SSN { get; set; }
        public int Quantity { get; set; }
        public string Description { get; set; }

        public string ImageName { get; set; }
        public int saledQuantity { get; set; }
        public double saledAmount { get; set; }

        public List<Product> Products { get; set; }
        public double Price { get; set; }
       
        public int? OfferID { get; set; }
        public List<Offer> offers { get; set; }
    }
}
