using Golf.Tournament.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Golf.Tournament.Utility
{
    public static class HtmlTeeboxHelper
    {
        public static MvcHtmlString Draw(this TeeBox teebox, bool showPar = false)
        {
            var sb = new StringBuilder();

            sb.AppendFormat("<span class=\"label label-default\" style=\"background-color: {0}; color: {1}; display: inline-block; padding: 5px 10px;\">", teebox.Color.ToHtml(), GetContrastColor(teebox.Color.ToHtml()));

            if (showPar)
            {
                string tooltip = string.Format("{0} - Par: {1} CR/SL: {2}/{3} @{4}m", teebox.Name, teebox.Par, teebox.CourseRating, teebox.SlopeRating, teebox.Distance);
                sb.AppendFormat("<span data-toggle=\"tooltip\" data-placement=\"top\" title=\"{1}\">{0}</span>", teebox.Par, tooltip);
            } else
            {
                sb.Append("&nbsp;");
            }

            sb.Append("</span>");

            return new MvcHtmlString(sb.ToString());
        }

        /// <summary>
        /// Get color (black/white) depending on bgColor so it would be clearly seen.
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        public static string GetContrastColor(string color)
        {
            if (string.IsNullOrWhiteSpace(color))
            {
                return BLACK;
            }

            var colorValue = Convert.ToInt32(color.Replace("#", ""), 16);

            if (colorValue > 0xffffff / 2)
            {
                return BLACK;
            } else
            {
                return WHITE;

            }
        }

        private const string BLACK = "#000000";
        private const string WHITE = "#ffffff";
    }
}