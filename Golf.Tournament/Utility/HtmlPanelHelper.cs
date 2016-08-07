using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.WebPages;

namespace Golf.Tournament.Utility
{
    public static class HtmlPanelHelper
    {
        public static MvcHtmlString Panel(this HtmlHelper helper, string panelTitle, Func<object, HelperResult> body, Func<object, HelperResult> footer = null, string panelClass = "panel-default", bool isCollapsible = false, bool isCollapsed = false)
        {
            return Panel(helper, panelTitle, body.Invoke(null).ToHtmlString(), footer.Invoke(null).ToHtmlString(), panelClass, isCollapsible, isCollapsed);
        }

        public static MvcHtmlString Panel(this HtmlHelper helper, string panelTitle, string body, Func<object, HelperResult> footer = null, string panelClass = "panel-default", bool isCollapsible = false, bool isCollapsed = false)
        {
            return Panel(helper, panelTitle, body, footer.Invoke(null).ToHtmlString(), panelClass, isCollapsible, isCollapsed);
        }

        public static MvcHtmlString Panel(this HtmlHelper helper, string panelTitle, Func<object, HelperResult> body, string footer = null, string panelClass = "panel-default", bool isCollapsible = false, bool isCollapsed = false)
        {
            return Panel(helper, panelTitle, body.Invoke(null).ToHtmlString(), footer, panelClass, isCollapsible, isCollapsed);
        }

        public static MvcHtmlString Panel(this HtmlHelper helper, string panelTitle, MvcHtmlString body, MvcHtmlString footer = null, string panelClass = "panel-default", bool isCollapsible = false, bool isCollapsed = false)
        {
            return Panel(helper, panelTitle, body.ToHtmlString(), footer.ToHtmlString(), panelClass, isCollapsible, isCollapsed);
        }

        public static MvcHtmlString Panel(this HtmlHelper helper, string panelTitle, Func<object, HelperResult> body, MvcHtmlString footer = null, string panelClass = "panel-default", bool isCollapsible = false, bool isCollapsed = false)
        {
            return Panel(helper, panelTitle, body.Invoke(null).ToHtmlString(), footer.ToHtmlString(), panelClass, isCollapsible, isCollapsed);
        }

        public static MvcHtmlString Panel(this HtmlHelper helper, string panelTitle, MvcHtmlString body, Func<object, HelperResult> footer = null, string panelClass = "panel-default", bool isCollapsible = false, bool isCollapsed = false)
        {
            return Panel(helper, panelTitle, body.ToHtmlString(), footer.Invoke(null).ToHtmlString(), panelClass, isCollapsible, isCollapsed);
        }

        public static MvcHtmlString Panel(this HtmlHelper helper, string panelTitle, string body, MvcHtmlString footer = null, string panelClass = "panel-default", bool isCollapsible = false, bool isCollapsed = false)
        {
            return Panel(helper, panelTitle, body, footer.ToHtmlString(), panelClass, isCollapsible, isCollapsed);
        }

        public static MvcHtmlString Panel(this HtmlHelper helper, string panelTitle, MvcHtmlString body, string footer = null, string panelClass = "panel-default", bool isCollapsible = false, bool isCollapsed = false)
        {
            return Panel(helper, panelTitle, body.ToHtmlString(), footer, panelClass, isCollapsible, isCollapsed);
        }

        /////

        public static MvcHtmlString Panel(this HtmlHelper helper, Func<object, HelperResult> panelTitle, Func<object, HelperResult> body, Func<object, HelperResult> footer = null, string panelClass = "panel-default", bool isCollapsible = false, bool isCollapsed = false)
        {
            return Panel(helper, panelTitle.Invoke(null).ToHtmlString(), body.Invoke(null).ToHtmlString(), footer.Invoke(null).ToHtmlString(), panelClass, isCollapsible, isCollapsed);
        }

        public static MvcHtmlString Panel(this HtmlHelper helper, Func<object, HelperResult> panelTitle, string body, Func<object, HelperResult> footer = null, string panelClass = "panel-default", bool isCollapsible = false, bool isCollapsed = false)
        {
            return Panel(helper, panelTitle.Invoke(null).ToHtmlString(), body, footer.Invoke(null).ToHtmlString(), panelClass, isCollapsible, isCollapsed);
        }

