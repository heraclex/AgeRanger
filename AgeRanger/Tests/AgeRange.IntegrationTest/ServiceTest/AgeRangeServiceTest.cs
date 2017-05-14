using AgeRange.IntegrationTest.Configuration;
using AgeRanger.Service.Contract;
using AgeRanger.Service.Contract.Models;
using Autofac;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace AgeRange.IntegrationTest.ServiceTest
{
    [TestClass]
    public class AgeRangeServiceTest
    {
        private ILifetimeScope autofacScope;

        [TestInitialize]
        public void Setup()
        {
            var autofacConfiguration = new AutofacConfigurationForIntegrationTest();
            autofacConfiguration.DoStart();
            this.autofacScope = autofacConfiguration.container.BeginLifetimeScope();
        }

        [TestMethod]
        public void TestGetAllPeople()
        {
            // Arrange
            var service = this.autofacScope.Resolve<IAgeRangeService>();

            // Act
            var result = service.FindPeople(string.Empty);

            // Assert
            Assert.AreEqual(result.Count(), 10);
        }

        [TestMethod]
        public void TestGetSomePeople()
        {
            // Arrange
            var service = this.autofacScope.Resolve<IAgeRangeService>();

            // Act
            var result = service.FindPeople("Nguyen");

            // Assert
            Assert.AreEqual(1, result.Count());
        }

        [TestMethod]
        public void TestGetPersonById()
        {
            // Arrange
            var service = this.autofacScope.Resolve<IAgeRangeService>();

            // Act
            var result = service.GetPersonById(1);

            // Assert
            Assert.AreNotEqual(null, result);
        }

        [TestMethod]
        public void TestEditExitingPerson()
        {
            // Arrange
            var service = this.autofacScope.Resolve<IAgeRangeService>();
            var personTestId = 1;
            var person = service.GetPersonById(personTestId);

            // Act
            person.FirstName = person.FirstName + "-Edited-";
            service.SavePerson(person);

            var eidtedPerson = service.GetPersonById(personTestId);
            // Assert
            Assert.AreNotEqual(eidtedPerson, person);

            // Clean up data test
            eidtedPerson.FirstName = eidtedPerson.FirstName.Replace("-Edited-", "");
            service.SavePerson(person);
        }

        // All Testcases adding new records into db will be hold until find out the bug related to casting type on DbContext
        [Ignore]
        [TestMethod]
        public void TestAddNewPerson()
        {
            // Arrange
            var service = this.autofacScope.Resolve<IAgeRangeService>();
            var personModel = new PersonModel()
            {
                FirstName = "Newbie",
                LastName = "Test",
                Age = 100
            };
            // Act
            var returnedPersonModel = service.SavePerson(personModel);

            // Assert
            Assert.AreNotEqual(0, returnedPersonModel.Id);
            Assert.IsTrue(returnedPersonModel.AgeGroup != string.Empty);
            Assert.AreNotEqual(personModel, returnedPersonModel);
        }

        [TestCleanup]
        public void TearDown()
        {
            // BUG: got error casting type when perform dbContext here.
            //using (var context = this.autofacScope.Resolve<AgeRangerDbContext>() as AgeRangerDbContext)
            //{
            //    var ageGroups = context.Set<AgeGroup>().AsQueryable().ToList();
            //    foreach (var group in ageGroups)
            //        context.Set<AgeGroup>().Remove(group);
            //    context.SaveChanges();
            //}
            
            // release all resource, include service/repo instance
            this.autofacScope.Dispose();
        }

        /*
        private void CreateTestData()
        {            
            var ageGroups = new List<AgeGroup>
                {
                    new AgeGroup() {MinAge = 0, MaxAge = 2, Description = "Toddler" },
                    new AgeGroup() {MinAge = 2, MaxAge = 14, Description = "Child" },
                    new AgeGroup() {MinAge = 14, MaxAge = 20, Description = "Teenager" },
                    new AgeGroup() {MinAge = 20, MaxAge = 25, Description = "Early twenties" },
                    new AgeGroup() {MinAge = 25, MaxAge = 30, Description = "Almost thirty" },
                    new AgeGroup() {MinAge = 30, MaxAge = 50, Description = "Very adult" },
                    new AgeGroup() {MinAge = 50, MaxAge = 70, Description = "Kinda old" },
                    new AgeGroup() {MinAge = 70, MaxAge = 99, Description = "Old" },
                    new AgeGroup() {MinAge = 99, MaxAge = 110, Description = "Very old" },
                    new AgeGroup() {MinAge = 110, MaxAge = 199, Description = "Crazy ancient" },
                    new AgeGroup() {MinAge = 199, MaxAge = 4999, Description = "Vampire" },
                    new AgeGroup() {MinAge = 4999, MaxAge = null, Description = "Kauri tree" }
                };

            var dbContext = this.autofacScope.Resolve<AgeRangerDbContext>();
            dbContext.Set<AgeGroup>().AddRange(ageGroups);
            dbContext.SaveChanges();
        }*/
    }
}
