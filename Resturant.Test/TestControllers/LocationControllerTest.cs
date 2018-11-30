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
    class LocationControllerTest
    {
        private readonly Mock<ILocationRepository> _locationRepositoryMock = new Mock<ILocationRepository>();
        private readonly LocationController _locationController;

        public LocationControllerTest()
        {
            _locationController = new LocationController(_locationRepositoryMock.Object);
        }

        [Test]
        public async Task Get_FetchAllLocation()
        {

            var location = await _locationController.Get();

            Assert.NotNull(location);

            var foundResult = location as OkObjectResult;
            Assert.NotNull(foundResult);
            Assert.AreEqual(200, foundResult.StatusCode);
        }

        [Test]
        public async Task Get_FetchLocation()
        {
            int locationid = 1;
            var location = await _locationController.GetById(locationid);

            Assert.NotNull(location);

            var foundResult = location as OkObjectResult;
            Assert.NotNull(foundResult);
            Assert.AreEqual(200, foundResult.StatusCode);
        }

        [Test]
        public async Task Post_InsertNewLocation()
        {
            var newLocation = new Location
            {
                Name = "Accra", Discount = 10,
                Date = DateTime.UtcNow,
                UserId = "807ba6c0-e845-4695-847e-92edca9d66db"
            };

            var location = await _locationController.Post(newLocation);

            Assert.NotNull(location);

            var createdResult = location as CreatedResult;
            Assert.NotNull(createdResult);
            Assert.AreEqual(201, createdResult.StatusCode);
        }

        [Test]
        public async Task Put_UpdateLocation()
        {
            var loc = new Location
            {
                LocationId = 1,
                Name = "Accra", Discount = 10,
                Date = DateTime.UtcNow,
                UserId = "807ba6c0-e845-4695-847e-92edca9d66db",
                MDate = DateTime.UtcNow,
                MUserId = "807ba6c0-e845-4695-847e-92edca9d66db"
            };

            var location = await _locationController.Put(loc, loc.LocationId);

            Assert.NotNull(location);

            var Result = location as OkObjectResult;
            Assert.NotNull(Result);
            Assert.AreEqual(200, Result.StatusCode);
        }
    }
}
