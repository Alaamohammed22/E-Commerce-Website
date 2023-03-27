using WebApplication1.Models;
namespace WebApplication1.ViewModel
{
    public class ProductVM
    {
        public int ID { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
        public string? ImageName { get; set; }
        public IFormFile ImageNameFile { get; set; }
        public int CategoryID { get; set; }
        public  List<Category> Categories { get; set; }
        public int BrandID { get; set; }
        public List<Brand> Brands { get; set; }
        //public virtual List<Supplier_Product> Supplier_Products { get; set; }
    }
}
