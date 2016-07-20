﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Golf.Tournament.Models
{
    public class PlayerEditViewModel
    {
        public PlayerEditViewModel()
        {
            Player = new Player();
        }

        public Player Player { get; set; }

        public IEnumerable<Club> Clubs { get; set; }
    }
}