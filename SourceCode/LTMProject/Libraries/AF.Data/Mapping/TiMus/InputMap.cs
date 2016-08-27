using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AF.Domain.Domain.TiMus;

namespace AF.Data.Mapping.TiMus
{
    public class InputMap : AFEntityTypeConfiguration<Input>
    {
        public InputMap()
        {
            this.ToTable("Input");
            this.HasKey(d => d.Id);
            this.HasRequired(d => d.TiMu).WithMany(d => d.Inputs).HasForeignKey(d => d.TmId);
        }
    }
}
