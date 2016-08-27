using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AF.Domain.Domain.TiMus;

namespace AF.Data.Mapping.TiMus
{
    public class ShushiTrackMap : AFEntityTypeConfiguration<ShushiTrack>
    {
        public ShushiTrackMap()
        {
            this.ToTable("ShushiTrack");
            this.HasKey(m => m.Id);
        }
    }
}
