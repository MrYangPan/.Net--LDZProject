using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AF.Core;

namespace AF.Domain.Domain.Books
{
   public class Publisher: BaseEntity, IKvEntity
    {
       public string Name { get; set; }
        public string Area { get; set; }
        public string ManagerCompany { get; set; }
        public string HostCompany { get; set; }
        public string Type { get; set; }
        public bool Active { get; set; }
       public string Key => Name;
       public string Value => Id.ToString();
    }
}

