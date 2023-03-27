using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using WebApplication1.Hubs;
using WebApplication1.Models;
using WebApplication1.repo;

namespace WebApplication1
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            // for json
            builder.Services.AddControllers().AddJsonOptions(x =>
                x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

            // Add services to the container.DAy 8 injection
            builder.Services.AddDbContext<Context>(option =>
            {
                option.UseSqlServer(builder.Configuration.GetConnectionString("CS"));

            });

            builder.Services.AddScoped<Ireposatory<Customer>, Reposatory<Customer>>();
            builder.Services.AddScoped<Ireposatory<Delivary>, Reposatory<Delivary>>();
            builder.Services.AddScoped<Ireposatory<Supplier>, Reposatory<Supplier>>();
            builder.Services.AddScoped<Ireposatory<Brand>, Reposatory<Brand>>();
            builder.Services.AddScoped<Ireposatory<CustomerSelected_SupplierProduct>, Reposatory<CustomerSelected_SupplierProduct>>();
            builder.Services.AddScoped<Ireposatory<Category>, Reposatory<Category>>();
            builder.Services.AddScoped<Ireposatory<Category_Brand>, Reposatory<Category_Brand>>();
            builder.Services.AddScoped<Ireposatory<Offer>, Reposatory<Offer>>();
            builder.Services.AddScoped<Ireposatory<Order>, Reposatory<Order>>();
            builder.Services.AddScoped<Ireposatory<Product>, Reposatory<Product>>();
            builder.Services.AddScoped<Ireposatory<Review>, Reposatory<Review>>();
            builder.Services.AddScoped<Ireposatory<Supplier_Product>, Reposatory<Supplier_Product>>();
            builder.Services.AddScoped<Ireposatory<Supplier_Product_Order>, Reposatory<Supplier_Product_Order>>();

            //*********************************
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(
                options => {
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireUppercase = false;
                    options.Password.RequireDigit = false;  
                }
                ).AddEntityFrameworkStores<Context>();

            // Add services to the container.

            //
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
            });

            builder.Services.AddSignalR();

            var app = builder.Build();
            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            
            app.MapHub<CommentHub>("/Comment");
            
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");


            //app.MapControllerRoute(
            //    name: "Admin",
            //    pattern: "Admin/{controller=Role}/{action=New}");

            app.Run();
        }
    }
}