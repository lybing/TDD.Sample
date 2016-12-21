using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TDD.Sample.Domain;

namespace TDD.Sample.Data
{
    public class BlogConfiguration : EntityTypeConfiguration<Blog>
    {
        public BlogConfiguration()
        {
            ToTable("Blog");
            Property(b => b.Name).IsRequired().HasMaxLength(100);
            Property(b => b.URL).IsRequired().HasMaxLength(200);
            Property(b => b.Owner).IsRequired().HasMaxLength(50);
            Property(b => b.DateCreated).HasColumnType("datetime2");
        }
    }
}
