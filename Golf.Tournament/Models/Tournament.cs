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
        public string Title { get; set; }

        [JsonProperty("participants")]
        public IEnumerable<TournamentParticipant> Participants { get; set; }

        [JsonProperty("club")]
        public Club Club { get; set; }

        [JsonProperty("course")]
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

        [JsonProperty("teaTime")]
        public DateTime TeaTime { get; set; }
    }

    public class TeamTournament : Tournament
    {
        public IEnumerable<Team> Teams { get; set; }
    }

    public class Team
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public float Hcp { get; set; }

        public DateTime TeaTime { get; set; }

        public IEnumerable<TournamentParticipant> Members { get; set; }
    }
}