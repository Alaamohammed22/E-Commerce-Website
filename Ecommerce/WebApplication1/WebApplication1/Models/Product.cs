using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace WebApplication1.Models
{
    public class Product
    {
        public int ID { get; set; }

        //
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageName { get; set; }
        public int CategoryID { get; set; }
        public virtual Category Category { get; set; }  
        public int BrandID { get; set; }
        public virtual Brand Brand { get; set; }
        public virtual List<Supplier_Product> Supplier_Products { get; set; }

    }
}
