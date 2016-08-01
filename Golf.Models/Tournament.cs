using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace Golf.Tournament.Models
{
    [JsonConverter(typeof(TournamentConverter))]
    public class Tournament
    {
        static readonly Dictionary<Type, TournamentType> typeToSubType;
        static readonly Dictionary<TournamentType, Type> subTypeToType;

        static Tournament()
        {
            typeToSubType = new Dictionary<Type, TournamentType>()
        {
            { typeof(Tournament), TournamentType.Single },
            { typeof(TeamTournament), TournamentType.Team }
        };
            subTypeToType = typeToSubType.ToDictionary(pair => pair.Value, pair => pair.Key);
        }

        public static Type GetType(TournamentType type)
        {
            return subTypeToType[type];
        }

        [JsonProperty("_id")]
        public string Id { get; set; }

        [JsonProperty("type")]
        [Required]
        [JsonConverter(typeof(StringEnumConverter))]
        public TournamentType TournamentType { get { return typeToSubType[GetType()]; } }

        [JsonProperty("scoreType")]
        [Required]
        public ScoreType ScoreType { get; set; }

        [JsonProperty("title")]
        [Required]
        public string Title { get; set; }

        [JsonProperty("participants")]
        public TournamentParticipantCollection Participants { get; set; }

        [JsonProperty("club")]
        [Required]
        public Club Club { get; set; }

        [JsonProperty("course")]
        [Required]
        public Course Course { get; set; }

        [JsonProperty("date")]
        [DataType(DataType.DateTime)]
        public DateTime Date { get; set; }

        [JsonProperty("hasStarted")]
        public bool HasStarted { get; set; }

        [JsonProperty("hasFinished")]
        public bool HasFinished { get; set; }
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum TournamentType
    {
        [Display(Name = "Single")]
        [EnumMember(Value = "single")]
        Single,

        [Display(Name = "Team")]
        [EnumMember(Value = "team")]
        Team
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum ScoreType
    {
        [Display(Name = "Stableford")]
        [EnumMember(Value = "stableford")]
        Stableford,

        [Display(Name = "Strokeplay")]
        [EnumMember(Value = "strokeplay")]
        Strokeplay
    }

    public class TournamentConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(Tournament);
        }

        public override bool CanWrite { get { return false; } }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var token = JToken.Load(reader);
            var typeToken = token["type"];
            if (typeToken == null)
                throw new InvalidOperationException("invalid object");
            var actualType = Tournament.GetType(typeToken.ToObject<TournamentType>(serializer));
            if (existingValue == null || existingValue.GetType() != actualType)
            {
                var contract = serializer.ContractResolver.ResolveContract(actualType);
                existingValue = contract.DefaultCreator();
            }
            using (var sr = new StringReader(token.ToString()))
            {
                // Using "populate" avoids infinite recursion.
                serializer.Populate(sr, existingValue);
            }
            return existingValue;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}