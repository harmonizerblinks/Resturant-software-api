using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using Resturant.Controllers;
using Resturant.Models;
using Resturant.Repository;
using System;
using System.Threading.Tasks;

namespace Resturant.Test.TestControllers
{
    class DiscountControllerTest
    {
        private readonly Mock<IDiscountRepository> _discountRepositoryMock = new Mock<IDiscountRepository>();
        private readonly DiscountController _discountController;

        public DiscountControllerTest()
        {
            _discountController = new DiscountController(_discountRepositoryMock.Object);
        }

        [Test]
        public async Task Get_FetchAllDiscount()
        {

            var discount = await _discountController.Get();

            Assert.NotNull(discount);

            var foundResult = discount as OkObjectResult;
            Assert.NotNull(foundResult);
            Assert.AreEqual(200, foundResult.StatusCode);
        }

        [Test]
        public async Task Get_FetchDiscount()
        {
            int discountid = 1;
            var discount = await _discountController.GetById(discountid);

            Assert.NotNull(discount);

            var foundResult = discount as OkObjectResult;
            Assert.NotNull(foundResult);
            Assert.AreEqual(200, foundResult.StatusCode);
        }

        [Test]
        public async Task Post_InsertNewDiscount()
        {
            var newDiscount = new Discount
            {
                Name = "Harmony Alabi",
                Mobile = "0238288675",
                Percent = 5, LocationId = 1,
                Date = DateTime.UtcNow,
                UserId = "807ba6c0-e845-4695-847e-92edca9d66db"
            };

            var discount = await _discountController.Post(newDiscount);

            Assert.NotNull(discount);

            var createdResult = discount as CreatedResult;
            Assert.NotNull(createdResult);
            Assert.AreEqual(201, createdResult.StatusCode);
        }

        [Test]
        public async Task Put_UpdateDiscount()
        {
            var dis = new Discount
            {
                DiscountId = 1,
                Name = "Harmony Alabi",
                Mobile = "0238288675", Percent = 5,
                LocationId = 1, Date = DateTime.UtcNow,
                UserId = "807ba6c0-e845-4695-847e-92edca9d66db",
                MDate = DateTime.UtcNow,
                MUserId = "807ba6c0-e845-4695-847e-92edca9d66db"
            };

            var discount = await _discountController.Put(dis, dis.DiscountId);

            Assert.NotNull(discount);

            var Result = discount as OkObjectResult;
            Assert.NotNull(Result);
            Assert.AreEqual(200, Result.StatusCode);
        }
    }
}
