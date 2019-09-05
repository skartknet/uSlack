//using System;
//using System.Net.Http;
//using System.Threading.Tasks;
//using Microsoft.VisualStudio.TestTools.UnitTesting;

//namespace uSlack.Tests
//{
//    [TestClass]
//    public class InteractivityTests
//    {
//        [TestMethod]
//        public async Task TestMethod1()
//        {
//            var payload = @"{""""type"""":""""block_actions"",""team"":{""id"":""T0CAG"",""domain"":""acme-creamery""},""user"":{""id"":""U0CA5"",""username"":""Amy McGee"",""name"":""Amy McGee"",""team_id"":""T3MDE""},""api_app_id"":""A0CA5"",""token"":""Shh_its_a_seekrit"",""container"":{""type"":""message"",""text"":""The contents of the original message where the action originated""},""trigger_id"":""12466734323.1395872398"",""response_url"":""https://www.postresponsestome.com/T123567/1509734234"",""actions"":[{""type"":""button"",""block_id"":""VDK4"",""action_id"":""Hkw"",""text"":{""type"":""plain_text"",""text"":""Farmhouse"",""emoji"":true},""value"":""click_me_123"",""action_ts"":""1565043554.591044""}]}";

//            var contentPayload = new MultipartFormDataContent();
//            contentPayload.LoadIntoBufferAsync()

//            using (HttpClient client = new HttpClient())
//            {
//                client.BaseAddress = new Uri("http://localhost:8888");
//                client.PostAsync("/umbraco/api/interactiveapi/processresponse", )


//                var response = await client.GetAsync("/umbraco/api/interactiveapi/processresponse");
//            }
//        }
//    }
//}
