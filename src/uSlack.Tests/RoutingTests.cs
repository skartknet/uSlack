using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using uSlack.Services;
using uSlack.Services.Models;

namespace uSlack.Tests
{
    [TestClass]
    public class RoutingTests
    {
        [TestMethod]
        public void ShouldSelectcontroller()
        {
            var typeResolver = new InteractiveControllerTypeResolver();
            var controllerSelector = new InteractiveControllerSelector(typeResolver);


            var result = controllerSelector.SelectController(new InteractiveRoute()
            {
                Controller = "content",
                Method = "unpublish",
                Value = "123"
            });


        }
    }
}
