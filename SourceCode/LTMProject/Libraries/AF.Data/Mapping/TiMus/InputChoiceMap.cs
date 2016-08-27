using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AF.Domain.Domain.TiMus;

namespace AF.Data.Mapping.TiMus
{
    public class InputChoiceMap : AFEntityTypeConfiguration<InputChoice>
    {
        public InputChoiceMap()
        {
            this.ToTable("InputChoice");
            this.HasKey(d => d.Id);
            this.HasRequired(d => d.Input).WithMany(d => d.InputChoice).HasForeignKey(d => d.InputId);
        }
    }
}
