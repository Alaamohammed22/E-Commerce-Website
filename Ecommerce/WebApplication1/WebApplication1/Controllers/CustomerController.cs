using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.ViewModel;
using WebApplication1.Models;
using WebApplication1.repo;
using System.Security.Claims;
using WebApplication1.Helper;

namespace WebApplication1.Controllers
{
    public class CustomerController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        Ireposatory<Customer> repo;
        Ireposatory<Product> productRepo;
        Ireposatory<Supplier_Product> supProductRepo;
        Ireposatory<CustomerSelected_SupplierProduct> selectedItemsRepo;
        Ireposatory<Supplier_Product_Order> Supplier_Product_OrderRepo;
        Ireposatory<Order> ordersRepo;
        public CustomerController(Ireposatory<Order> _ordersRepo,Ireposatory<CustomerSelected_SupplierProduct> _selectedItemsRepo, Ireposatory<Supplier_Product> _supProductRepo,Ireposatory<Product> _productRepo,Ireposatory<Customer> _repo,UserManager<ApplicationUser> _userManager, Ireposatory<Supplier_Product_Order> Supplier_Product_OrderRepo) {
            repo = _repo;
            userManager = _userManager;
            productRepo = _productRepo;
            supProductRepo = _supProductRepo;
            selectedItemsRepo = _selectedItemsRepo;
            ordersRepo = _ordersRepo;
            this.Supplier_Product_OrderRepo = Supplier_Product_OrderRepo;
            //context = _context;
        }

        public async Task<IActionResult> index()
        {
            Claim idClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            string userid = idClaim.Value;
            Claim nameClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name);
            string username = nameClaim.Value;
            Customer customer = repo.getall().FirstOrDefault(c => c.ApplicationUserId == userid);
            CustomerProfileVM c = new CustomerProfileVM();
            c.CustomId = customer.ID;
            c.userName = username;
            ViewData["id"] = c.CustomId;
            ViewData["Username"] = c.userName;
            List<Supplier_Product> products = supProductRepo.getall("Product");
            //ViewData["ProductName"] = ;
           // int Id;
            //foreach (Supplier_Product s in products)
            //{
            //    Id = s.ProductID;
            //    Product p = productRepo.getbyid(Id);
            //    //ViewData["ProductName"] = p.Name;
            //    //ViewData["ProductImage"] = p.ImageName;
            //    //ViewData["SupplierId"] = s.ID;
             
