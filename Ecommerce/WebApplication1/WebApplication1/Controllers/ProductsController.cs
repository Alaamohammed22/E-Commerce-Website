using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.FlowAnalysis.DataFlow;
using NuGet.Protocol.Core.Types;
using System.Security.Claims;
using System.Security.Policy;
using WebApplication1.Models;
using WebApplication1.repo;

namespace WebApplication1.Controllers
{
    public class ProductsController : Controller
    {
        UserManager<ApplicationUser> userManager;
        Ireposatory<Customer> CustomerRepo;
        Ireposatory< Supplier_Product> Supplierreproduct;
        Ireposatory<Category> CategoryRepositry;
        Ireposatory<Product> ProductRepositry;
        Ireposatory<Review> ReviewRepositry;
        Ireposatory<Supplier_Product> supProductRepo;
        Ireposatory<CustomerSelected_SupplierProduct> selectedItemsRepo;

        public ProductsController
            (UserManager<ApplicationUser> userManager,Ireposatory<Supplier_Product> Supplierreproduct, Ireposatory<Category> CategoryRepositry, Ireposatory<Product> ProductRepositry, Ireposatory<Customer> CustomerRepo
           ,Ireposatory<Review> ReviewRepositry, Ireposatory<Supplier_Product> supProductRepo, Ireposatory<CustomerSelected_SupplierProduct> selectedItemsRepo)
        {
            this.userManager=userManager;
            this.Supplierreproduct = Supplierreproduct;
            this.ProductRepositry = ProductRepositry;
            this.CategoryRepositry = CategoryRepositry;
            this.CustomerRepo = CustomerRepo;
            this.supProductRepo= supProductRepo;
            this.selectedItemsRepo = selectedItemsRepo;
            this.ReviewRepositry = ReviewRepositry;
        }


        public IActionResult Index()
        {
            ViewData["category"] = CategoryRepositry.getall() ;
            List<Supplier_Product> ProductModel = Supplierreproduct.getall("Product","Supplier").Where(sp=>sp.Supplier.IsDeleted==false).ToList();
            return View("Index", ProductModel);
        }
         

        public async Task<IActionResult> Deatils(int id)
        {
            Supplier_Product ProductModel = Supplierreproduct.getall("Product", "Reviews").FirstOrDefault(s=>s.ID==id);
            ProductModel.Product = ProductRepositry.getall("Brand").FirstOrDefault(p=>p.ID==ProductModel.ProductID);
           
            
            ProductModel.Reviews = ReviewRepositry.getall("Customer").Where(r=>r.Supplier_ProductID==id).ToList();  
            foreach(Review item in ProductModel.Reviews)
            {
                item.Customer = CustomerRepo.getall("ApplicationUser").FirstOrDefault(c=>c.ID==item.CustomerId);
            }

            if (ProductModel.Quantity>0)
            {
                ViewData["Availability"] = "In Stock";
                ViewData["Style"] = "text-success";
            }
            else
            {
                ViewData["Availability"] = "Un Available";
                ViewData["Style"] = "text-danger";
            }
            Claim idClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            string userid = idClaim.Value;
            ApplicationUser applicationUser = await userManager.FindByIdAsync(userid);
            ViewData["imagename"] = applicationUser.ImageName;
            ViewData["relatedProducts"] = Supplierreproduct.getall("Product").Where(sp=>sp.Product.BrandID== ProductModel.Product.BrandID) ; 
            return View("Deatils", ProductModel);
        }
       
        public IActionResult productbycategory(int categoryID)
        {
            if(categoryID==0)
            {
                List<Supplier_Product> AllProducts = Supplierreproduct.getall("Product", "Supplier").Where(sp => sp.Supplier.IsDeleted == false).ToList();
                return Json(AllProducts);//new { name = AllProducts }//new { name = "" }, new { name = "" }
            }
            
            List<Supplier_Product> CategoryProduct = Supplierreproduct.getall("Product", "Supplier").Where(sp => sp.Supplier.IsDeleted == false && sp.Product.CategoryID == categoryID).ToList();
            return Json(CategoryProduct);//new { name=CategoryProduct }// name= new { age=50} },new { name=""

        }

        public void AddToCart(int supplierproductID, int quantity)//SelectedItemsInCartVM selectedItems)
        {
            Claim idClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            string userid = idClaim.Value;
            List<Customer> customers = CustomerRepo.getall();
            Customer customer = customers.FirstOrDefault(c => c.ApplicationUserId == userid);
            Supplier_Product s = supProductRepo.getbyid(supplierproductID);
            Product p = ProductRepositry.getbyid(s.ProductID);
            CustomerSelected_SupplierProduct selected_Item = selectedItemsRepo.getall().FirstOrDefault(item => item.Supplier_ProductID == supplierproductID);

            if (selected_Item == null)
            {
                CustomerSelected_SupplierProduct selected_SupplierProduct = new CustomerSelected_SupplierProduct();
                selected_SupplierProduct.CustomerID = customer.ID;
                selected_SupplierProduct.Supplier_ProductID = supplierproductID;
                selected_SupplierProduct.Quantity = quantity;
                selected_SupplierProduct.TotalPrice = s.Price * quantity;
                selectedItemsRepo.create(selected_SupplierProduct);
            }
            else
            {
                selected_Item.Quantity += quantity;
                selected_Item.TotalPrice = s.Price* selected_Item.Quantity;
                selectedItemsRepo.update(selected_Item);
            }
            
        }


        public async Task<IActionResult> GetTotalQuantity(string name)
        {
            ApplicationUser applicationUser = await userManager.FindByNameAsync(name);
            Customer customer = CustomerRepo.getall().FirstOrDefault(c=>c.ApplicationUserId==applicationUser.Id);
            return Json(selectedItemsRepo.getall().Where(s=>s.CustomerID== customer.ID).Sum(s=>s.Quantity));
        }


        public IActionResult review(int supplierproductid, string comment)
        {
            Claim idClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            string userid = idClaim.Value;
            Customer customer = CustomerRepo.getall().FirstOrDefault(c => c.ApplicationUserId == userid);
            DateTime dateTime = DateTime.Now;
            Review review = new Review();
            review.Supplier_ProductID = supplierproductid;
            review.ReviewText = comment;
            review.Date = dateTime;
            review.CustomerId = customer.ID;
            ReviewRepositry.create(review);
            return Json(review);
        }


    }
}
