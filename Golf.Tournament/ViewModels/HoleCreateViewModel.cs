﻿using Golf.Tournament.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Golf.Tournament.ViewModels
{
    public class HoleCreateViewModel
    {
        public HoleCreateViewModel()
        {
            Hole = new Hole();
        }

        public Club Club { get; set; }

        public Hole Hole { get; set; }
    }
}