namespace WebApplication1.Models
{
    public class Brand
    {
         public int ID { get; set; }
        public string Name { get; set; }
        public virtual List<Product> Products { get; set; }

        public virtual List<Category_Brand> Category_Brand { get; set; }


    }
}
