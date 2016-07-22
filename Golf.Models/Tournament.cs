using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Golf.Tournament.Models
{
    public class Tournament
    {
        [JsonProperty("_id")]
        public string Id { get; set; }

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
    }

    public class TournamentParticipant
    {
        [JsonProperty("_id")]
        public string Id { get; set; }

        [JsonProperty("player")]
        public Player Player { get; set; }

        [JsonProperty("teeBoxId")]
        public string TeebBoxId { get; set; }

        [JsonProperty("teaTime")]
        public DateTime TeaTime { get; set; }
    }

    [JsonArray]
    public class TournamentParticipantCollection : List<TournamentParticipant>
    {
        public TournamentParticipantCollection()
            : base()
        {

        }

        public TournamentParticipantCollection(IEnumerable<TournamentParticipant> participants)
            : base(participants)
        {

        }
    }

        public class TeamTournament : Tournament
    {
        [JsonProperty("teams")]
        public TeamCollection Teams { get; set; }
    }

    [JsonArray]
    public class TeamCollection : List<Team>
    {
        public TeamCollection()
            : base()
        {

        }

        public TeamCollection(IEnumerable<Team> teams)
            : base(teams)
        {

        }
    }

    public class Team
    {
        [JsonProperty("_id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("hcp")]
        public float Hcp { get; set; }

        [JsonProperty("teatime")]
        public DateTime TeaTime { get; set; }

        [JsonProperty("members")]
        public TournamentParticipantCollection Members { get; set; }
    }
}