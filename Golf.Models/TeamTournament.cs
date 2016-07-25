using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Golf.Tournament.Models
{
    public class TeamTournament : Tournament
    {
        [JsonProperty("teams")]
        public TeamCollection Teams { get; set; }
    }
}