using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Golf.Tournament.ViewModels
{
    public class FileUploadViewModel
    {
        public FileUploadViewModel()
        {

        }
        public string Id { get; set; }

        public string Name { get; set; }

        public dynamic HtmlAttributes { get; internal set; }
    }
}