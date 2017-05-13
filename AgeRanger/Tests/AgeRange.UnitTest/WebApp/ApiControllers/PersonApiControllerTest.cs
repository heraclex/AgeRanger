using Microsoft.VisualStudio.TestTools.UnitTesting;
using AgeRanger.WebApp.Controllers.Api;
using AgeRanger.Service.Contract.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Web.Http;
using AgeRange.UnitTest.Helper;
using System.Web.Http.ModelBinding;
using System.ComponentModel.DataAnnotations;
using System;
using System.Web.Http.Hosting;

namespace AgeRange.UnitTest.WebApp.ApiControllers
{
    [TestClass]
    public class PersonApiControllerTest : BaseControllerTest
    {
        [TestMethod]
        public void TestGetAllPeopleWithoutFiltering()
        {
            // Agrange
            var controller = new PersonController(this.mockService.Object);
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();

            //Act
            var response = controller.Filter(string.Empty);

            // Assert
            Assert.IsTrue(response.TryGetContentValue(out List<PersonModel> people));
            
            Assert.IsTrue(people.Count == 4);
            var person = people.Find(x => x.Id == 4);
            Assert.IsTrue(person.FirstName == "Toan" && person.AgeGroup == "Vampire");
        }

        /// <summary>
        /// Assume when user inputs first/last name, the query should search on both columns
        /// </summary>
        [TestMethod]
        public void TestFilteringPeopleWithLastNameOrFirstName()
        {
            // Agrange
            var controller = new PersonController(this.mockService.Object);
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();

            //Act
            var response = controller.Filter("tona");

            // Assert
            Assert.IsTrue(response.TryGetContentValue(out List<PersonModel> people));
            Assert.IsTrue(people.Count == 2);
            Assert.IsTrue(people.Find(x => x.Id == 1).LastName == "Cantona");
            Assert.IsTrue(people.Find(x => x.Id == 3).FirstName == "Cantona");
        }

        [TestMethod]
        public void TestGetExistingPersonWithValidId()
        {
            // Agrange
            var controller = new PersonController(this.mockService.Object);
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();

            //Act
            var response = controller.Get(1);

            // Assert
            Assert.IsTrue(response.TryGetContentValue(out PersonModel person));
            Assert.IsTrue(person.FirstName == "David" && person.LastName == "Cantona" 
                && person.Age == 29 && person.AgeGroup == "Almost thirty");
        }

        [TestMethod]
        public void TestGetExistingPersonWithInValidId()
        {
            // Agrange
            var controller = new PersonController(this.mockService.Object);
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();

            //Act
            var response = controller.Get(100);

            // Assert
            Assert.IsTrue(response.StatusCode == System.Net.HttpStatusCode.NotFound);
        }

        [TestMethod]
        public void TestAddNewPersonWithInValidModel()
        {
            // Agrange
            var newPersonModel = TestingDataDource.InvalidNewPersonModel;
            var controller = new PersonController(this.mockService.Object);
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();
            controller.ModelState.AddModelError("InValidFirstName", "FirstName is required");

            //Act
            var response = controller.Post(newPersonModel);

            // Assert
            Assert.IsTrue(response.StatusCode == System.Net.HttpStatusCode.NotAcceptable);

            Assert.IsTrue(response.TryGetContentValue(out IList<ModelErrorCollection> errors));
            Assert.IsTrue(errors.Count == 1);
            var errorCollection = errors[0] as ModelErrorCollection;
            Assert.IsTrue(errorCollection.Count == 1 && errorCollection[0].ErrorMessage.Equals("FirstName is required"));
        }

        [TestMethod]
        public void TestAddNewPersonWithValidModel()
        {
            // Agrange
            var newPersonModel = TestingDataDource.ValidNewPersonModel;
            var controller = new PersonController(this.mockService.Object);
            controller.Request = new HttpRequestMessage()
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri("http://localhost:55611/" + "api/person/")
            };
            controller.Configuration = new HttpConfiguration();

            //Act
            var response = controller.Post(newPersonModel);

            // Assert
            Assert.IsTrue(response.StatusCode == System.Net.HttpStatusCode.Created);
            Assert.IsTrue(response.TryGetContentValue(out PersonModel model));
            Assert.IsTrue(model.Id > 0);
        }

        [TestMethod]
        public void TestEditNotExistingPersonWithValidModel()
        {
            // Agrange
            var personModel = TestingDataDource.ExistingPersonModel;
            personModel.Id = 10000; // change to invalid id
            var controller = new PersonController(this.mockService.Object);
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();

            //Act
            var response = controller.Update(personModel.Id, personModel);

            // Assert
            Assert.IsTrue(response.StatusCode == System.Net.HttpStatusCode.NotFound);
        }

        [TestMethod]
        public void TestEditExistingPersonWithInValidModel()
        {
            // Agrange
            var personModel = TestingDataDource.ExistingPersonModel;
            personModel.FirstName = string.Empty;
            var controller = new PersonController(this.mockService.Object);
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();
            controller.ModelState.AddModelError("InValidFirstName", "FirstName is required");

            //Act
            var response = controller.Update(personModel.Id, personModel);

            // Assert
            Assert.IsTrue(response.StatusCode == System.Net.HttpStatusCode.NotAcceptable);
            Assert.IsTrue(response.TryGetContentValue(out IList<ModelErrorCollection> errors));
            Assert.IsTrue(errors.Count == 1);
            var errorCollection = errors[0] as ModelErrorCollection;
            Assert.IsTrue(errorCollection.Count == 1 && errorCollection[0].ErrorMessage.Equals("FirstName is required"));
        }

        [TestMethod]
        public void TestEditExistingPersonWithValidModel()
        {
            // Agrange
            var personModel = TestingDataDource.ExistingPersonModel;
            personModel.FirstName = personModel.FirstName + " edited";
            var controller = new PersonController(this.mockService.Object);
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();

            //Act
            var response = controller.Update(personModel.Id, personModel);

            // Assert
            Assert.IsTrue(response.StatusCode == System.Net.HttpStatusCode.OK);
            Assert.IsTrue(response.TryGetContentValue(out PersonModel returnedModel));
            Assert.IsTrue(returnedModel.FirstName.Equals(personModel.FirstName));
        }

        [TestCleanup]
        public void TearDown()
        {
            // Remove resource
        }
    }
}
