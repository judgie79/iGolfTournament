using Golf.Tournament.Utility;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Golf.Tournament.Models
{
    public class Player
    {
        public Player()
        {
            Address = new Address();
            HomeClub = new Club();
        }

        [JsonProperty("_id")]
        public string Id { get; set; }

        [JsonProperty("firstname")]
        [Required]
        public string Firstname { get; set; }

        [JsonProperty("lastname")]
        [Required]
        public string Lastname { get; set; }

        [JsonProperty("address")]
        public Address Address { get; set; }

        [JsonProperty("homeClub")]
        public Club HomeClub { get; set; }

        [JsonProperty("membership")]
        public Membership Membership { get; set; }

        [JsonProperty("hcp")]
        [MaxValue(54)]
        [MinValue(0)]
        public float Hcp { get; set; }

        [JsonProperty("isOfficialHcp")]
        public bool IsOfficialHcp { get; set; }
    }

    public class Membership
    {
        [JsonProperty("clubNr")]
        [Required]
        public string ClubNr { get; set; }

        [JsonProperty("nr")]
        [Required]
        public string Nr { get; set; }

        [JsonProperty("serviceNr")]
        [Required]
        public string ServiceNr { get; set; }
    }
}