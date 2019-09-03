﻿using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace uSlack.Tests
{
    [TestClass]
    public class SecurityTests
    {
        [TestMethod]
        public void SlackSignatureShouldBeValid_demoData()
        {
            var payload =
                "token=xyzz0WbapA4vBCDEFasx0q6G&team_id=T1DC2JH3J&team_domain=testteamnow&channel_id=G8PSS9T3V&channel_name=foobar&user_id=U2CERLKJA&user_name=roadrunner&command=%2Fwebhook-collect&text=&response_url=https%3A%2F%2Fhooks.slack.com%2Fcommands%2FT1DC2JH3J%2F397700885554%2F96rGlfmibIGlgcZRskXaIFfN&trigger_id=398738663015.47445629121.803a0bc887a14d10d2c447fce8b6703c";
            string slackSignature = "v0=a2114d57b48eac39b9ad189dd8316235a7b4a8d21a10bd27519666489c69b503";
            string signingSecret = "8f742231b10e8888abcd99yyyzzz85a5";

            var obj = new PrivateObject(new Security.SecurityService());

            var isValid = (bool)obj.Invoke("IsValidSlackSignature", 1531420618, payload, slackSignature, signingSecret);

            Assert.IsTrue(isValid);
        }

        [TestMethod]
        public void SlackSignatureShouldBeValid_realData()
        {
            var payload =
                "payload=%7B%22type%22%3A%22block_actions%22%2C%22team%22%3A%7B%22id%22%3A%22TKAKBLC56%22%2C%22domain%22%3A%22exxenmedia%22%7D%2C%22user%22%3A%7B%22id%22%3A%22UJZ6URSMR%22%2C%22username%22%3A%22lopezadeh%22%2C%22name%22%3A%22lopezadeh%22%2C%22team_id%22%3A%22TKAKBLC56%22%7D%2C%22api_app_id%22%3A%22AKAKEAJJC%22%2C%22token%22%3A%22HyTjyNFr2NHlQC0CqkkZtLYy%22%2C%22container%22%3A%7B%22type%22%3A%22message%22%2C%22message_ts%22%3A%221562642155.000100%22%2C%22channel_id%22%3A%22CKCEGGARM%22%2C%22is_ephemeral%22%3Afalse%7D%2C%22trigger_id%22%3A%22679572444339.656657692176.19e6516fa527f1589e91484f32c904c0%22%2C%22channel%22%3A%7B%22id%22%3A%22CKCEGGARM%22%2C%22name%22%3A%22general%22%7D%2C%22message%22%3A%7B%22type%22%3A%22message%22%2C%22subtype%22%3A%22bot_message%22%2C%22text%22%3A%22test1%22%2C%22ts%22%3A%221562642155.000100%22%2C%22username%22%3A%22uSlack%22%2C%22bot_id%22%3A%22BKE0G32UX%22%2C%22blocks%22%3A%5B%7B%22type%22%3A%22actions%22%2C%22block_id%22%3A%22O4i1%22%2C%22elements%22%3A%5B%7B%22type%22%3A%22button%22%2C%22action_id%22%3A%22DKC%22%2C%22text%22%3A%7B%22type%22%3A%22plain_text%22%2C%22text%22%3A%22Farmhouse%22%2C%22emoji%22%3Atrue%7D%2C%22value%22%3A%22click_me_123%22%7D%2C%7B%22type%22%3A%22button%22%2C%22action_id%22%3A%22rRVe1%22%2C%22text%22%3A%7B%22type%22%3A%22plain_text%22%2C%22text%22%3A%22Kin+Khao%22%2C%22emoji%22%3Atrue%7D%2C%22value%22%3A%22click_me_123%22%7D%2C%7B%22type%22%3A%22button%22%2C%22action_id%22%3A%223nT%22%2C%22text%22%3A%7B%22type%22%3A%22plain_text%22%2C%22text%22%3A%22Ler+Ros%22%2C%22emoji%22%3Atrue%7D%2C%22value%22%3A%22click_me_123%22%7D%5D%7D%5D%7D%2C%22response_url%22%3A%22https%3A%5C%2F%5C%2Fhooks.slack.com%5C%2Factions%5C%2FTKAKBLC56%5C%2F693116707030%5C%2FvdFROXGwvhLiTrpKuHH80w9E%22%2C%22actions%22%3A%5B%7B%22action_id%22%3A%22DKC%22%2C%22block_id%22%3A%22O4i1%22%2C%22text%22%3A%7B%22type%22%3A%22plain_text%22%2C%22text%22%3A%22Farmhouse%22%2C%22emoji%22%3Atrue%7D%2C%22value%22%3A%22click_me_123%22%2C%22type%22%3A%22button%22%2C%22action_ts%22%3A%221562806229.993035%22%7D%5D%7D";
            string slackSignature = "v0=93262c675fa246e100dd4dac560911542969b7624dfb62573f8898e48805588a";
            string signingSecret = "082329709a7f677932a374734ace7f0c";
            int timespan = 1562806230;


            var obj = new PrivateObject(new Security.SecurityService());

            var isValid = (bool)obj.Invoke("IsValidSlackSignature", timespan, payload, slackSignature, signingSecret);


            Assert.IsTrue(isValid);
        }
    }
}
