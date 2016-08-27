using AF.Domain.Domain.Books;

namespace AF.Data.Mapping.Books
{
    public class BookMap : AFEntityTypeConfiguration<Book>
    {
        public BookMap()
        {
            this.ToTable("Book");
            this.HasKey(m => m.Id);
            this.HasRequired(d => d.Subject).WithMany().HasForeignKey(d => d.SubjectId); ;
            this.HasRequired(d => d.Degree).WithMany().HasForeignKey(d => d.DegreeId); ;
            this.HasRequired(d => d.Grade).WithMany().HasForeignKey(d => d.GradeId); ;
            this.HasRequired(d => d.Publisher).WithMany().HasForeignKey(d => d.PublisherId);
            this.HasRequired(d => d.Term).WithMany().HasForeignKey(d => d.TermId);
        }
    }
}
