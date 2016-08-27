using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AF.Domain.Domain.Customers;

namespace AF.Data.Mapping.Customers
{
    public partial class PermissionRecordMap :AFEntityTypeConfiguration<PermissionRecord>
    {
        public PermissionRecordMap()
        {
            this.ToTable("PermissionRecord");
            this.HasKey(c => c.Id);

            this.HasMany(c => c.CustomerRoles)
              .WithMany(d=>d.PermissionRecords)
              .Map(m => m.ToTable("PermissionRecord_Role_Mapping"));
        }
    }
}
