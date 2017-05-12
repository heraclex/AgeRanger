using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AgeRanger.WebApp.Controllers.Api;

namespace AgeRange.UnitTest.WebApp
{
    [TestClass]
    public class PersonApiControllerTest : BaseControllerTest
    {
        [TestMethod]
        public void Get_People_Without_Filtering()
        {
            // Agrange
            var personController = new PersonController(this.mockService.Object);

            //Act
            var messageResponse = personController.Get(string.Empty);

            // Assert
            //Assert.IsTrue(messageResponse.Content)

        }
    }
}
