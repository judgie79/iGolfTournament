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
            StringBuilder sb = new StringBuilder();

            /*
             <div class="panel panel-default panel-collapsed">
                <div class="panel-heading">
                    <h3 class="panel-title">@Model.Name</h3>
                    <span class="pull-right clickable"><i class="glyphicon glyphicon-chevron-down"></i></span>
                </div>
                <div class="panel-body">
                    <div class="row">
                        <div class="col-md-12">
                            @Html.DisplayFor(model => model.TeeBoxes)
                        </div>
                    </div>
                </div>
                @*<div class="panel-footer">
                </div>*@
            </div>
            */
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
            sb.Append(body.Invoke(null).ToHtmlString());
            sb.Append("   </div>\n");

            if (footer != null)
            {
                sb.Append("   <div class=\"panel-footer\">\n");
                sb.Append(footer.Invoke(null).ToHtmlString());
                sb.Append("   </div>\n");
            }

            sb.AppendFormat("</div>\n");

            return MvcHtmlString.Create(sb.ToString());
        }
    }
}