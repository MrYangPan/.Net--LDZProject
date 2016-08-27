using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using AF.Domain.Domain.Customers;

namespace AF.Data.Mapping.Customers
{
    public partial class CustomerRoleMap : AFEntityTypeConfiguration<CustomerRole>
    {
        public CustomerRoleMap()
        {
            this.ToTable("CustomerRole");
            this.HasKey(a => a.Id);
            this.Property(c => c.Name).HasMaxLength(255);
            this.Property(c => c.SystemName).HasMaxLength(255);
        }
    }
}
