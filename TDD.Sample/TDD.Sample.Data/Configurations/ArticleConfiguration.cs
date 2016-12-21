using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TDD.Sample.Domain;

namespace TDD.Sample.Data
{
    public class ArticleConfiguration : EntityTypeConfiguration<Article>
    {
        public ArticleConfiguration()
        {
            ToTable("Article");
            Property(a => a.Title).IsRequired().HasMaxLength(100);
            Property(a => a.Contents).IsRequired();
            Property(a => a.Author).IsRequired().HasMaxLength(50);
            Property(a => a.URL).IsRequired().HasMaxLength(200);
            Property(a => a.DateCreated).HasColumnType("datetime2");
            Property(a => a.DateEdited).HasColumnType("datetime2");
        }
    }
}
