using Microsoft.AspNetCore.SignalR;
using WebApplication1.Models;
using WebApplication1.repo;

namespace WebApplication1.Hubs
{
    public class ReviewNotification : Hub
    {
        //Context context = new Context();
        Ireposatory<Review> ReviewRepo;

        public ReviewNotification(Ireposatory<Review> ReviewRepo)
        {
            this.ReviewRepo = ReviewRepo;
        }
        public void NewComment(int Supplier_ProductID, string text, int Customer)
        {
            Review review = new Review
            {
                Supplier_ProductID = Supplier_ProductID,
                ReviewText = text,
                 CustomerId = Customer
            };
            ReviewRepo.create(review);
           // context.Reviews.Add(review);
            //context.SaveChanges();
            Clients.All.SendAsync("NewCommentNotify", Supplier_ProductID, text, Customer);
        }
        public override Task OnConnectedAsync()
        {
            return base.OnConnectedAsync();
        }
    }
}
