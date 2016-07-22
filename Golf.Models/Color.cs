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
    [Serializable]
    [JsonConverter(typeof(ColorSerializer))]
    public class Color : ISerializable
    {
        System.Drawing.Color _color;

        public Color()
        {
            _color = new System.Drawing.Color();
        }

        public Color(string value)
        {
            _color = (System.Drawing.Color)System.Drawing.ColorTranslator.FromHtml(value);
        }

        public byte R
        {
            get
            {
                
                return _color.R;
            }
        }

        public byte G
        {
            get
            {

                return _color.G;
            }
        }

        public byte B
        {
            get
            {

                return _color.B;
            }
        }

        public string Name
        {
            get
            {
                if (_color.IsNamedColor)
                    return _color.Name;
                else
                    return System.Drawing.ColorTranslator.ToHtml(_color);
            }
        }

        public string Value
        {
            get
            {
                return System.Drawing.ColorTranslator.ToHtml(_color);
            }

            set
            {
                _color = System.Drawing.ColorTranslator.FromHtml(value);
            }
        }



        public override string ToString()
        {
            return System.Drawing.ColorTranslator.ToHtml(_color); 
        }

        public string ToHtml()
        {
            return System.Drawing.ColorTranslator.ToHtml(_color);
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return _color.GetHashCode();
        }

        //note: this is private to control access; the serializer can still access this constructor
        private Color(SerializationInfo info, StreamingContext ctxt)
        {
            _color = (System.Drawing.Color)System.Drawing.ColorTranslator.FromHtml(info.GetString("Value"));
        }

        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
        public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
        {
            info.AddValue("Value", Value);
        }

    }

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