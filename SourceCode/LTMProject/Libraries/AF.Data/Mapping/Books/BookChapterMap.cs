using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AF.Domain.Domain.Books;

namespace AF.Data.Mapping.Books
{
    public class BookChapterMap : AFEntityTypeConfiguration<BookChapter>
    {
        public BookChapterMap()
        {
            this.ToTable("BookChapter");
            this.HasKey(m => m.Id);
            this.HasOptional(d => d.BookChapterParent).WithMany(d => d.BookChapterChild).HasForeignKey(d=>d.ParentId);
            this.HasRequired(d => d.UploadFile).WithMany().HasForeignKey(d => d.WordFileId);
            this.HasOptional(d => d.BookWorkTaskItem).WithMany().HasForeignKey(d => d.TaskItemId);
        }
    }
}
