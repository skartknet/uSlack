using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.IO;
using Umbraco.Core.IO;
using uSlack.Configuration;

namespace uSlack.Tests
{
    [TestFixture]
    public class ConfigurationServiceShould
    {
        string configLocation = "~/App_Plugins/uSlack/Config/";
        string _configFilePath;


        [OneTimeSetUp]
        public void SetVariables()
        {
            _configFilePath = IOHelper.MapPath(configLocation + "uslack.config");
        }

        [TearDown]
        public void CleanUp()
        {
            File.Delete(_configFilePath);
        }


        [Test]
        public void ConfigFromFileHasCorrectStructure()
        {
            File.WriteAllText(_configFilePath, @"{
                                          ""token"": ""123"",
                                          ""channel"": ""uslack"",
                                          ""sections"": {
                                            ""contentService"": {
                                              ""parameters"": {
                                                ""published"": true,
                                                ""unpublished"": true,
                                                ""deleted"": true,
                                                ""moved"": true,
                                                ""rolledBack"": true
                                              }
                                            }        
                                          }
                                        }
                                        ");

            var sut = new ConfigurationService();
            sut.Initialize();

            Assert.That(sut.AppConfiguration.Token, Is.EqualTo("123"));
            Assert.That(sut.AppConfiguration.SlackChannel, Is.EqualTo("uslack"));

            Assert.That(sut.AppConfiguration.Sections, Has.Count.EqualTo(1));
            Assert.That(sut.AppConfiguration.Sections["contentService"].Parameters, Has.Count.EqualTo(5));
            Assert.That(sut.AppConfiguration.Sections["contentService"].Parameters["moved"], Is.EqualTo(true));
        }

        [Test]
        public void NotCrashIfconfigNotPresent()
        {
            var sut = new ConfigurationService();            

            Assert.DoesNotThrow(() => sut.Initialize());
            Assert.That(sut.AppConfiguration, Is.InstanceOf<UslackConfiguration>());
        }

        [Test]
        public void DeserializeAppConfigurationProperly()
        {
            var json = @"{
                            ""token"": ""123"",
                            ""channel"": ""uslack"",
                            ""sections"": {
                            ""contentService"": {
                                ""parameters"": {
                                ""published"": true,
                                ""unpublished"": true,
                                ""deleted"": true,
                                ""moved"": true,
                                ""rolledBack"": true
                                }
                            }        
                            }
                        }
                        ";

            Assert.DoesNotThrow(()=> JsonConvert.DeserializeObject<UslackConfiguration>(json));
        }

        [Test]
        public void ConfigChangeIsSavedToFile() {
            File.WriteAllText(_configFilePath, @"{
                                          ""token"": ""123"",
                                          ""channel"": ""uslack"",
                                          ""sections"": {
                                            ""contentService"": {
                                              ""parameters"": {
                                                ""published"": true,
                                                ""unpublished"": true,
                                                ""deleted"": true,
                                                ""moved"": true,
                                                ""rolledBack"": true
                                              }
                                            }        
                                          }
                                        }
                                        ");

            var sut = new ConfigurationService().Initialize();

            var json = JsonConvert.SerializeObject(sut.AppConfiguration);
            var model = JsonConvert.DeserializeObject<UslackConfiguration>(json);

            model.Token = "abc";

            sut.SaveAppConfiguration(model);

            var tester = new ConfigurationService().Initialize();
            Assert.That(tester.AppConfiguration.Token, Is.EqualTo("abc"));

        }

        [Test]
        public void SupportObjectAsParameter()
        {
            File.WriteAllText(_configFilePath, @"{
                                          ""token"": ""123"",
                                          ""channel"": ""uslack"",
                                          ""sections"": {
                                            ""contentService"": {
                                              ""parameters"": {
                                                ""boolean"": true,
                                                ""string"": ""abc"", 
                                                ""integer"": 123
                                              }
                                            }        
                                          }
                                        }
                                        ");

            var sut = new ConfigurationService().Initialize();            
            Assert.That(sut.GetParameter<bool>("boolean", "contentService"), Is.EqualTo(true));
            Assert.That(sut.GetParameter<string>("string", "contentService"), Is.EqualTo("abc"));
            Assert.That(sut.GetParameter<Int64>("integer", "contentService"), Is.EqualTo(123));

        }

        [Test]
        public void ReturnDefaultValuesForNotFoundKeys()
        {
            File.WriteAllText(_configFilePath, @"{
                                          ""token"": ""123"",
                                          ""channel"": ""uslack"",
                                          ""sections"": {
                                            ""contentService"": {
                                              ""parameters"": {
                                                ""boolean"": true,
                                                ""string"": ""abc"", 
                                                ""integer"": 123
                                              }
                                            }        
                                          }
                                        }
                                        ");

            var sut = new ConfigurationService().Initialize();

            Assert.That(sut.GetParameter<bool>("b", "contentService"), Is.EqualTo(default(bool)));
            Assert.That(sut.GetParameter<string>("s", "contentService"), Is.EqualTo(default(string)));
            Assert.That(sut.GetParameter<Int64>("i", "contentService"), Is.EqualTo(default(Int64)));

            Assert.That(sut.GetParameter<bool>("boolean", "c"), Is.EqualTo(default(bool)));
            Assert.That(sut.GetParameter<string>("string", "c"), Is.EqualTo(default(string)));
            Assert.That(sut.GetParameter<Int64>("integer", "c"), Is.EqualTo(default(Int64)));
        }

        [Test]
        public void ReturnsMessageFileContent()
        {
            var sut = new ConfigurationService();
            sut.Initialize();

            var fileContent = sut.GetMessage("filetest");
            Assert.That(fileContent, Is.EqualTo("testcontent"));
        }

        [Test]
        public void ReturnsNotFoundExceptionForWrongMessageAlias()
        {
            var sut = new ConfigurationService();
            sut.Initialize();

            Assert.Throws<FileNotFoundException>(() => sut.GetMessage("wrongAlias"));
        }
    }
}
