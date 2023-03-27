using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebApplication1.Models;
using WebApplication1.repo;
using WebApplication1.ViewModel;

namespace WebApplication1.Controllers
{
   
    public class DelivaryController : Controller
    {
        private readonly UserManager<ApplicationUser> user;
        Ireposatory<Delivary> delivaryRepository;
        Ireposatory<Customer> customerRepository;
        Ireposatory<Order> orderRepository;

      
        public DelivaryController(Ireposatory<Delivary> _delivaryRepo, Ireposatory<Customer> _customerRepo, UserManager<ApplicationUser>_user,Ireposatory<Order> _orderRepository)
        {
            delivaryRepository = _delivaryRepo;
            customerRepository= _customerRepo;
            user = _user;
            orderRepository = _orderRepository;

        }
     
        [HttpGet]
        public IActionResult New()
        {
            
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> New(DelivaryWithOrderListVM delivaryVM)
        {

                if(ModelState.IsValid == true)
                {
                Claim idClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
                string userid = idClaim.Value;

                Delivary delivaryModel = new Delivary();

                delivaryModel.SSN = delivaryVM.SSN;
                delivaryModel.SSNImageName = delivaryVM.SSNImageName;
                delivaryModel.IsBusy = delivaryVM.IsBusy;
                delivaryModel.AccountNumber = delivaryVM.AccountNumber;
                delivaryModel.ApplicationUserId = userid;
                delivaryRepository.create(delivaryModel);
                return RedirectToAction("ShowProfile",new { userID= userid });
                }
                return View(delivaryVM);
        }
        
        [HttpGet]
        public async Task<IActionResult> ShowProfile(string userid)
        {


            //var claims = User.Claims;
            //Claim idClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            //string userid = idClaim.Value;


            List<Delivary> delivaries = delivaryRepository.getall();
            Delivary delivaryModel = delivaries.FirstOrDefault(d => d.ApplicationUserId == userid);
            ApplicationUser user1 = await user.FindByIdAsync(userid);

            DelivaryWithOrderListVM delivaryVM=new DelivaryWithOrderListVM();
            delivaryVM.ID = delivaryModel.ID;
            delivaryVM.UserName = user1.UserName;
           // delivaryVM.Email = user1.Email;
            //delivaryVM.PhoneNumber = user1.PhoneNumber;
            delivaryVM.Address = user1.Address;
            delivaryVM.SSN = delivaryModel.SSN;
            delivaryVM.SSNImageName= delivaryModel.SSNImageName;
            delivaryVM.AccountNumber = delivaryModel.AccountNumber;
            delivaryVM.IsBusy = delivaryModel.IsBusy;

         
             List<Order> orders = orderRepository.getall().Where(d => d.OrderState == "").ToList();
             delivaryVM.Orders = orders;
            
            return View(delivaryVM);
        }

        [HttpPost]
        public async Task<IActionResult> ShowProfile(DelivaryWithOrderListVM delivaryVM)
        {
           
            if (ModelState.IsValid == true)
            {
                Delivary delivaryModel = delivaryRepository.getbyid(delivaryVM.ID);
                ApplicationUser user1 = await user.FindByIdAsync(delivaryModel.ApplicationUserId);
                
                    user1.UserName = delivaryVM.UserName;
                    user1.Address = delivaryVM.Address;
                    //user1.PhoneNumber = delivaryVM.PhoneNumber;
                    //user1.Email = delivaryVM.Email;
                   
                    delivaryModel.SSN = delivaryVM.SSN;
                    delivaryModel.SSNImageName = delivaryVM.SSNImageName;
                    delivaryModel.IsBusy = delivaryVM.IsBusy;
                    delivaryModel.AccountNumber = delivaryVM.AccountNumber;
                    //delivaryModel.Orders=delivaryVM.Orders;

                    delivaryRepository.update(delivaryModel);
                    await user.UpdateAsync(user1);

                    return RedirectToAction("ShowProfile",new { userID = user1.Id });
                }
            return View(delivaryVM);
        }
        [Authorize]
       public void Checked(int id)
        {

            Claim idClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            string userid = idClaim.Value;
            Delivary delivaries = delivaryRepository.getall().FirstOrDefault(e=>e.ApplicationUserId==userid);
            delivaries.IsBusy = true;
            delivaryRepository.update(delivaries);
            Order order=orderRepository.getbyid(id);
            order.OrderState = "token";
            orderRepository.update(order);

        }
    }
}
