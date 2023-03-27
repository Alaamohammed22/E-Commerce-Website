using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{
    public class Order
    {
        public int ID { get; set; }
        public DateTime Date { get; set; }
        public double TotalPrice { get; set; }
        public string PaidMethod { get; set; }
        public string CrediteNumber { get; set; }
        public string OrderState { get; set; }
        public virtual Customer Customer { get; set; }
        public int CustomerID { get; set; }
        public virtual Delivary Delivary { get; set; }
        public int? DelivaryID { get; set; }

        public virtual List<Supplier_Product_Order> Supplier_Product_Orders { get; set; }

    }
}
