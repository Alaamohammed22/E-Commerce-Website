using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
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
        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager, Ireposatory<Delivary> _delivaryRepo)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.roleManager = roleManager;
            delivaryRepo= _delivaryRepo;
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
                user.ImageName = newUser.ImageName;


                IdentityResult result = await userManager.CreateAsync(user, newUser.Password);//user.PasswordHash
                if (result.Succeeded)
                {
                   //await userManager.AddToRoleAsync(user, "Admin");
                    await userManager.AddToRoleAsync(user, newUser.RoleName);
                    await signInManager.SignInAsync(user, false);

                    if (newUser.RoleName == "Admin")
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    else if (newUser.RoleName == "Customer")
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    else if (newUser.RoleName == "Supplier")
                    {
                        return RedirectToAction("Index", "Home");
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
                if (user != null)
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
                            return RedirectToAction("Index", "Home");
                        }
                        else if (role == "Supplier")
                        {
                            return RedirectToAction("Index", "Home");
                        }
                        else // Delivary
                        {
                            return RedirectToAction("ShowProfile", "Delivary", new { userID = user.Id });
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

    }

}
