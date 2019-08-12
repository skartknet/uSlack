using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using uSlack.Models;

namespace uSlack.Deserialisation
{
    public class InteractiveActionConverter : JsonConverter
    {
        public override bool CanWrite => false;
        public override bool CanRead => true;


        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            try
            {
                var array = JArray.Load(reader);

                var list = new List<IAction>();

                foreach (var item in array)
                {
                    var action = default(IAction);

                    switch (item["type"].Value<string>())
                    {
                        case "button":
                            action = item.ToObject<ButtonElementInteractive>();
                            break;
                        case "static_select":
                            action = item.ToObject<StaticSelectElementInteractive>();
                            break;
                    }


                    list.Add(action);
                }

                return list;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(IAction);
        }
    }
}
