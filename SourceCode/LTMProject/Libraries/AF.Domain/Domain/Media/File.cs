using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AF.Core;

namespace AF.Domain.Domain.Media
{
    public class File : BaseEntity
    {
        public string Name { get; set; }
        public string OriginallName { get; set; }
        public string RelatePath { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
