using Microsoft.VisualStudio.TestTools.UnitTesting;
using uSlack.Tests.Data;

namespace uSlack.Tests
{
    [TestClass]
    public class SecurityTests
    {
        [TestMethod]
        public void SlackSignatureShouldBeValid_demoData()
        {
            var payload = TestingData.DemoPayload;
            string slackSignature = "v0=a2114d57b48eac39b9ad189dd8316235a7b4a8d21a10bd27519666489c69b503";
            string signingSecret = "8f742231b10e8888abcd99yyyzzz85a5";

            var obj = new PrivateObject(new Security.SecurityService());

            var isValid = (bool)obj.Invoke("IsValidSlackSignature", 1531420618, payload, slackSignature, signingSecret);

            Assert.IsTrue(isValid);
        }

        [TestMethod]
        public void SlackSignatureShouldBeValid_realData()
        {
            var payload = TestingData.RealPayload;
            string slackSignature = "v0=93262c675fa246e100dd4dac560911542969b7624dfb62573f8898e48805588a";
            string signingSecret = "082329709a7f677932a374734ace7f0c";
            int timespan = 1562806230;


            var obj = new PrivateObject(new Security.SecurityService());

            var isValid = (bool)obj.Invoke("IsValidSlackSignature", timespan, payload, slackSignature, signingSecret);


            Assert.IsTrue(isValid);
        }
    }
}
