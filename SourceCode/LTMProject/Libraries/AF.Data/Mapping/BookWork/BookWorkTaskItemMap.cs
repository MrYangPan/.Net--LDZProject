using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AF.Domain.Domain.BookWork;

namespace AF.Data.Mapping.BookWork
{
    public class BookWorkTaskItemMap : AFEntityTypeConfiguration<BookWorkTaskItem>
    {
        public BookWorkTaskItemMap()
        {
            this.ToTable("BookWorkTaskItem");
            this.HasKey(m => m.Id);
            this.HasRequired(t => t.BookWorkTask).WithMany(d => d.BookWorkTaskItems).HasForeignKey(d => d.TaskId);
            this.HasRequired(d => d.BookChapter).WithMany().HasForeignKey(d => d.ChapterId);
        }
    }
}
