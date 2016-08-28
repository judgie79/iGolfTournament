using Golf.Tournament.Utility;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Golf.Tournament.Models
{
    public static class HoleCollectionExtensions
    {
        public static HoleCollection ToHoleCollection(this IEnumerable<Hole> holes)
        {
            return new HoleCollection(holes);
        }
    }
}