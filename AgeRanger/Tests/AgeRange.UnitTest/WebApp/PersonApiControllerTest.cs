using Microsoft.VisualStudio.TestTools.UnitTesting;
using AgeRanger.WebApp.Controllers.Api;
using AgeRanger.Service.Contract.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Web.Http;

namespace AgeRange.UnitTest.WebApp
{
    [TestClass]
    public class PersonApiControllerTest : BaseControllerTest
    {
        [TestMethod]
        public void GetAll_People_Without_Filtering()
        {
            // Agrange
            var controller = new PersonController(this.mockService.Object);
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();

            //Act
            var response = controller.Get(string.Empty);

            // Assert
            Assert.IsTrue(response.TryGetContentValue(out List<PersonModel> people));

            // get all person
            Assert.IsTrue(people.Count == 3);
            var person = people.Find(x => x.Id == 3);
            Assert.IsTrue(person.FirstName == "Toan" && person.AgeGroup == "NOT Human");
        }
    }
}
