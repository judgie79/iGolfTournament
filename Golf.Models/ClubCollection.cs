using Golf.Tournament.Utility;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Golf.Tournament.Models
{
    [JsonArray]
    public class ClubCollection : List<Club>
    {
        public ClubCollection()
            : base()
        {
        }

        public ClubCollection(IEnumerable<Club> clubCollection)
            : base(clubCollection)
        {

        }
    }
}