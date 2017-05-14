using AgeRanger.Service.Contract.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AgeRange.AcceptanceTest.BrowerTests
{
    [TestClass]
    public class TestAddPersonFunctions : BrowerTestBase
    {
        [TestMethod]
        public void AddNewPersonPopupLoadingCorrectly()
        {
            this.WaitForElementVisible(By.CssSelector("div[class=table-responsive] table tbody tr td"), 3);
            webDriver.FindElement(By.Id("addPersonBtn")).Click();
            this.WaitForElementVisible(By.CssSelector("body div[class=modal-dialog ]"), 3);

            var popupTitle = webDriver.FindElement(By.CssSelector("h3[class=modal-title]")).Text;
            var inputFirstNameText = webDriver.FindElement(By.Id("firstname")).Text;
            var inputLastNameText = webDriver.FindElement(By.Id("lastname")).Text;
            var inputAge = webDriver.FindElement(By.Id("age")).GetAttribute("value");
            webDriver.FindElement(By.CssSelector("button[id=btnCancel]")).Click();

            Assert.AreEqual("Add New Person", popupTitle);
            Assert.AreEqual(string.Empty, inputFirstNameText);
            Assert.AreEqual(string.Empty, inputLastNameText);
            Assert.AreEqual("0", inputAge);
        }

        [TestMethod]
        public void AddNewPersonWithValidData()
        {
            var newPersonModel = new PersonModel()
            {
                FirstName = "First A",
                LastName = "Last B",
                Age = 1000
            };

            this.WaitForElementVisible(By.Id("addPersonBtn"), 3);
            webDriver.FindElement(By.Id("addPersonBtn")).Click();
            this.WaitForElementVisible(By.CssSelector("body div[class=modal-dialog ]"), 3);
                        
            webDriver.FindElement(By.Id("firstname")).SendKeys(newPersonModel.FirstName);
            webDriver.FindElement(By.Id("lastname")).SendKeys(newPersonModel.LastName);
            webDriver.FindElement(By.Id("age")).SendKeys(newPersonModel.Age.ToString());
            webDriver.FindElement(By.CssSelector("button[id=btnAdd]")).Click();

            // Waiting for reload data
            Thread.Sleep(2000);
            var newPersonTableRow = webDriver.FindElements(By.CssSelector("div[class=table-responsive] table tbody tr")).FirstOrDefault();

            Assert.IsNotNull(newPersonTableRow);
            var personFields = newPersonTableRow.FindElements(By.TagName("td"));
            Assert.AreNotEqual("0", personFields[0].Text); // Verify Id
            Assert.AreEqual(newPersonModel.FirstName, personFields[1].Text);// Verify FirstName
            Assert.AreEqual(newPersonModel.LastName, personFields[2].Text);// Verify LastName
            Assert.AreEqual(newPersonModel.Age.ToString(), personFields[3].Text);// Verify Age

            // remove test data
            personFields.Last().FindElement(By.TagName("a")).Click();
            Thread.Sleep(3000);
        }

        [Ignore]
        [TestMethod]
        public void AddNewPersonWithInvalidValidData()
        {
            // Will be implement later.
            throw new NotImplementedException();
        }

    }
}


