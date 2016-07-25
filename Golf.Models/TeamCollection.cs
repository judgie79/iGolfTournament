using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Golf.Tournament.Models
{
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
}