        public static MvcHtmlString Panel(this HtmlHelper helper, Func<object, HelperResult> panelTitle, Func<object, HelperResult> body, string footer = null, string panelClass = "panel-default", bool isCollapsible = false, bool isCollapsed = false)
        {
            return Panel(helper, panelTitle.Invoke(null).ToHtmlString(), body.Invoke(null).ToHtmlString(), footer, panelClass, isCollapsible, isCollapsed);
        }

        public static MvcHtmlString Panel(this HtmlHelper helper, Func<object, HelperResult> panelTitle, MvcHtmlString body, MvcHtmlString footer = null, string panelClass = "panel-default", bool isCollapsible = false, bool isCollapsed = false)
        {
            return Panel(helper, panelTitle.Invoke(null).ToHtmlString(), body.ToHtmlString(), footer.ToHtmlString(), panelClass, isCollapsible, isCollapsed);
        }

        public static MvcHtmlString Panel(this HtmlHelper helper, Func<object, HelperResult> panelTitle, Func<object, HelperResult> body, MvcHtmlString footer = null, string panelClass = "panel-default", bool isCollapsible = false, bool isCollapsed = false)
        {
            return Panel(helper, panelTitle.Invoke(null).ToHtmlString(), body.Invoke(null).ToHtmlString(), footer.ToHtmlString(), panelClass, isCollapsible, isCollapsed);
        }

        public static MvcHtmlString Panel(this HtmlHelper helper, Func<object, HelperResult> panelTitle, MvcHtmlString body, Func<object, HelperResult> footer = null, string panelClass = "panel-default", bool isCollapsible = false, bool isCollapsed = false)
        {
            return Panel(helper, panelTitle.Invoke(null).ToHtmlString(), body.ToHtmlString(), footer.Invoke(null).ToHtmlString(), panelClass, isCollapsible, isCollapsed);
        }

        public static MvcHtmlString Panel(this HtmlHelper helper, Func<object, HelperResult> panelTitle, string body, MvcHtmlString footer = null, string panelClass = "panel-default", bool isCollapsible = false, bool isCollapsed = false)
        {
            return Panel(helper, panelTitle.Invoke(null).ToHtmlString(), body, footer.ToHtmlString(), panelClass, isCollapsible, isCollapsed);
        }

        public static MvcHtmlString Panel(this HtmlHelper helper, Func<object, HelperResult> panelTitle, MvcHtmlString body, string footer = null, string panelClass = "panel-default", bool isCollapsible = false, bool isCollapsed = false)
        {
            return Panel(helper, panelTitle.Invoke(null).ToHtmlString(), body.ToHtmlString(), footer, panelClass, isCollapsible, isCollapsed);
        }

        ////

        public static MvcHtmlString Panel(this HtmlHelper helper, MvcHtmlString panelTitle, Func<object, HelperResult> body, Func<object, HelperResult> footer = null, string panelClass = "panel-default", bool isCollapsible = false, bool isCollapsed = false)
        {
            return Panel(helper, panelTitle.ToHtmlString(), body.Invoke(null).ToHtmlString(), footer.Invoke(null).ToHtmlString(), panelClass, isCollapsible, isCollapsed);
        }

        public static MvcHtmlString Panel(this HtmlHelper helper, MvcHtmlString panelTitle, string body, Func<object, HelperResult> footer = null, string panelClass = "panel-default", bool isCollapsible = false, bool isCollapsed = false)
        {
            return Panel(helper, panelTitle.ToHtmlString(), body, footer.Invoke(null).ToHtmlString(), panelClass, isCollapsible, isCollapsed);
        }

        public static MvcHtmlString Panel(this HtmlHelper helper, MvcHtmlString panelTitle, Func<object, HelperResult> body, string footer = null, string panelClass = "panel-default", bool isCollapsible = false, bool isCollapsed = false)
        {
            return Panel(helper, panelTitle.ToHtmlString(), body.Invoke(null).ToHtmlString(), footer, panelClass, isCollapsible, isCollapsed);
        }

