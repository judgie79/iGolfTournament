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
            HomeClub = new HomeClub();
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
        public HomeClub HomeClub { get; set; }

        [JsonProperty("membership")]
        public Membership Membership { get; set; }

        [JsonProperty("hcp")]
        [MaxFloatValue(54)]
        [MinFloatValue(0)]
        [Required]
        public float Hcp { get; set; }

        [JsonProperty("overrideHcp")]
        public bool OverrideHcp { get; set; }

        [JsonProperty("isOfficialHcp")]
        public bool IsOfficialHcp { get; set; }

        public string Fullname { get
            {
                return string.Format("{0} {1}", Firstname, Lastname);
            }
        }
    }
}