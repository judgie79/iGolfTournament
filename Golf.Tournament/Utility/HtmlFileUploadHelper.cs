﻿using Golf.Tournament.ViewModels;
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
    public static class HtmlFileUploadHelper
    {
        public static MvcHtmlString FileUploadFor<TModel, TProperty>(this HtmlHelper<TModel> helper, Expression<Func<TModel, TProperty>> expression, object htmlAttributes = null)
        {
            var metaData = ModelMetadata.FromLambdaExpression(expression, helper.ViewData);
            string id = helper.IdFor(expression).ToHtmlString();
            string name = helper.NameFor(expression).ToHtmlString();
            var editor = new FileUploadViewModel()
            {
                Id = id,
                Name = name,
                HtmlAttributes = htmlAttributes
            };

            return helper.Partial("FileUpload", editor);
        }
    }
}