using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace AF.Core.Extensions
{
    public static class JsonExtensions
    {

        /// <summary>
        /// Json2s the object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jsonstr">The jsonstr.</param>
        /// <returns></returns>
        public static T Json2Obj<T>(this string jsonstr)
        {
            return JsonConvert.DeserializeObject<T>(jsonstr);
        }


        /// <summary>
        /// Obj2s the json.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj">The object.</param>
        /// <returns></returns>
        public static string Obj2Json<T>(this T obj)
        {
            return JsonConvert.SerializeObject(obj);
        }
    }
}
