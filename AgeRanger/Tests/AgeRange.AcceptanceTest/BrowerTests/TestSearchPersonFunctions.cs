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
    public class TestSearchPersonFunctions : BrowerTestBase
    {
        [TestMethod]
        public void IsSearchTextBoxAvailable()
        {
            var searchBox = webDriver.FindElement(By.CssSelector("input[name=searchBox]"));
            Assert.IsNotNull(searchBox);
            Assert.IsTrue(searchBox.Enabled);
            Assert.AreEqual(string.Empty, searchBox.Text);
        }

        [TestMethod]
        public void AllPeopleAreShownWhenPageLoaded()
        {
            var numberOftableRows = webDriver.FindElements(By.CssSelector("div[class=table-responsive] table tbody tr")).Count();

            var totalNumberDisplay = webDriver.FindElement(By.CssSelector("div[class=panel-footer] h4 span")).Text;

            var totalNumberDisplayOnFooter = 0;
            int.TryParse(totalNumberDisplay, out totalNumberDisplayOnFooter);

            Assert.IsTrue(numberOftableRows > 0);
            Assert.IsTrue(totalNumberDisplayOnFooter > 0);
            Assert.AreEqual(totalNumberDisplayOnFooter, totalNumberDisplayOnFooter);
        }

        [TestMethod]
        public void FilteringPeopleByFirstOrLastName()
        {
            this.WaitForElementVisible(By.CssSelector("div[class=table-responsive] table tbody tr"), 3);
            // Arrage
            var totalNumberDisplayOnTable = webDriver.FindElements(By.CssSelector("div[class=table-responsive] table tbody tr")).Count();
            
            // Act
            var searchBox = webDriver.FindElement(By.CssSelector("input[name=searchBox]"));
            searchBox.SendKeys("Nguyen");

            // Waiting for loading data
            Thread.Sleep(2000);

            var tableRows = webDriver.FindElements(By.CssSelector("div[class=table-responsive] table tbody tr"));

            Assert.IsTrue(tableRows.Count < totalNumberDisplayOnTable);
            Assert.IsTrue(tableRows.Count == 1);

            var tableColumns = tableRows.First().FindElements(By.CssSelector("td"));
            // Verify data on record
            Assert.IsTrue(tableColumns.Count == 7);

            // Baseline: #9	F:'Kitty'	 L:'Nguyen'	Age:203	 Group:'Vampire'
            Assert.AreEqual("9", tableColumns[0].Text); // Verify Id
            Assert.AreEqual("Kitty", tableColumns[1].Text); // Verify FirstName
            Assert.AreEqual("Nguyen", tableColumns[2].Text); // Verify LastName
            Assert.AreEqual("203", tableColumns[3].Text); // Verify Age
            Assert.AreEqual("Vampire", tableColumns[4].Text); // Verify AgeGroup
        }
    }
}