            //}
            return View(products);
        }
        public async Task< IActionResult> CustomerProfile()
        {
            Claim idClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            string userid = idClaim.Value;
            //Customer c=repo.getbyidCustom(c=>c.ID==id && c.ApplicationUserId==c.ApplicationUser.Id);
           // Customer customer=repo.getbyid(id);
            Customer customer = repo.getall().FirstOrDefault(c => c.ApplicationUserId == userid);
            ApplicationUser user = await userManager.FindByIdAsync(customer.ApplicationUserId);
            CustomerProfileVM c=new CustomerProfileVM();
            c.CustomId = customer.ID;
            //ViewData["id"] = c.CustomId;
            c.userName = user.UserName;
            c.Address=user.Address;
            c.Phone = user.PhoneNumber;
            c.Email = user.Email;
            c.Image = user.ImageName;

            return View(c);
        }
        [HttpPost]
        public async Task< IActionResult> CustomerProfile(CustomerProfileVM customer)
        {
           Customer customer1 = repo.getbyid(customer.CustomId);
            ApplicationUser user = await userManager.FindByIdAsync(customer1.ApplicationUserId);
            if (customer != null)
            {
               //Customer oldEmp = new Customer();
                customer1.ApplicationUser.Address=customer.Address;
                customer1.ApplicationUser.PhoneNumber = customer.Phone;
                customer1.ApplicationUser.Email = customer.Email;
                customer1.ApplicationUser.UserName = customer.userName;
                customer1.ApplicationUser.ImageName = ImagesHelper.UploadImg(customer.ImageFile, "ProfileIMG");
                repo.update(customer1);
                return RedirectToAction("CustomerProfile", new {id=customer.CustomId});
            }
            return View(customer);
            
        }
        public IActionResult ShowCart()
        {
            Claim idClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            string userid = idClaim.Value;
            List<Customer> customers = repo.getall();
            Customer customer = customers.FirstOrDefault(c => c.ApplicationUserId == userid);
            List<CustomerSelected_SupplierProduct> selectedItems = selectedItemsRepo.getall().Where(s => s.CustomerID == customer.ID).ToList(); ;
            List<SelectedItemsInCartVM> selectedItemsInCarts = new List<SelectedItemsInCartVM>();

            double totalprice = 0;

            foreach (CustomerSelected_SupplierProduct item in selectedItems)
            {
                Supplier_Product s1 = supProductRepo.getbyid(item.Supplier_ProductID);
                Product p = productRepo.getbyid(s1.ProductID);
                SelectedItemsInCartVM selectedItems1=new SelectedItemsInCartVM()
                {
                    Price = s1.Price,
                    CustomId=customer.ID,
                    Description=p.Description,
                    Image=p.ImageName,
                    productName=p.Name,
                    Quantity= item.Quantity,
                    selectedItemId =item.ID,
                    totalPrice= item.TotalPrice,
                };
                totalprice += selectedItems1.totalPrice;
                selectedItemsInCarts.Add(selectedItems1);
            }
            ViewData["totalprice"] = totalprice;
            return View("Cart",selectedItemsInCarts);
            //return View(selectedItemsInCarts);
        }
       
        
        
        
        //public void AddToCart(int productId)//SelectedItemsInCartVM selectedItems)
        //{
        //   Claim idClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
        //    string userid = idClaim.Value;
        //    List<Customer> customers =repo.getall();
        //    Customer customer = customers.FirstOrDefault(c=>c.ApplicationUserId==userid);
        //    Supplier_Product s = supProductRepo.getbyid(productId);
        //    Product p = productRepo.getbyid(s.ProductID);
        //   CustomerSelected_SupplierProduct selected_SupplierProduct = new CustomerSelected_SupplierProduct();
        //    selected_SupplierProduct.CustomerID = customer.ID;
        //    selected_SupplierProduct.Quantity = s.Quantity;
        //    selected_SupplierProduct.TotalPrice = s.Price;
        //    selected_SupplierProduct.Supplier_ProductID = productId;
        //    selectedItemsRepo.create(selected_SupplierProduct);
            
        //}
      
        
        
        public IActionResult Delete(int selectId)
        {
            //List<CustomerSelected_SupplierProduct> s1 = selectedItemsRepo.getall();
            
              CustomerSelected_SupplierProduct s = selectedItemsRepo.getbyid(selectId);
              selectedItemsRepo.delete(s);
              return Json(new { totalprice = selectedItemsRepo.getall().Sum(s => s.TotalPrice), totalquantity = selectedItemsRepo.getall().Sum(s => s.Quantity) });

            //List <CustomerSelected_SupplierProduct> s1 = selectedItemsRepo.getall();
            //return RedirectToAction("ShowCart");
        }
        public IActionResult Increase(int selectId, int quantity, int price)
        {
            CustomerSelected_SupplierProduct s = selectedItemsRepo.getbyid(selectId);
            Supplier_Product supplier_Product = supProductRepo.getbyid(s.Supplier_ProductID);
            s.TotalPrice = quantity * price;
            s.Quantity = quantity;
            supplier_Product.Quantity = quantity;
            selectedItemsRepo.update(s);
            return Json(new { producttotalprice = s.TotalPrice, totalprice = selectedItemsRepo.getall().Sum(s => s.TotalPrice), totalquantity = selectedItemsRepo.getall().Sum(s => s.Quantity) });
        }


        public void Order(int customId)
        {
            List<CustomerSelected_SupplierProduct> selectedItems = selectedItemsRepo.getall().Where(s => s.CustomerID == customId).ToList();

           // Customer customer = repo.getbyid(customId);
            Order order1 = new Order()
            {
                CrediteNumber = "123",
                CustomerID = customId,
                Date = DateTime.Now,
                OrderState = "doesn't token",
                TotalPrice = selectedItems.Sum(t => t.TotalPrice),
                PaidMethod = "Cash",
                //DelivaryID=1
            };
            ordersRepo.create(order1);
            Supplier_Product_Order Supplier_Product_Order;
            foreach (CustomerSelected_SupplierProduct item in selectedItems)
            {
                Supplier_Product_Order = new Supplier_Product_Order();

                Supplier_Product_Order.OrderID = order1.ID;
                Supplier_Product_Order.TotalPrice = item.TotalPrice;
                Supplier_Product_Order.Supplier_ProductID = item.Supplier_ProductID;
                Supplier_Product_Order.Quntity = item.Quantity;

                selectedItemsRepo.delete(item);
                Supplier_Product_OrderRepo.create(Supplier_Product_Order);

               
            }
        } 
         //   return View();
        }
}
