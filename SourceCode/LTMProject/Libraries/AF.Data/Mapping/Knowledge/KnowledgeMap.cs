using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AF.Domain.Domain.Knowledge;

namespace AF.Data.Mapping.Knowledge
{
    public class KnowledgeMap : AFEntityTypeConfiguration<KnowledgePiont>
    {
        public KnowledgeMap()
        {
            this.ToTable("Knowledge");
            this.HasKey(m => m.Id);
            this.HasRequired(d => d.Suject).WithMany(d => d.Knowledges).HasForeignKey(d => d.SubjectId);
        }
    }
}