        public static MvcHtmlString Panel(this HtmlHelper helper, MvcHtmlString panelTitle, MvcHtmlString body, MvcHtmlString footer = null, string panelClass = "panel-default", bool isCollapsible = false, bool isCollapsed = false)
        {
            return Panel(helper, panelTitle.ToHtmlString(), body.ToHtmlString(), footer.ToHtmlString(), panelClass, isCollapsible, isCollapsed);
        }

        public static MvcHtmlString Panel(this HtmlHelper helper, MvcHtmlString panelTitle, Func<object, HelperResult> body, MvcHtmlString footer = null, string panelClass = "panel-default", bool isCollapsible = false, bool isCollapsed = false)
        {
            return Panel(helper, panelTitle.ToHtmlString(), body.Invoke(null).ToHtmlString(), footer.ToHtmlString(), panelClass, isCollapsible, isCollapsed);
        }

        public static MvcHtmlString Panel(this HtmlHelper helper, MvcHtmlString panelTitle, MvcHtmlString body, Func<object, HelperResult> footer = null, string panelClass = "panel-default", bool isCollapsible = false, bool isCollapsed = false)
        {
            return Panel(helper, panelTitle.ToHtmlString(), body.ToHtmlString(), footer.Invoke(null).ToHtmlString(), panelClass, isCollapsible, isCollapsed);
        }

        public static MvcHtmlString Panel(this HtmlHelper helper, MvcHtmlString panelTitle, string body, MvcHtmlString footer = null, string panelClass = "panel-default", bool isCollapsible = false, bool isCollapsed = false)
        {
            return Panel(helper, panelTitle.ToHtmlString(), body, footer.ToHtmlString(), panelClass, isCollapsible, isCollapsed);
        }

        public static MvcHtmlString Panel(this HtmlHelper helper, MvcHtmlString panelTitle, MvcHtmlString body, string footer = null, string panelClass = "panel-default", bool isCollapsible = false, bool isCollapsed = false)
        {
            return Panel(helper, panelTitle.ToHtmlString(), body.ToHtmlString(), footer, panelClass, isCollapsible, isCollapsed);
        }

        /* HTML Syntax
        <div class="panel panel-default panel-collapsed">
            <div class="panel-heading">
                <h3 class="panel-title"></h3>
                <span class="pull-right clickable"><i class="glyphicon glyphicon-chevron-down"></i></span>
            </div>
            <div class="panel-body">
            </div>
            <div class="panel-footer">
            </div>
        </div>
        */
        public static MvcHtmlString Panel(this HtmlHelper helper, string panelTitle, string body, string footer = null, string panelClass = "panel-default", bool isCollapsible = false, bool isCollapsed = false)
        {
            StringBuilder sb = new StringBuilder();

            
            string panelClassStr = string.Format(" {0}", panelClass);
            string panelCollapsedStr = isCollapsible && isCollapsed ? " panel-collapsed" : "";

            //panel
            sb.AppendFormat("<div class=\"panel{0}{1}\">\n", panelClassStr, panelCollapsedStr);

            //  panelHeader
            sb.Append("   <div class=\"panel-heading\">\n");
            sb.AppendFormat("       <h3 class=\"panel-title\">{0}</h3>\n", panelTitle);
            if (isCollapsible) {
                sb.Append("       <span class=\"pull-right clickable\">");
                
                if (isCollapsed)
                    sb.Append("<i class=\"glyphicon glyphicon-chevron-down\"></i></span>\n");
                else
                    sb.Append("<i class=\"glyphicon glyphicon-chevron-up\"></i></span>\n");
            }
            sb.Append("   </div>\n");
            //  panelHeader

            //panel body
            sb.Append("   <div class=\"panel-body\">\n");
            sb.Append(body);
            sb.Append("   </div>\n");

            if (footer != null)
            {
                sb.Append("   <div class=\"panel-footer\">\n");
                sb.Append(footer);
                sb.Append("   </div>\n");
            }

            sb.AppendFormat("</div>\n");

            return MvcHtmlString.Create(sb.ToString());
        }
    }
}