using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Golf.Tournament.Utility
{
    public static class FileHelper
    {
        public static bool HasFile(this HttpPostedFileBase file)
        {
            return file != null && file.ContentLength > 0;
        }
    }
}