using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AF.Domain.Domain.TiMus;

namespace AF.Data.Mapping.TiMus
{
    public class TiMuTypeMap : AFEntityTypeConfiguration<TiMuType>
    {
        public TiMuTypeMap()
        {
            this.ToTable("TMType");
            this.HasKey(m => m.Id);
            this.HasRequired(d =>d.Subject ).WithMany(d => d.TiMuTypes).HasForeignKey(d => d.SubjectId);
        }
    }
}
