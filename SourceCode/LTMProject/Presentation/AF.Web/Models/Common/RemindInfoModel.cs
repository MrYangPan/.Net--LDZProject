using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AF.Domain.Domain.BookWork;

namespace AF.Web.Models.Common
{
    public class RemindInfoModel
    {
        public IList<BookWorkTask> Tasks { get; set; } 
    }
}