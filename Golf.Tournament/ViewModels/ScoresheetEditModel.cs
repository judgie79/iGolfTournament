using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Golf.Tournament.Models
{
    public class ScoresheetEditModel
    {
        public ScoresheetEditModel(TeeBox teeBox)
        {
            this.TeeBox = teeBox;
        }

        public TeeBox TeeBox { get; private set; }
    }
}