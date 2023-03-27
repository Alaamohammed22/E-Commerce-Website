using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;
using WebApplication1.repo;
using WebApplication1.ViewModel;

namespace WebApplication1.Controllers
{
  //[Authorize(Roles ="Admin")]
    public class RoleController : Controller
    { 


     private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly Ireposatory<Category> CategoryRepo;
      private readonly Ireposatory<Brand> BrandRepo;
        private readonly Ireposatory<Supplier> SupplierRepo;
        private readonly Ireposatory<Customer> CustomerRepo;
        private readonly Ireposatory<Delivary> DelivaryRepo;


        public RoleController(RoleManager<IdentityRole> roleManager, Ireposatory<Category> CategoryRepo,
            Ireposatory<Brand> BrandRepo, Ireposatory<Supplier> SupplierRepo,
            Ireposatory<Customer> CustomerRepo, Ireposatory<Delivary> DelivaryRepo, UserManager<ApplicationUser> userManager)
    {
            this.roleManager = roleManager;
            this.userManager = userManager;
            this.CategoryRepo= CategoryRepo;
            this.BrandRepo= BrandRepo;
            this.SupplierRepo= SupplierRepo;
            this.CustomerRepo= CustomerRepo;
            this.DelivaryRepo= DelivaryRepo;
    }

        public IActionResult RoleIndex()
        {
            List<RoleVM> roles = new List<RoleVM>();
            List<IdentityRole> roles2 = roleManager.Roles.ToList();
            foreach(IdentityRole role in roles2)
            {
                RoleVM roleVM= new RoleVM();
                roleVM.RoleName = role.Name;
                roles.Add(roleVM);
            }
            return View(roles);
        }
        public IActionResult New()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> New(RoleVM roleVM)
    {
        if (ModelState.IsValid)
        {
            IdentityRole role = new IdentityRole();
            role.Name = roleVM.RoleName;
            IdentityResult result = await roleManager.CreateAsync(role);
            if (result.Succeeded)
            {
                return View();
            }
            else
            {
                    foreach (var item in result.Errors)
                    {
                        ModelState.AddModelError("", item.Description);
                    }
            }
        }
        return View(roleVM);
    }



        // *************************Categories***************************************** 
        public IActionResult CategoryIndex()
        {
            return View(CategoryRepo.getall().ToList());
        }

        public IActionResult CategoryNew()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CategoryNew(CategoryVM newCategory)
        {
            if(ModelState.IsValid)
            {
                Category category=new Category();
                category.Name = newCategory.Name;
                CategoryRepo.create(category);
            }
            return View(newCategory);
        }

        public IActionResult CategoryEdit(int id) 
        {
            Category category = CategoryRepo.getbyid(id);
            CategoryVM categoryVM = new CategoryVM();
            categoryVM.ID  = category.ID;
            categoryVM.Name = category.Name;    
            return View(categoryVM);
        }
        [HttpPost]
        public IActionResult CategoryEdit(CategoryVM newCategory)
        {
            if(ModelState.IsValid)
            {
                Category category = CategoryRepo.getbyid(newCategory.ID);
                category.Name = newCategory.Name;
                CategoryRepo.update(category);
            }
            return View(newCategory);
        }

        public void CategoryDelete(int id)
        {
            CategoryRepo.delete(CategoryRepo.getbyid(id));
            
        }





        // *************************Brands***************************************** 
        public IActionResult BrandIndex()
        {
            return View(BrandRepo.getall().ToList());
        }

        public IActionResult BrandNew()
        {
            return View();
        }

        [HttpPost]
        public IActionResult BrandNew(BrandVM newBrand)
        {
            if (ModelState.IsValid)
            {
                Brand Brand = new Brand();
                Brand.Name = newBrand.Name;
                BrandRepo.create(Brand);
            }
            return View(newBrand);
        }

        public IActionResult BrandEdit(int id)
        {
            Brand Brand = BrandRepo.getbyid(id);
            BrandVM BrandVM = new BrandVM();
            BrandVM.ID = Brand.ID;
            BrandVM.Name = Brand.Name;
            return View(BrandVM);
        }
        [HttpPost]
        public IActionResult BrandEdit(BrandVM newBrand)
        {
            if (ModelState.IsValid)
            {
                Brand Brand = BrandRepo.getbyid(newBrand.ID);
                Brand.Name = newBrand.Name;
                BrandRepo.update(Brand);
            }
            return View(newBrand);
        }

        public void BrandDelete(int id)
        {
            BrandRepo.delete(BrandRepo.getbyid(id));

        }




        // *************************Supplier***************************************** 
        public IActionResult SupplierIndex()
        {
            return View(SupplierRepo.getall("ApplicationUser").Where(s=>s.IsDeleted==false).ToList());
        }

        public IActionResult SupplierEdit(int id)
        {
            Supplier Supplier = SupplierRepo.getall("ApplicationUser").FirstOrDefault(s=>s.ID==id);
            UserVM UserVM = new UserVM();
            UserVM.ID = Supplier.ID;
            UserVM.Name = Supplier.ApplicationUser.UserName;
            UserVM.VerifecationState = Supplier.VerifecationState;
            return View(UserVM);
        }
        [HttpPost]
        public IActionResult SupplierEdit(UserVM newSupplier)
        {
            if (ModelState.IsValid)
            {
                Supplier Supplier = SupplierRepo.getall("ApplicationUser").FirstOrDefault(s => s.ID == newSupplier.ID);
                Supplier.VerifecationState = newSupplier.VerifecationState;
                SupplierRepo.update(Supplier);
            }
            return View(newSupplier);
        }

        public async void SupplierDelete(int id)
        {
            Supplier Supplier = SupplierRepo.getall("ApplicationUser").FirstOrDefault(s => s.ID == id);
            ApplicationUser applicationUser =await userManager.FindByIdAsync(Supplier.ApplicationUserId);
            Supplier.IsDeleted = true;
            applicationUser.IsDeleted = true;
            SupplierRepo.update(Supplier);
            userManager.UpdateAsync(applicationUser);
         }





        // *************************Customer***************************************** 
        public IActionResult CustomerIndex()
        {
            return View(CustomerRepo.getall("ApplicationUser").Where(s => s.IsDeleted == false).ToList());
        }

        public async void CustomerDelete(int id)
        {
            Customer customer = CustomerRepo.getall("ApplicationUser").FirstOrDefault(s => s.ID == id);
            ApplicationUser applicationUser = await userManager.FindByIdAsync(customer.ApplicationUserId);
            customer.IsDeleted = true;
            applicationUser.IsDeleted = true;
            CustomerRepo.update(customer);
            userManager.UpdateAsync(applicationUser);
            //Customer customer = CustomerRepo.getall("ApplicationUser").FirstOrDefault(s => s.ID == id);
            //ApplicationUser applicationUser = customer.ApplicationUser;
            //CustomerRepo.delete(customer);
            //await userManager.DeleteAsync(applicationUser);
        }





        // *************************Delivary***************************************** 
        public IActionResult DelivaryIndex()
        {
            return View(DelivaryRepo.getall("ApplicationUser").Where(s => s.IsDeleted == false).ToList());
        }

        public async void DelivaryDelete(int id)
        {
            Delivary delivary = DelivaryRepo.getall("ApplicationUser").FirstOrDefault(s => s.ID == id);
            ApplicationUser applicationUser = await userManager.FindByIdAsync(delivary.ApplicationUserId);
            delivary.IsDeleted = true;
            applicationUser.IsDeleted = true;
            DelivaryRepo.update(delivary);
            userManager.UpdateAsync(applicationUser);
        }



    }
}
