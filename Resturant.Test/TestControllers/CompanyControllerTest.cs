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
    class CompanyControllerTest
    {
        private readonly Mock<ICompanyRepository> _companyRepositoryMock = new Mock<ICompanyRepository>();
        private readonly CompanyController _companyController;

        public CompanyControllerTest()
        {
            _companyController = new CompanyController(_companyRepositoryMock.Object);
        }

        [Test]
        public async Task Get_FetchAllCompany()
        {

            var company = await _companyController.Get();

            Assert.NotNull(company);

            var foundResult = company as OkObjectResult;
            Assert.NotNull(foundResult);
            Assert.AreEqual(200, foundResult.StatusCode);
        }

        [Test]
        public async Task Get_FetchCompany()
        {
            int companyid = 1;
            var company = await _companyController.GetById(companyid);

            Assert.NotNull(company);

            var foundResult = company as OkObjectResult;
            Assert.NotNull(foundResult);
            Assert.AreEqual(200, foundResult.StatusCode);
        }

        [Test]
        public async Task Post_InsertNewCompany()
        {
            var newCompany = new Company
            {
                Name = "Harmony Technology",
                Postal = "123 Accra Ghana",
                Mobile = "0238288675",
                Address = "NO 24 Odonkor street, new town, Accra.",
                Date = DateTime.UtcNow,
                UserId = "807ba6c0-e845-4695-847e-92edca9d66db"
            };

            var company = await _companyController.Post(newCompany);

            Assert.NotNull(company);

            var createdResult = company as CreatedResult;
            Assert.NotNull(createdResult);
            Assert.AreEqual(201, createdResult.StatusCode);
        }

        [Test]
        public async Task Put_UpdateCompany()
        {
            var com = new Company
            {
                CompanyId = 1,
                Name = "Harmony Technology",
                Postal = "123 Accra Ghana",
                Mobile = "0238288675",
                Address = "NO 24 Odonkor street, new town, Accra.",
                Date = DateTime.UtcNow,
                UserId = "807ba6c0-e845-4695-847e-92edca9d66db",
                MDate = DateTime.UtcNow,
                MUserId = "807ba6c0-e845-4695-847e-92edca9d66db"
            };

            var company = await _companyController.Put(com, com.CompanyId);

            Assert.NotNull(company);

            var Result = company as OkObjectResult;
            Assert.NotNull(Result);
            Assert.AreEqual(200, Result.StatusCode);
        }
    }
}
