using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using WebApplication1.Controllers;
using WebApplication1.Models;
using WebApplication1.repo;
using static Azure.Core.HttpHeader;

namespace ProjectTest
{
    [TestFixture]
    public class MyTests
    {
        private  readonly UserManager<ApplicationUser> user;
        private Context context;
        Ireposatory<Delivary> delreposatory;
     
        //public MyTests(UserManager<ApplicationUser> _user)
        //{
        //    user= _user;
        //}

        [SetUp]
        public void SetUp()
        {
            var options = new DbContextOptionsBuilder<Context>()
                 .UseInMemoryDatabase(databaseName: "E_CommerceSystem")
                 .Options;
            context = new Context(options);
            delreposatory = new Reposatory<Delivary>(context);

            var delivaries = new List<Delivary>
            {
                new Delivary { ID = 1, SSN = "251534",SSNImageName="1.jpg",IsBusy=false,AccountNumber="123",
                    ApplicationUserId="23aca093-642a-441e-92f8-e543ebe411a8"},
                new Delivary { ID = 2, SSN = "22323",SSNImageName="2.jpg",IsBusy=false,AccountNumber="456",
                ApplicationUserId="d614754f-b6df-496e-88f3-916862a9b490"},
                new Delivary { ID = 3, SSN = "14432",SSNImageName="3.jpg",IsBusy=false,AccountNumber="789"}
                   
            };

            context.Delivaries.AddRange(delivaries);
            context.SaveChanges();
        }
        [TearDown]
        public void TearDown()
        {
            context.Dispose();
        }

        [Test]
        public void GetAll_test()
        {
            var result = delreposatory.getall();
            Assert.That(result,Is.Not.Null);
            
        }
        [Test]
        public void GetById_test()
        {
            int id = 2;
            var result = delreposatory.getbyid(id);
            Assert.IsNotNull(result);
            
        }
        [Test]
        public void GetByIdWithAreEqual_test()
        {
            int id = 2;
            var result = delreposatory.getbyid(id);
            Assert.AreEqual("2.jpg", result.SSNImageName);

        }
        [Test]
        public void SSNContainNumber2_test()
        {
            Delivary del = delreposatory.getbyid(1);
            Assert.That(del.SSN.Contains('3'));
        }
        [Test]
        public void LengthOfDelivaryList_test()
        {
            List<Delivary> delivaries = delreposatory.getall();
            Assert.AreEqual(3, delivaries.Count);
        }
         
    }
}
