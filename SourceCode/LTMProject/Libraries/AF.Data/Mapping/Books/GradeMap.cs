using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AF.Domain.Domain.Books;

namespace AF.Data.Mapping.Books
{
    public class GradeMap : AFEntityTypeConfiguration<Grade>
    {
        public GradeMap()
        {
            this.ToTable("Grade");
            this.HasKey(m => m.Id);
        }
    }
}
