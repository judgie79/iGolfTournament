﻿using Golf.Tournament.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Golf.Tournament.ViewModels
{
    public abstract class CourseViewModel
    {
        public Course Course { get; set; }
    }
}