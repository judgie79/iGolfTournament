using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;

namespace Golf.Tournament.ViewModels
{
    public class DashboardListPanelViewModel<TModel> : DashboardListPanelViewModel
    {
        public DashboardListPanelViewModel(IEnumerable<TModel> dataModel)
            : base(dataModel.Cast<object>())
        {
            DataModel = dataModel;
        }

        private IEnumerable<TModel> dataModel;
        public new IEnumerable<TModel> DataModel
        {
            get
            {
                return dataModel;
            }
            set
            {
                dataModel = value;
                base.DataModel = value.Cast<object>();
            }
        }

        private Expression<Func<TModel, string>> key;
        public new Expression<Func<TModel, string>> Key
        {
            get
            {
                return key;
            }
            set
            {
                key = value;

                ParameterExpression p = Expression.Parameter(typeof(object));

                base.Key = Expression.Lambda<Func<object, string>>
                (
                    Expression.Invoke(value, Expression.Convert(p, typeof(TModel))),
                    p
                );
            }
        }
        private Expression<Func<TModel, string>> title;
        public new Expression<Func<TModel, string>> Title
        {
            get
            {
                return title;
            }
            set
            {
                title = value;

                ParameterExpression p = Expression.Parameter(typeof(object));

                base.Title = Expression.Lambda<Func<object, string>>
                (
                    Expression.Invoke(value, Expression.Convert(p, typeof(TModel))),
                    p
                );
            }
        }

        private Expression<Func<TModel, string>> info;
        public new Expression<Func<TModel, string>> Info
        {
            get
            {
                return info;
            }
            set
            {
                info = value;

                ParameterExpression p = Expression.Parameter(typeof(object));

                base.Info = Expression.Lambda<Func<object, string>>
                (
                    Expression.Invoke(value, Expression.Convert(p, typeof(TModel))),
                    p
                );
            }
        }

        public string GetValue(TModel item, Expression<Func<TModel, string>> selector)
        {
            return selector.Compile().Invoke(item);
        }
    }

    public class DashboardListPanelViewModel
    {
        public DashboardListPanelViewModel(IEnumerable<object> dataModel)
        {
            DataModel = dataModel;
        }

        public IEnumerable<object> DataModel { get; set; }

        public Expression<Func<object, string>> Key { get; set; }
        public Expression<Func<object, string>> Title { get; set; }
        public Expression<Func<object, string>> Info { get; set; }

        public string ListTitleIcon { get; set; }
        public string ListIcon { get; set; }
        public string ListTitle { get; set; }

        public MvcHtmlString Link { get; set; }

        public string GetValue(object item, Expression<Func<object, string>> selector)
        {
            return selector.Compile().Invoke(item);
        }
    }
}