using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using WebApplication1.Models;
using WebApplication1.repo;
using WebApplication1.ViewModel;
using WebApplication1.Helper;

namespace WebApplication1.Controllers
{
    public class SupplierActionController : Controller
    {
        Ireposatory<Offer> offerrepo;
        Ireposatory<Category>  categoryRepo;
        Ireposatory<Brand> brandRepo;
        Ireposatory<Supplier> supllierrepo;
        Ireposatory<Product>  productrepo;
        Ireposatory<Supplier_Product> Supplier_Productrepo;

        
        int iD ;
        public SupplierActionController(Ireposatory<Supplier_Product> Supplier_Productrepo, Ireposatory<Product> productrepo,
           Ireposatory<Offer> offerrepo, Ireposatory<Supplier> supllierrepo,
           Ireposatory<Category> categoryRepo, Ireposatory<Brand> brandRepo)
        {
            this.offerrepo = offerrepo;
            
            this.supllierrepo = supllierrepo;
            this.productrepo = productrepo;
            this.categoryRepo = categoryRepo;
            this.brandRepo = brandRepo;
            this.Supplier_Productrepo = Supplier_Productrepo;
        }
       
        [Authorize]
        public IActionResult Index()
        {
            var claims = User.Claims;

            Claim idClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            string userid = idClaim.Value;
            Supplier supplier = supllierrepo.getall().FirstOrDefault(s => s.ApplicationUserId == userid);
            List<SupplierProductVM> supplier_Products = new List<SupplierProductVM>();
            List<Product> products = new List<Product>();
            ViewData["category"] = categoryRepo.getall();
            List<Supplier_Product> supplier_ProductLIST = Supplier_Productrepo.getall().Where(s => s.SupplierID == supplier.ID && s.Quantity != 0).ToList();
            foreach(var item in supplier_ProductLIST)
            {
                SupplierProductVM supplierProduct = new SupplierProductVM();
               Product product1 = new Product();
                Supplier supplier1 = new Supplier();
                product1 = productrepo.getbyid( item.ProductID);
                supplier1 = supllierrepo.getbyid( item.SupplierID);
                supplierProduct.ID = item.ID;
                supplierProduct.PointsForProduct = item.PointsForProduct;
                supplierProduct.Price = item.Price;
                supplierProduct.NameProduct = product1.Name;
                supplierProduct.Description = product1.Description;
                supplierProduct.SSN = supplier1.SSN;
                supplierProduct.Quantity = item.Quantity;
                supplierProduct.ImageName= product1.ImageName;
                products.Add(product1);
                supplierProduct.saledQuantity = item.Quantity;
                supplierProduct.saledAmount = 0;
               
                supplier_Products.Add(supplierProduct);
            }
           ViewData["Product"] = products;
            foreach (Product product2 in products)
            {
                product2.Brand = brandRepo.getbyid( product2.BrandID);
                product2.Category = categoryRepo.getbyid( product2.CategoryID);
            }
            List<Product> products2= Supplier_Productrepo.getall("Product").Where(s => s.SupplierID != supplier.ID && s.Quantity != 0).Select(s=>s.Product).ToList();

            ViewData["products2"]= products2.Where(p => products.All(x => x.ID != p.ID));
            
            return View("dashboard", supplier_Products);
        }
        public IActionResult Edit(int id)
        {
            Supplier_Product supplier_Product = Supplier_Productrepo.getbyid(id);
            ViewData["offers"] = offerrepo.getall();
            return View(supplier_Product);
        }
        [HttpPost]
        public IActionResult Update( Supplier_Product supplier_Product)
        {
            Supplier_Productrepo.update( supplier_Product);
            //Supplier_Productrepo.update(supplier_Product);
            return RedirectToAction("Index");
        }
        public IActionResult Delete(int id)
        {
            Supplier_Product supplier_Product = Supplier_Productrepo.getbyid(id);
            Supplier_Productrepo.delete(supplier_Product);
            return RedirectToAction("Index");
        }



        public IActionResult AddProduct()
        {
            ProductVM productVM = new ProductVM();
            productVM.Categories = categoryRepo.getall().ToList();
            productVM.Brands = brandRepo.getall().ToList();
            return View(productVM);
        }
        
        [HttpPost]
        public ActionResult Insert(ProductVM product)
        {
            Product product1 = new Product();
            product1.Name = product.Name;
            product1.Description = product.Description;
            product1.ImageName = ImagesHelper.UploadImg(product.ImageNameFile, "ProductIMG");
            product1.CategoryID = product.CategoryID;
            product1.BrandID = product.BrandID;
            List<Product> products = productrepo.getall();
            int Count = 0;
            //int id;
            foreach (Product ProductItem in products)
            {
                if (ProductItem.Name == product.Name)
                {
                    product1.ID = ProductItem.ID;
                    product.ID = ProductItem.ID;
                    Count++;
                }
            }
            
            if (Count == 0)
            {
                productrepo.create(product1);
                
                product.ID = product1.ID;
                iD ++;
                return RedirectToAction("AddProductToMyList", "SupplierAction", iD);
            }
            iD = product1.ID;

            return RedirectToAction("AddProductToMyList","SupplierAction", iD);
        }




        public ActionResult AddProductToMyList(int id)
        {
            Supplier_Product supplier_Product = new Supplier_Product();
            if(id != 0)
            {

            supplier_Product.ProductID = id;
            }
            else
            {
                iD = productrepo.getall().OrderBy(c=>c.ID).LastOrDefault().ID;
                supplier_Product.ProductID = iD; 
            }
            ViewData["offers"] = offerrepo.getall();
            return View(supplier_Product);
        }
        [HttpPost]
        public async Task<IActionResult> AddProductToMyList(Supplier_Product supplierProduct)
        {
            supplierProduct.ID = 0;
            var claims = User.Claims;//keys cookie (NameIdentifier,Name
            Claim idClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            string userid = idClaim.Value;
            Supplier supplier =supllierrepo.getall().FirstOrDefault(s => s.ApplicationUserId == userid);
            supplierProduct.SupplierID = supplier.ID;
            //if (ModelState.IsValid == true)
            //{
            int count = 0;
            List<Supplier_Product> supplier_ProductLIST = Supplier_Productrepo.getall().Where(s => s.SupplierID == supplier.ID).ToList();
            foreach (Supplier_Product supplierProductItem in supplier_ProductLIST)
            {
                if(supplierProductItem.ProductID == supplierProduct.ProductID)
                {
                    count++;
                    supplierProductItem.Quantity += supplierProduct.Quantity;
                    Supplier_Productrepo.update(supplierProductItem);
                }
            }
            if(count == 0)
            {

            Supplier_Productrepo.create(supplierProduct);
            }
                
                return this.RedirectToAction("Index");
            //}
            //else
            //{
            //    foreach (var item in result.Errors)
            //    {
            //        ModelState.AddModelError("", item.Description);
            //    }
            //    return View(supplierProduct);
            //}
            //return View(supplierProduct);
        }




     
    }
}
