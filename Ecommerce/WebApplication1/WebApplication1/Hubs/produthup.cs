using Microsoft.AspNetCore.SignalR;
using WebApplication1.Models;
using WebApplication1.repo;

namespace WebApplication1.Hubs
{
    public class produthup:Hub
    {
        Ireposatory<Review> commentsRepo;
        Ireposatory<Product> productsRepo;
        Ireposatory<Supplier_Product> Supplier_ProductRepo;


        public produthup(Ireposatory<Review> comments, Ireposatory<Product> products, Ireposatory<Supplier_Product> Supplier_ProductRepo)

        {//Context c
            this.commentsRepo = comments;
            this.productsRepo = products;
            this.Supplier_ProductRepo= Supplier_ProductRepo;
           // commetss = new genaricreposatoryofDEPArTEMENTS<commets>(c);
            //productss = new genaricreposatoryofDEPArTEMENTS<products>(c);

        }

        public override Task OnConnectedAsync()
        {
            return base.OnConnectedAsync();
        }

        public void commentofproducts(int supplierproductid ,string comment,int customerid)
        {
            Review review = new Review();
            review.ReviewText = comment;
            review.CustomerId= customerid; 
            review.Supplier_ProductID = supplierproductid;
            review.Date= DateTime.Now;
            commentsRepo.create(review);
            Clients.All.SendAsync("append_commnets", supplierproductid, comment, customerid);
        }

        public void changeorder(int supplierproductid, int quantity)
        {
            
           
            Supplier_Product supplier_Product = Supplier_ProductRepo.getbyid(supplierproductid);
            supplier_Product.Quantity = supplier_Product.Quantity - quantity;
            Supplier_ProductRepo.update(supplier_Product);
            Clients.All.SendAsync("quantityChanged", supplierproductid, supplier_Product.Quantity);
            //products temp= productss.getbyid(id);
            //if (temp.quantity - neddequtity > 0)
            //{
            //    temp.quantity -= neddequtity;
            //    productss.update(temp);
            //    Clients.All.SendAsync("make_change_of_product_value",temp.quantity,temp.Id);
            //}
            //else
            //{
            //    throw new Exception("wuatity will be < 0");
            //}

        }

    }
}
