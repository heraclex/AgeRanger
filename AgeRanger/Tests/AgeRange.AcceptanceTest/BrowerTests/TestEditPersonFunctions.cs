using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgeRange.AcceptanceTest.BrowerTests
{
    [TestClass]
    public class TestEditPersonFunctions : BrowerTestBase 
    {
        [TestMethod]
        public void EditPersonPopupLoadingCorrectly()
        {
            this.WaitForElementVisible(By.CssSelector("div[class=table-responsive] table tbody tr td"), 3);
            var personRowOnTop = webDriver.FindElements(By.CssSelector("div[class=table-responsive] table tbody tr")).FirstOrDefault();
            var personFields = personRowOnTop.FindElements(By.TagName("td"));

            // Jump to column has Edit button, hit it to open Edit Popup
            personFields[personFields.Count - 2].FindElement(By.TagName("a")).Click();
            this.WaitForElementVisible(By.CssSelector("body div[class=modal-dialog]"), 3);

            var popupTitle = webDriver.FindElement(By.CssSelector("h3")).Text;
            var inputFirstNameText = webDriver.FindElement(By.Id("firstname")).GetAttribute("value");
            var inputLastNameText = webDriver.FindElement(By.Id("lastname")).GetAttribute("value");
            var inputAge = webDriver.FindElement(By.Id("age")).GetAttribute("value");
            webDriver.FindElement(By.CssSelector("button[id=btnCancel]")).Click();

            Assert.AreEqual("Edit Person (#: " + personFields[0].Text + ")", popupTitle);
            Assert.AreEqual(personFields[1].Text, inputFirstNameText);
            Assert.AreEqual(personFields[2].Text, inputLastNameText);
            Assert.AreEqual(personFields[3].Text, inputAge);
        }

        [Ignore]
        [TestMethod]
        public void EditPersonWithInvalidValidData()
        {
            // Will be implement later.
            throw new NotImplementedException();
        }
    }
}
