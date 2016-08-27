using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AF.Domain.Domain.TiMus;

namespace AF.Data.Mapping.TiMus
{
    public class TiMuAttributeExtendMap : AFEntityTypeConfiguration<TiMuAttributeExtend>
    {
        public TiMuAttributeExtendMap()
        {
            this.ToTable("TiMuAttributeExtend");
            this.HasKey(m => m.Id);
        }
    }
}
