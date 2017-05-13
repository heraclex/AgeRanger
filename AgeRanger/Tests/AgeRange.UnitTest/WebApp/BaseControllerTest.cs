using AgeRange.UnitTest.Helper;
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
        protected Mock<IAgeRangeService> mockService;

        [TestInitialize]
        public virtual void Setup()
        {
            this.mockService = new Mock<IAgeRangeService>();

            var people = TestingDataDource.PersonModelList;
            
            // Mock FindPeople Method on Service
            mockService.Setup(m => m.FindPeople(It.IsAny<string>())).Returns((string filterCriteria) => {
                if (string.IsNullOrEmpty(filterCriteria))
                {
                    return people;
                }

                return people.Where(x => x.FirstName.Contains(filterCriteria) 
                || x.LastName.Contains(filterCriteria)).ToList();
            });

            // Mock SavePerson Method on Service
            mockService.Setup(m => m.SavePerson(It.IsAny<PersonModel>())).Returns((PersonModel m) => 
            {
                if(m.Id == 0)
                    m.Id = 999;
                return m;
            });

            // Mock SavePerson Method on Service
            mockService.Setup(m => m.IsPersonExisted(It.IsAny<long>())).Returns((long id) =>
            {
                return TestingDataDource.PersonList.Any(x=>x.Id == id);
            });

            // Mock GetPerson By Id Method on Service
            mockService.Setup(m => m.GetPersonById(It.IsAny<long>())).Returns((long id) =>
            {
                return TestingDataDource.PersonModelList.FirstOrDefault(x => x.Id == id);
            });
        }
    }
}

