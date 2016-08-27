using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AF.Web.Extensions
{
    public static class DateTimeExtensions
    {
        public static string FormatDate(this DateTime dt)
        {
            return dt.ToString("yyyy-MM-dd HH:mm:ss");
        }
    }
}