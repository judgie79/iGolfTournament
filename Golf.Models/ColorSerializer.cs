using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Web;

namespace Golf.Tournament.Models
{
    public class ColorSerializer : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var color = value as Color;
            writer.WriteValue(color.Value);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return new Color(reader.Value.ToString());
        }

        public override bool CanConvert(Type objectType)
        {
            return typeof(Color).IsAssignableFrom(objectType);
        }
    }
}