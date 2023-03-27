using WebApplication1.Models;
namespace WebApplication1.ViewModel
{
    public class SelectedItemsInCartVM
    {
        public int selectedItemId { get; set; }
        public int CustomId { get; set; }
        public int supplier_ProductId { get; set; }
        public string  Image { get; set; }
        public string Description { get; set; }
        public string productName{ get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
        public double totalPrice { get; set; }

        //public List<CustomerSelected_SupplierProduct> Selected_items { get; set; }
        //public List<Supplier_Product> Selected_items { get; set; }
    }
}
