using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AF.Web.Models.Manager
{
    public class ChapterViewModel
    {
        public ChapterViewModel()
        {
            Children = new List<ChapterViewModel>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }
        public IList<ChapterViewModel> Children { get; set; }
    }
}