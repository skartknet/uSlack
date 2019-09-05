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
    /// <summary>
    /// It creates a default Configuration Group using those classes and methods in all the assemblies that use
    /// the <see cref="SectionHandlerAttribute"/> and <see cref="EventHandlerAttribute"/>.
    /// </summary>
    public class ConfigurationBuilder : IConfigurationBuilder
    {

        public ConfigurationGroup CreateDefaultConfiguration()
        {
            var registeredTypes = GetConfigurationSections();
            
            var baseConfig = new ConfigurationGroup {Sections = BuildSections(registeredTypes)};

            return baseConfig;
        }

        protected virtual Dictionary<string, ConfigSection> BuildSections(IEnumerable<Type> registeredSections)
        {
            var dict = new Dictionary<string, ConfigSection>();

            foreach (var sectionType in registeredSections)
            {             
                var methods = GetConfigurationEventHandlers(sectionType);
                var configSection = new ConfigSection();
                configSection.SectionHandlers = new Dictionary<string, object>();

                foreach (var method in methods)
                {
                    var attr = method.GetCustomAttribute<EventHandlerAttribute>();

                    if (attr == null) continue;

                    configSection.SectionHandlers.Add(attr.Alias, attr.DefaultValue);
                }

                dict.Add(sectionType.GetCustomAttribute<SectionHandlerAttribute>().Alias,
                        configSection);
            }

            return dict;
        }

        private IEnumerable<MethodInfo> GetConfigurationEventHandlers(Type section)
        {
            return section.GetMethods().Where(m => m.GetCustomAttributes().Any(att => att is EventHandlerAttribute));
        }


        //https://stackoverflow.com/questions/607178/how-enumerate-all-classes-with-custom-class-attribute
        private IEnumerable<Type> GetConfigurationSections()
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
