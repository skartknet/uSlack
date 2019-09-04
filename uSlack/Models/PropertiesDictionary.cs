using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Core.Models;
using Umbraco.Core.Models.Entities;
using Umbraco.Core.Models.PublishedContent;

namespace uSlack.Models
{
    public class PropertiesDictionary : Dictionary<string, string>
    {
        public PropertiesDictionary(IEntity entity)
        {
            this.Add("id", entity.Id.ToString());
            this.Add("deleteDate", entity.DeleteDate.ToString());
            this.Add("createDate", entity.CreateDate.ToString());
        }

        public PropertiesDictionary(IPublishedContent node) : this(node as IEntity)
        {
            this.Add("name", node.Name);
            foreach (var prop in node.Properties)
            {
                this.Add(prop.Alias, prop.GetValue().ToString());
            }
        }

        public PropertiesDictionary(IContent node) : this(node as IEntity)
        {
            this.Add("name", node.Name);
            foreach (var prop in node.Properties)
            {
                this.Add(prop.Alias, prop.GetValue().ToString());
            }
        }
    }

}
