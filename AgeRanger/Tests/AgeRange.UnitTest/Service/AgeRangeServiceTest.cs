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
            var exixtedPersonModel = TestingDataDource.ExistingPersonModel;
            exixtedPersonModel.FirstName = exixtedPersonModel.FirstName + " Edited";
            var service = new AgeRanger.Service.Implementation.AgeRangeService(
                this.mockAgeGroupRepository.Object, this.mockPersonRepository.Object);

            //Act
            var returnedModel = service.SavePerson(exixtedPersonModel);

            // Assert
            Assert.IsTrue(returnedModel.FirstName.Equals(exixtedPersonModel.FirstName));
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

        [Ignore]
        [TestMethod]        
        public void TestDeleteExistingPerson()
        {
            throw new NotImplementedException();
        }

        [TestCleanup]
        public void TearDown()
        {
            // Remove resource
        }
    }
}
