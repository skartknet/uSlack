using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SlackAPI;
using SlackAPI.Composition;
using SlackAPI.Interactive;
using Umbraco.Core.Composing;
using uSlack.Interactive;

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

            var array = JArray.Load(reader);

            var list = new List<IInteractiveElement>();

            //Maybe we can make this more elegant using the type discriminator. https://github.com/manuc66/JsonSubTypes            
            foreach (var item in array)
            {
                var action = default(IInteractiveElement);
                var type = item["type"].Value<string>();
                switch (type)
                {
                    case ElementTypes.Button:
                        action = item.ToObject<ButtonElementInteractive>();
                        break;
                    case ElementTypes.DatePicker:
                        action = item.ToObject<DatePickerElementInteractive>();
                        break;
                    case ElementTypes.StaticSelect:
                        action = item.ToObject<StaticSelectElementInteractive>();
                        break;
                    case ElementTypes.ExternalSelect:
                        action = item.ToObject<ExternalSelectElementInteractive>();
                        break;
                    case ElementTypes.UserSelect:
                        action = item.ToObject<UserSelectElementInteractive>();
                        break;
                    case ElementTypes.Overflow:
                        action = item.ToObject<OverflowElementInteractive>();
                        break;
                    default:
                        action = null;
                        Current.Logger.Error(typeof(InteractiveActionConverter), $"An interactive element of type '{type}' is not supported.");
                        break;
                }

                list.Add(action);
            }

            return list;

        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(IInteractiveElement);
        }
    }
}
