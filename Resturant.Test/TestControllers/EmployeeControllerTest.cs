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
    class EmployeeControllerTest
    {
        private readonly Mock<IEmployeeRepository> _employeeRepositoryMock = new Mock<IEmployeeRepository>();
        private readonly EmployeeController _employeeController;

        public EmployeeControllerTest()
        {
            _employeeController = new EmployeeController(_employeeRepositoryMock.Object);
        }

        [Test]
        public async Task Get_FetchAllEmployee()
        {

            var employee = await _employeeController.Get();

            Assert.NotNull(employee);

            var foundResult = employee as OkObjectResult;
            Assert.NotNull(foundResult);
            Assert.AreEqual(200, foundResult.StatusCode);
        }

        [Test]
        public async Task Get_FetchEmployee()
        {
            int employeeid = 1;
            var employee = await _employeeController.GetById(employeeid);

            Assert.NotNull(employee);

            var foundResult = employee as OkObjectResult;
            Assert.NotNull(foundResult);
            Assert.AreEqual(200, foundResult.StatusCode);
        }

        [Test]
        public async Task Post_InsertNewEmployee()
        {
            var newEmployee = new Employee
            {
                FullName = "Harmony Alabi", Mobile = "0238288675",
                Email = "harmonizerblinks@gmail.com", Position = "Admin",
                Image = "harmony.jpg", Nationality = "Nigerian",
                DateOfBirth = DateTime.Now.AddYears(1994),
                Address = "No 24 Odonkor Street, New Town Accra",
                Date = DateTime.UtcNow,
                UserId = "807ba6c0-e845-4695-847e-92edca9d66db"
            };

            var employee = await _employeeController.Post(newEmployee);

            Assert.NotNull(employee);

            var createdResult = employee as CreatedResult;
            Assert.NotNull(createdResult);
            Assert.AreEqual(201, createdResult.StatusCode);
        }

        [Test]
        public async Task Put_UpdateEmployee()
        {
            var emp = new Employee
            {
                EmployeeId = 1,
                FullName = "Harmony Alabi",
                Mobile = "0238288675", Email = "harmonizerblinks@gmail.com",
                Position = "Admin", Image = "harmony.jpg",
                Nationality = "Nigerian", DateOfBirth = DateTime.Now.AddYears(1994),
                Address = "No 24 Odonkor Street, New Town Accra",
                Date = DateTime.UtcNow,
                UserId = "807ba6c0-e845-4695-847e-92edca9d66db",
                MDate = DateTime.UtcNow,
                MUserId = "807ba6c0-e845-4695-847e-92edca9d66db"
            };

            var employee = await _employeeController.Put(emp, emp.EmployeeId);

            Assert.NotNull(employee);

            var Result = employee as OkObjectResult;
            Assert.NotNull(Result);
            Assert.AreEqual(200, Result.StatusCode);
        }
    }
}
