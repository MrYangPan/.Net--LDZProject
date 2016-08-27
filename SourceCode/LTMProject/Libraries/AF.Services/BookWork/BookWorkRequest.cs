using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AF.Services.Books;

namespace AF.Services.BookWork
{
    public class BookWorkRequest : BookRequest
    {
        public int? BookId { get; set; }
    }
}
