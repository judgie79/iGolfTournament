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
        [Required]
        public Address Address { get; set; }

        [JsonProperty("homeClub")]
        public Club HomeClub { get; set; }
        

        [JsonProperty("hcp")]
        [Required]
        public decimal Hcp { get; set; }
    }
}