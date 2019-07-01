using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using uSlack.Configuration;

namespace uSlack.Configuration
{
    class ConfigurationBuilder
    {
        private ConfigurationBuilder()
        { }

        protected static void Init()
        {
            var template = new List<AppConfig>();

            var baseConfig = new AppConfig();
            baseConfig.Sections = BuildSections();

            template.Add(baseConfig);

            var config = JsonConvert.SerializeObject(template);
        }

        private static Dictionary<string, ConfigSection> BuildSections()
        {
            var dict = new Dictionary<string, ConfigSection>();
            var registeredTypes = GetTypesWithConfigurationAttribute();

            return dict;
        }

        private void FindRegisteredConfiguration()
        {

        }

        //https://stackoverflow.com/questions/607178/how-enumerate-all-classes-with-custom-class-attribute
        static IEnumerable<Type> GetTypesWithConfigurationAttribute()
        {
            var configuredTypes =
                from a in AppDomain.CurrentDomain.GetAssemblies().AsParallel()
                from t in a.GetTypes()
                let attributes = t.GetCustomAttributes(typeof(RegisterAsUslackConfigurationAttribute), true)
                where attributes != null && attributes.Length > 0
                select t;

            return configuredTypes;
        }
    }
}
