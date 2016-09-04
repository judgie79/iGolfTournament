using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Golf.Tournament.Models
{
    [JsonArray]
    public class TeeboxCollection : List<TeeBox>
    {
        public TeeboxCollection()
            : base()
        {
           
        }

        public TeeboxCollection(IEnumerable<TeeBox> teeboxes)
            : base(teeboxes)
        {

        }
    }
}