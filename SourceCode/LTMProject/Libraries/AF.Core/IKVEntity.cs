using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AF.Core
{
    public interface IKvEntity
    {
        string Key { get;  }
        string Value { get;  }
    }
}
