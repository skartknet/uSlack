using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Lucene.Net.Search;
using uSlack.Configuration;

namespace uSlack.Configuration
{
    class ConfigurationBuilder
    {

        internal AppConfiguration CreateDefaultConfiguration()
        {
            var baseConfig = new AppConfiguration {Sections = BuildSections()};

            return baseConfig;
        }

        protected Dictionary<string, ConfigSection> BuildSections()
        {
            var dict = new Dictionary<string, ConfigSection>();
            var registeredTypes = GetConfigurationSections();

            foreach (var sectionType in registeredTypes)
            {
                //var section = new KeyValuePair<string, ConfigSection>
                //(
                //    sectionType.GetCustomAttribute<EventHandlerAttribute>().Alias
                //);

                var methods = GetConfigurationEventHandlers(sectionType);
                var configSection = new ConfigSection();
                foreach (var method in methods)
                {
                    var attr = method.GetCustomAttribute<EventHandlerAttribute>();
                    configSection.Parameters.Add(attr.Alias, attr.DefaultValue);
                }

                dict.Add(sectionType.GetCustomAttribute<EventHandlerAttribute>().Alias,
                        configSection);
            }

            return dict;
        }

        protected IEnumerable<MethodInfo> GetConfigurationEventHandlers(Type section)
        {
            return section.GetMethods().Where(m => m.GetCustomAttributes().Any(att => att is EventHandlerAttribute));
        }


        //https://stackoverflow.com/questions/607178/how-enumerate-all-classes-with-custom-class-attribute
        protected IEnumerable<Type> GetConfigurationSections()
        {
            var configuredTypes =
                from a in AppDomain.CurrentDomain.GetAssemblies().AsParallel()
                from t in a.GetTypes()
                let attributes = t.GetCustomAttributes(typeof(SectionHandlerAttribute), true)
                where attributes != null && attributes.Length > 0
                select t;

            return configuredTypes;
        }
    }
}
