using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Autofac;
using AgeRanger.Service.Contract;

namespace AgeRange.IntegrationTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var autofac = new AutofacBootstrapperForIntegrationTest();
            autofac.DoStart();
            using (var scope = autofac.container.BeginLifetimeScope())
            {
                var service = scope.Resolve<IAgeRangeService>();
                var a = service.FindPeople(string.Empty);
            }
        }
    }
}
