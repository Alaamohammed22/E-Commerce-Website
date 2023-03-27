using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol.Core.Types;
using System.Security.Claims;
using WebApplication1.Helper;
using WebApplication1.Models;
using WebApplication1.repo;
using WebApplication1.ViewModel;

namespace WebApplication1.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        Ireposatory<Delivary> delivaryRepo;
        private Ireposatory<Supplier> IrepSupplier;
        public Ireposatory<Customer> reposatory;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager, Ireposatory<Delivary> _delivaryRepo, Ireposatory<Supplier> IrepSupplier,Ireposatory<Customer> reposatory )
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.roleManager = roleManager;
            delivaryRepo= _delivaryRepo;
            this.IrepSupplier = IrepSupplier;
            this.reposatory= reposatory;
        }


        public IActionResult Registration()
        {
            RegistrationVM temp = new RegistrationVM();
            temp.IdentityRoleS = roleManager.Roles.Where(R => R.Name != "Admin").ToList();
            return View(temp);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Registration(RegistrationVM newUser)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = new ApplicationUser();
                user.UserName = newUser.UserName;
                user.PasswordHash = newUser.Password;
                user.Address = newUser.Address;
                user.ImageName = ImagesHelper.UploadImg(newUser.ImageFile, "ProfileIMG"); 
                user.IsDeleted= false;

                IdentityResult result = await userManager.CreateAsync(user, newUser.Password);
                if (result.Succeeded)
                {
                   // await userManager.AddToRoleAsync(user, "Admin");
                   await userManager.AddToRoleAsync(user, newUser.RoleName);
                    await signInManager.SignInAsync(user, false);

                    if (newUser.RoleName == "Admin")
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    else if (newUser.RoleName == "Customer")
                    {

                        Customer customer = new Customer();
                        //ApplicationUser applicationUser= await userManager.FindByNameAsync(newUser.UserName);
                        customer.ApplicationUserId = user.Id;//applicationUser.Id;
                        reposatory.create(customer);
                        return RedirectToAction("CustomerProfile", "Customer");
                    }
                    else if (newUser.RoleName == "Supplier")
                    {
                        //Supplier supplier = new Supplier();
                        //supplier.ApplicationUserId = user.Id;
                        //supplier.SSN = "555555555555555";
                        //supplier.SSNImageName = "1.jpg";
                        //supplier.AccountNumber = "555555555555555555";
                        //supplier.VerifecationState = "Binding";
                        //supplier.TotalSales = 5000;
                        //IrepSupplier.create(supplier);
                        return RedirectToAction("New", "Supplier");
                        //return RedirectToAction("Index", "SupplierAction" , new { userid=user.Id});
                    }
                    else // Delivery
                    {
                        return RedirectToAction("New", "Delivary");
                    }
                }
                else
                {
                    foreach (var item in result.Errors)
                    {
                        ModelState.AddModelError("", item.Description);
                    }
                }
            }
            newUser.IdentityRoleS = roleManager.Roles.Where(R => R.Name != "Admin").ToList();
            return View(newUser);
        }


        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginVM userVM)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = await userManager.FindByNameAsync(userVM.UserName);
                if (user != null && user.IsDeleted==false)
                {
                    Microsoft.AspNetCore.Identity.SignInResult result = await signInManager.PasswordSignInAsync(user, userVM.Password, userVM.RememberMe, false);
                    if (result.Succeeded)
                    {
                        
                        //  return RedirectToAction("Index", "Home");
                      List<string> roles= (List<string>)await userManager.GetRolesAsync(user);
                      string role=  roles.FirstOrDefault();
                        if (role == "Admin")
                        {
                            return RedirectToAction("Index", "Home");
                        }
                        else if (role == "Customer")
                        {
                            List<Customer> customerList = reposatory.getall();
                            Customer c = customerList.FirstOrDefault(c => c.ApplicationUserId == user.Id);
                            //return RedirectToAction("Index", "Customer", new { id = c.ID });
                            return RedirectToAction("Index", "Products");
                        }
                        else if (role == "Supplier")
                        {
                            return RedirectToAction("Edit", "Supplier");
                        }
                        else // Delivary
                        {
                            return RedirectToAction("ShowProfile", "Delivary");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "Wrong Password");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Wrong Data (UserNAme Does not Exist)");
                }
            }
            return View(userVM);
        }


        public async Task<IActionResult> signOut()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Login");
        }

        public IActionResult ContactUs()
        {
            return View();
        }

    }

}
