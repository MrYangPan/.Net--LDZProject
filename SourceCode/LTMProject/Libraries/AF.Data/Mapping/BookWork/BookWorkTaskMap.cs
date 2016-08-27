using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AF.Domain.Domain.BookWork;

namespace AF.Data.Mapping.BookWork
{
    public class BookWorkTaskMap : AFEntityTypeConfiguration<BookWorkTask>
    {
        public BookWorkTaskMap()
        {
            this.ToTable("BookWorkTask");
            this.HasKey(m => m.Id);
            this.HasRequired(d => d.Book).WithMany().HasForeignKey(d => d.BookId);
            this.HasRequired(d => d.CheckCustomer).WithMany().HasForeignKey(d => d.CheckCustomerId);
            this.HasRequired(d => d.EntryCustomer).WithMany().HasForeignKey(d => d.EntryCustomerId);
            this.HasRequired(d => d.MarkCustomer).WithMany().HasForeignKey(d => d.MarkCustomerId);
        }
    }
}
