using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AF.Domain.Domain.Books;

namespace AF.Data.Mapping.Books
{
    public class BookTiMuMap : AFEntityTypeConfiguration<BookTiMu>
    {
        public BookTiMuMap()
        {
            this.ToTable("BookTiMu");
            this.HasKey(m => m.Id);
            this.HasRequired(d => d.Timu);
            this.HasRequired(d => d.TaskItem).WithMany().HasForeignKey(d => d.TaskItemId);
            this.HasRequired(t => t.TaskItem).WithMany(d => d.TaskBookTiMus).HasForeignKey(d => d.TaskItemId);
        }
    }
}
