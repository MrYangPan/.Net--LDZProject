using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.UI.WebControls;

namespace AF.Web.Framework.UI.MVC
{
    public static class HtmlExtension
    {

        #region Public Mehod

        /// <summary>
        /// Enums the radio list for.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <typeparam name="TEnum">The type of the enum.</typeparam>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="expression">The expression.</param>
        /// <returns></returns>
        public static MvcHtmlString EnumRadioListFor<TModel, TEnum>(this HtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TEnum>> expression)
        {
            if (expression == null)
            {
                throw new ArgumentNullException("expression");
            }

            ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            if (metadata == null)
            {
                throw new ArgumentException("expression");
            }

            if (metadata.ModelType == null)
            {
                throw new ArgumentException("expression");
            }

            if (!EnumHelper.IsValidForEnumHelper(metadata.ModelType))
            {
                throw new ArgumentException("expression");
            }

            string expressionName = ExpressionHelper.GetExpressionText(expression);
            Enum currentValue = htmlHelper.ViewData.Eval(expressionName) as Enum;
            var type = metadata.ModelType;
            IList<SelectListItem> selectList = GetSelectList(type);
            Type valueType = currentValue?.GetType();
            if (valueType != null && valueType != type && valueType != Nullable.GetUnderlyingType(type))
            {
                throw new ArgumentException("value");
            }
            IList<MvcHtmlString> mvcHtmlStrings = new List<MvcHtmlString>();
            if (selectList.Count != 0)
            {
                foreach (var itme in selectList)
                {
                    var id = expressionName + itme.Value;
                    mvcHtmlStrings.Add(htmlHelper.RadioButton(expressionName, itme.Value,
                        (currentValue != null) && (currentValue.ToString("d") == itme.Value),
                        new {id}));
                    mvcHtmlStrings.Add(htmlHelper.LabelForModel(itme.Text, new {@for = id}));
                }
            }
            return new MvcHtmlString(String.Concat(mvcHtmlStrings));
        }

        #endregion

        #region Utinitiles





        /// <summary>
        /// Gets the select list.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public static IList<SelectListItem> GetSelectList(Type type)
        {
            var radion = new RadioButton();
            if (type == null)
            {
                throw new ArgumentException("value");
            }

            if (!EnumHelper.IsValidForEnumHelper(type))
            {
                throw new ArgumentException("type");
            }

            IList<SelectListItem> selectList = new List<SelectListItem>();
            Type checkedType = Nullable.GetUnderlyingType(type) ?? type;
            // Populate the list
            const BindingFlags BindingFlags =
                BindingFlags.DeclaredOnly | BindingFlags.GetField | BindingFlags.Public | BindingFlags.Static;
            foreach (FieldInfo field in checkedType.GetFields(BindingFlags))
            {
                // fieldValue will be an numeric type (byte, ...)
                object fieldValue = field.GetRawConstantValue();

                selectList.Add(new SelectListItem {Text = GetDisplayName(field), Value = fieldValue.ToString(),});
            }

            return selectList;
        }


        /// <summary>
        /// Gets the display name.
        /// </summary>
        /// <param name="field">The field.</param>
        /// <returns></returns>
        private static string GetDisplayName(FieldInfo field)
        {
            DisplayAttribute display = field.GetCustomAttribute<DisplayAttribute>(inherit: false);
            if (display != null)
            {
                string name = display.GetName();
                if (!String.IsNullOrEmpty(name))
                {
                    return name;
                }
            }

            return field.Name;
        }


        #endregion

    }
}
