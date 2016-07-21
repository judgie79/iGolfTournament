using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Golf.Tournament.Utility
{
    public static class SectionHelpers
    {
        private class ScriptBlock : IDisposable
        {

            private string scriptsKey = "scripts";
            public static List<string> pageScripts(string scriptsKey)
            {
                if (HttpContext.Current.Items[scriptsKey] == null)
                    HttpContext.Current.Items[scriptsKey] = new List<string>();
                return (List<string>)HttpContext.Current.Items[scriptsKey];
            }

            WebViewPage webPageBase;

            public ScriptBlock(WebViewPage webPageBase, string _scriptsKey)
            {
                this.webPageBase = webPageBase;
                this.webPageBase.OutputStack.Push(new StringWriter());
                scriptsKey = _scriptsKey;
            }

            public void Dispose()
            {
                pageScripts(scriptsKey).Add(((StringWriter)this.webPageBase.OutputStack.Pop()).ToString());
            }
        }

        private class ContentBlock : IDisposable
        {
            private string pageContentKey = "pageContent";
            public static List<string> pageContent(string pageContentKey)
            {

                if (HttpContext.Current.Items[pageContentKey] == null)
                    HttpContext.Current.Items[pageContentKey] = new List<string>();
                return (List<string>)HttpContext.Current.Items[pageContentKey];
            }

            WebViewPage webPageBase;

            public ContentBlock(WebViewPage webPageBase, string pageContentKey)
            {
                this.webPageBase = webPageBase;
                this.webPageBase.OutputStack.Push(new StringWriter());
                this.pageContentKey = pageContentKey;
            }

            public void Dispose()
            {
                pageContent(pageContentKey).Add(((StringWriter)this.webPageBase.OutputStack.Pop()).ToString());
            }
        }

        public static IDisposable BeginScripts(this HtmlHelper helper, string scriptKey)
        {
            return new ScriptBlock((WebViewPage)helper.ViewDataContainer, scriptKey);
        }

        public static MvcHtmlString PageScripts(this HtmlHelper helper, string scriptKey)
        {
            return MvcHtmlString.Create(string.Join(Environment.NewLine, ScriptBlock.pageScripts(scriptKey).Select(s => s.ToString())));
        }

        public static IDisposable BeginContent(this HtmlHelper helper, string pageContentKey)
        {
            return new ContentBlock((WebViewPage)helper.ViewDataContainer, pageContentKey);
        }

        public static MvcHtmlString WriteContent(this HtmlHelper helper, string pageContentKey)
        {
            return MvcHtmlString.Create(string.Join(Environment.NewLine, ContentBlock.pageContent(pageContentKey).Select(s => s.ToString())));
        }
    }
}