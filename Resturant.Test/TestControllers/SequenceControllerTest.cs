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
    class SequenceControllerTest
    {
        private readonly Mock<ISequenceRepository> _sequenceRepositoryMock = new Mock<ISequenceRepository>();
        private readonly SequenceController _sequenceController;

        public SequenceControllerTest()
        {
            _sequenceController = new SequenceController(_sequenceRepositoryMock.Object);
        }

        [Test]
        public async Task Get_FetchAllSequence()
        {

            var sequence = await _sequenceController.Get();

            Assert.NotNull(sequence);

            var foundResult = sequence as OkObjectResult;
            Assert.NotNull(foundResult);
            Assert.AreEqual(200, foundResult.StatusCode);
        }

        [Test]
        public async Task Get_FetchSequence()
        {
            int sequenceid = 1;
            var sequence = await _sequenceController.GetById(sequenceid);

            Assert.NotNull(sequence);

            var foundResult = sequence as OkObjectResult;
            Assert.NotNull(foundResult);
            Assert.AreEqual(200, foundResult.StatusCode);
        }
        
        [Test]
        public async Task Post_InsertNewSequence()
        {
            var newSequence = new Sequence
            {
                Name = "Order",
                Prefix = "ORD",
                Counter = 100,
                Length = 4,
                Date = DateTime.UtcNow,
                UserId = "807ba6c0-e845-4695-847e-92edca9d66db"
            };

            var sequence = await _sequenceController.Post(newSequence);

            Assert.NotNull(sequence);

            var createdResult = sequence as CreatedResult;
            Assert.NotNull(createdResult);
            Assert.AreEqual(201, createdResult.StatusCode);
        }

        [Test]
        public async Task Put_UpdateSequence()
        {
            var seq = new Sequence
            {
                SequenceId = 1, Name = "Order", Prefix = "ORD",
                Counter = 100, Length = 4, Date = DateTime.UtcNow,
                UserId = "807ba6c0-e845-4695-847e-92edca9d66db",
                MDate = DateTime.UtcNow,
                MUserId = "807ba6c0-e845-4695-847e-92edca9d66db"
            };

            var sequence = await _sequenceController.Put(seq, seq.SequenceId);

            Assert.NotNull(sequence);

            var Result = sequence as OkObjectResult;
            Assert.NotNull(Result);
            Assert.AreEqual(200, Result.StatusCode);
        }

    }
}
