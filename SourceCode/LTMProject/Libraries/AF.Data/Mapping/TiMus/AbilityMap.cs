using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AF.Domain.Domain.TiMus;

namespace AF.Data.Mapping.TiMus
{
    public class AbilityMap : AFEntityTypeConfiguration<Ability>
    {
        public AbilityMap()
        {
            this.ToTable("Ability");
            this.HasKey(m => m.Id);
        }
    }
}
