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
    public class PlayerCollection : List<Player>
    {
        public PlayerCollection()
            : base()
        {

        }

        public PlayerCollection(IEnumerable<Player> players)
            : base(players)
        {

        }
    }
}