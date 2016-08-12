using Golf.Tournament.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace Golf.Tournament.Utility
{
    public static class WysiwygEditorHelper
    {
        public static MvcHtmlString WysiwygFor<TModel, TProperty>(this HtmlHelper<TModel> helper, Expression<Func<TModel, TProperty>> expression, object htmlAttributes = null)
        {
            var metaData = ModelMetadata.FromLambdaExpression(expression, helper.ViewData);
            string id = helper.IdFor(expression).ToHtmlString();
            string name = helper.NameFor(expression).ToHtmlString();
            var editor = new HtmlEditorViewModel()
            {
                Id = id,
                Name = name,
                Text = metaData.Model.ToString(),
                HtmlAttributes = htmlAttributes
            };

            return helper.Partial("HtmlEditor", editor);
        }
    }
}