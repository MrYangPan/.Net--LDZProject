using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AF.Domain.Domain.Media;

namespace AF.Data.Mapping.Media
{

    public class FileMap : AFEntityTypeConfiguration<File>
    {
        public FileMap()
        {
            this.ToTable("File");
            this.HasKey(m => m.Id);
        }
    }
}
