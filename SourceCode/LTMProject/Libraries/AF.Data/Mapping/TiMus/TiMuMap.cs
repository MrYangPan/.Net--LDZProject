using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AF.Domain.Domain.TiMus;

namespace AF.Data.Mapping.TiMus
{
    public class TiMuMap : AFEntityTypeConfiguration<TiMu>
    {
        public TiMuMap()
        {
            this.ToTable("TiMu");
            this.HasKey(m => m.Id);
            this.HasOptional(m => m.TiMuAttributeExtend).WithRequired(m => m.TiMu);
            this.HasRequired(d => d.Subject);
            this.HasRequired(d => d.TiMuType);
            this.HasRequired(d => d.Ability);
            this.HasRequired(d => d.Grade);
            this.HasOptional(d => d.TiMuParent).WithMany(d => d.TiMuChild).HasForeignKey(d => d.ParentId);
        }
    }
}
