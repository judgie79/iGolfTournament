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
}