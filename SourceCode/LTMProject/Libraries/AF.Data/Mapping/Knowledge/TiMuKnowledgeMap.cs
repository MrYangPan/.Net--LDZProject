using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AF.Domain.Domain.Knowledge;

namespace AF.Data.Mapping.Knowledge
{
    public class TiMuKnowledgeMap : AFEntityTypeConfiguration<TiMuKnowledge>
    {
        public TiMuKnowledgeMap()
        {
            this.ToTable("TiMuKnowledge");
            this.HasKey(p => p.Id);
            this.HasRequired(d => d.TiMu).WithMany(d => d.Knowledges).HasForeignKey(d => d.TmId);
            this.HasRequired(d => d.KnowledgePiont).WithMany(d => d.TiMuKnowledge).HasForeignKey(d => d.KnowledgeId);
        }
    }
}
