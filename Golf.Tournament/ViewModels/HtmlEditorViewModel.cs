using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace Golf.Tournament.ViewModels
{

    public class HtmlEditorViewModel
    {
        public HtmlEditorViewModel()
        {

        }

        public string Text { get; set; }

        public string Id { get; set; }

        public string Name { get; set; }

        public dynamic HtmlAttributes { get; internal set; }
    }
}