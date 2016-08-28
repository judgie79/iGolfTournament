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

        [JsonProperty("scoreCardIsCreated")]
        public bool ScorecardIsCreated { get; set; }
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
}