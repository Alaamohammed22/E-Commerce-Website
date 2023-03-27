using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using WebApplication1.Models;
using WebApplication1.repo;
using WebApplication1.ViewModel;
using WebApplication1.Helper;
using System.Security;

namespace WebApplication1.Controllers
{
    public class SupplierController : Controller
    {
        Ireposatory<Supplier> SupplierReposatory;
       

        public SupplierController
            (Ireposatory<Supplier> SupplierReposatory)//ask inject 
        {
            this.SupplierReposatory = SupplierReposatory;
           
        }
        //public IActionResult Index()
        //{
        //    return View();
        //}

        //public IActionResult GetAll()
        //{

        //    List<Supplier> suppliers = SupplierReposatory.getall();
        //    ViewData["msg"] = "Instructore has been created successfully";
        //    return View("Index", suppliers);
        //    //View =showAll ,Model =productListModel  type "List<Product>"
        //}

        public IActionResult New()
        {
            return View();
        } 

        [HttpPost]
        public IActionResult Insert(SupplierVM supplierVM)
        {

            if (ModelState.IsValid) //********** ModelState
            {
                var claims = User.Claims;//keys cookie (NameIdentifier,Name
                Claim idClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
                string userid = idClaim.Value;
                Supplier newSupplier = new Supplier
                {
                    AccountNumber=supplierVM.AccountNumber,
                    SSN= supplierVM.SSN,
                    SSNImageName= ImagesHelper.UploadImg(supplierVM.SSNImageFile, "SSNIMG"),
                    TotalSales = (double)supplierVM.TotalSales,
                    VerifecationState = supplierVM.VerifecationState,
                    ApplicationUserId=userid
                };
                SupplierReposatory.create(newSupplier);
                TempData["msg"] = "Supplier has been created successfully";
                //ViewData["msg"] = "Instructore has been created successfully";
                //return RedirectToAction("GetAll");
                return RedirectToAction("Edit");
                //return RedirectToAction("DEtails", "Employee", new { id=1   ,name="asfkdfj"});
            }
            return View("New", supplierVM);
        }




        public IActionResult Edit()
        {
            //old 
            Claim idClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            string userid = idClaim.Value;
            Supplier supplier = SupplierReposatory.getall("ApplicationUser").FirstOrDefault(s=>s.ApplicationUserId==userid);
            SupplierVM supplierVM = new SupplierVM
            {
                UserName = supplier.ApplicationUser.UserName,
                Email= supplier.ApplicationUser.Email,
                PhoneNumber= supplier.ApplicationUser.PhoneNumber,
                Image= supplier.ApplicationUser.ImageName,
                Address= supplier.ApplicationUser.Address,
                AccountNumber = supplier.AccountNumber,
                SSN = supplier.SSN,
                SSNImageName = supplier.SSNImageName,
                VerifecationState=supplier.VerifecationState,
                TotalSales=supplier.TotalSales
            };
            return View(supplierVM);
        }


        [HttpPost]
        public IActionResult Update(SupplierVM newSupplier)
        {

            if (ModelState.IsValid)
            {
                var claims = User.Claims;//keys cookie (NameIdentifier,Name
                Claim idClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
                Supplier oldSupplier = SupplierReposatory.getall("ApplicationUser").FirstOrDefault(s=>s.ApplicationUserId==idClaim.Value);
                oldSupplier.ApplicationUser.UserName = newSupplier.UserName;
                oldSupplier.ApplicationUser.Email = newSupplier.Email;
                oldSupplier.ApplicationUser.PhoneNumber = newSupplier.PhoneNumber;
                oldSupplier.ApplicationUser.Address = newSupplier.Address;
                oldSupplier.ApplicationUser.ImageName = ImagesHelper.UploadImg(newSupplier.ImageFile, "ProfileIMG");
                oldSupplier.SSN = newSupplier.SSN;
                oldSupplier.AccountNumber= newSupplier.AccountNumber;
                oldSupplier.SSNImageName = ImagesHelper.UploadImg(newSupplier.SSNImageFile,"SSNIMG");
                SupplierReposatory.update(oldSupplier);
                TempData["update"] = "Supplier has been updated successfully";

                return RedirectToAction("Edit");
            }
            return RedirectToAction("Edit");
        }

        //public IActionResult Delete(int id)
        //{
        //    Supplier supplier = SupplierReposatory.getbyid(id);
        //    SupplierReposatory.delete(supplier);
        //    return RedirectToAction("GetAll");
        //}
    }   
}
