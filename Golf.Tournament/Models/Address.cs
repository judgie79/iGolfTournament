using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Golf.Tournament.Models
{
    public class Address
    {
        [JsonProperty("street")]
        public string Street { get; set; }

        [JsonProperty("houseNo")]
        public string HouseNo { get; set; }

        [JsonProperty("zip")]
        public string Zip { get; set; }

        [JsonProperty("city")]
        public string City { get; set; }

        [JsonProperty("country")]
        [StringLength(2, MinimumLength = 2)]
        public string Country { get; set; }
    }
}