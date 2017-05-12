using AgeRanger.DbContext.Entities;
using AgeRanger.Service.Contract;
using AgeRanger.Service.Contract.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgeRange.UnitTest.WebApp
{

        [TestClass]
    public abstract class BaseControllerTest
    {
        protected Mock<IService> mockService;

        [TestInitialize]
        public virtual void Setup()
        {
            this.mockService = new Mock<IService>();
            
            mockService.Setup(m => m.GetPeople(string.Empty)).Returns(new List<PersonModel>()
            {
                new PersonModel() { Id = 1, FirstName = "David", LastName = "Cantona", Age = 29, AgeGroup = "Human"},
                new PersonModel() { Id = 2, FirstName = "Eric", LastName = "Lee", Age = 139, AgeGroup = "Ancient"},
                new PersonModel() { Id = 3, FirstName = "Toan", LastName = "Le", Age = 999, AgeGroup = "NOT Human"}
            });
            
            mockService.Setup(m => m.SavePerson(It.IsAny<PersonModel>()))
                .Returns((PersonModel m) => {                    
                    return m;
                });
        }
    }
}
