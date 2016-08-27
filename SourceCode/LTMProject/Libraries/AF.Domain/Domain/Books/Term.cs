using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AF.Core;

namespace AF.Domain.Domain.Books
{

    public class Term : BaseEntity, IKvEntity
    {
        public string Name { get; set; }
        public int Order { get; set; }
        public string Key => Name;
        public string Value => Id.ToString();
    }
}
