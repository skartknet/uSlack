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
   
        internal AppConfig CreateDefaultConfiguration()
        {
            var baseConfig = new AppConfig();
            baseConfig.Sections = BuildSections();                        

            return baseConfig;
        }

        protected Dictionary<string, ConfigSection> BuildSections()
        {
            var dict = new Dictionary<string, ConfigSection>();
            var registeredTypes = GetTypesWithConfigurationAttribute();

            foreach (var type in registeredTypes)
            {
                var methods = type.GetMethods();
                foreach (var method in methods)
                {

                }
            }

            return dict;
        }
     

        //https://stackoverflow.com/questions/607178/how-enumerate-all-classes-with-custom-class-attribute
        protected IEnumerable<Type> GetTypesWithConfigurationAttribute()
        {
            var configuredTypes =
                from a in AppDomain.CurrentDomain.GetAssemblies().AsParallel()
                from t in a.GetTypes()
                let attributes = t.GetCustomAttributes(typeof(SectionHandlersAttribute), true)
                where attributes != null && attributes.Length > 0
                select t;

            return configuredTypes;
        }
    }
}
