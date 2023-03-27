using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace WebApplication1.Models
{
    public class Context : IdentityDbContext<ApplicationUser> //DbContext
    {
        //public Context() : base()
        //{

        //}

        public Context(DbContextOptions option) : base(option)
        {

        }


        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlServer(@"Data Source=DESKTOP-SH1SPK1\SQL2019;Initial Catalog=E_CommerceSystem;Integrated Security=True;Encrypt=False");
        //    base.OnConfiguring(optionsBuilder);
        //}


        //protected override void OnModelCreating(ModelBuilder builder)
        //{

        //}

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<Delivary> Delivaries { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Category_Brand> Category_Brands { get; set; }
        public DbSet<Offer> Offers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Supplier_Product> Supplier_Products { get; set; }
        public DbSet<CustomerSelected_SupplierProduct> SelectedItems { get; set; }
        public DbSet<Supplier_Product_Order> Supplier_Product_Orders { get; set; }




    }


   
}
