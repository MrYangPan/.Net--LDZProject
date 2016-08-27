using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.UI;
using AF.Core;

namespace AF.Web.Framework.UI
{
    public static class HtmlExtensions
    {

        /// <summary>
        /// To the select items.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array">The array.</param>
        /// <returns></returns>
        public static IList<SelectListItem> ToSelectItems<T>(this IEnumerable<T> array) where T : IKvEntity
        {
            IList<SelectListItem> items = new List<SelectListItem>();
            var enumer = array.GetEnumerator();
            while (enumer.MoveNext())
            {
                items.Add(new SelectListItem() {Text = enumer.Current.Key, Value = enumer.Current.Value});
            }
            return items;
        }




    }
}
