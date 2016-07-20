using Golf.Tournament.Utility;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Golf.Tournament.Models
{
    public class Hole
    {
        [JsonProperty("holeId")]
        public string HoleId { get; set; }

        [JsonProperty("number")]
        [Required]
        [MinValue(1)]
        public int Number { get; set; }

        [JsonProperty("distance")]
        [Required]
        [MinValue(0)]
        public int Distance { get; set; }

        [JsonProperty("par")]
        [Required]
        [MinValue(3)]
        [MaxValue(5)]
        public int Par { get; set; }

        [JsonProperty("hcp")]
        [Required]
        [MinValue(1)]
        public int Hcp { get; set; }
    }

    [JsonArray]
    public class HoleCollection: List<Hole>
    {
        public HoleCollection()
            : base()
        {
        }

        public HoleCollection(IEnumerable<Hole> holeCollection)
            : base(holeCollection)
        {

        }
    }

    public static class HoleCollectionExtensions
    {
        public static HoleCollection ToHoleCollection(this IEnumerable<Hole> holes)
        {
            return new HoleCollection(holes);
        }
    }
}