using Golf.Tournament.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Golf.Tournament.ViewModels
{
    public class ScoresheetViewModel
    {
        public ScoresheetViewModel(TeeBox teeBox)
        {
            this.TeeBox = teeBox;
        }

        public TeeBox TeeBox { get; private set; }
    }
}