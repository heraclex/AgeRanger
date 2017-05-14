using AgeRange.UnitTest.Helper;
using AgeRanger.DbContext.Entities;
using AgeRanger.Repository;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AgeRange.UnitTest.Service
{
    [TestClass]
    public class AgeRangeServiceTest : BaseServiceTest
    {
        [TestMethod]
        public void TestGetAllPerson()
        {
            //Arrange            
            var service = new AgeRanger.Service.Implementation.AgeRangeService(
                this.mockAgeGroupRepository.Object, this.mockPersonRepository.Object);

            // Act
            var result = service.FindPeople(string.Empty);

            //Assert
            Assert.IsTrue(result.Count() == 4);
            var person = result.First(x => x.Id == 4);
            Assert.IsTrue(person.FirstName == "Toan" && person.AgeGroup == "Vampire");
        }

        [TestMethod]
        public void TestFindPersonWithExistingName()
        {
            // Arrange
            var service = new AgeRanger.Service.Implementation.AgeRangeService(
                this.mockAgeGroupRepository.Object, this.mockPersonRepository.Object);

            // Act
            var result = service.FindPeople("tona");

            // Assert
            Assert.IsTrue(result.Count() == 2);
        }
        
        [TestMethod]
        public void TestAddingNewPerson()
        {
            // Agrange
            var newPersonModel = TestingDataDource.ValidNewPersonModel;
            var service = new AgeRanger.Service.Implementation.AgeRangeService(
                this.mockAgeGroupRepository.Object, this.mockPersonRepository.Object);

            //Act
            var returnedModel = service.SavePerson(newPersonModel);

            // Assert
            Assert.IsTrue(returnedModel.Id > 0 && newPersonModel.Id == 0);
        }

        [TestMethod]
        public void TestEditExistingPerson()
        {
            // Agrange
            var existingPersonModel = TestingDataDource.ExistingPersonModel;
            existingPersonModel.FirstName = existingPersonModel.FirstName + " Edited";
            var service = new AgeRanger.Service.Implementation.AgeRangeService(
                this.mockAgeGroupRepository.Object, this.mockPersonRepository.Object);

            //Act
            var returnedModel = service.SavePerson(existingPersonModel);

            // Assert
            Assert.IsTrue(returnedModel.FirstName.Equals(existingPersonModel.FirstName));
        }

        [TestMethod]
        public void TestGetSpecificPersonWithInvalidName()
        {
            // Arrange
            var service = new AgeRanger.Service.Implementation.AgeRangeService(
                this.mockAgeGroupRepository.Object, this.mockPersonRepository.Object);

            // Act
            var result = service.FindPeople("tonaXXX");

            // Assert
            Assert.IsTrue(result.Count() == 0);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "Verify the Delete Method on Repo was called")]
        public void TestDeleteExistingPerson()
        {
            // Arrange
            var exixtedPersonModel = TestingDataDource.ExistingPersonModel;
            var service = new AgeRanger.Service.Implementation.AgeRangeService(
                this.mockAgeGroupRepository.Object, this.mockPersonRepository.Object);
            service.DeletePersonById(exixtedPersonModel.Id);
        }

        [TestCleanup]
        public void TearDown()
        {
            // Remove resource
        }
    }
}
