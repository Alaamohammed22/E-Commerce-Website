namespace WebApplication1.Models
{
    public class Category_Brand
    {
        public int ID { get; set; }
        public virtual Category Category { get; set; }
        public int CategoryID { get; set; }
        public virtual Brand Brand { get; set; }
        public int BrandID { get; set; }    

    }
}
