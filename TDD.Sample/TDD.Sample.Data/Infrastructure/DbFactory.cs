using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TDD.Sample.Data;

namespace TDD.Sample.Data
{
    public class DbFactory : Disposable, IDbFactory
    {
        BloggerEntities dbContext;

        public BloggerEntities Init()
        {
            return dbContext ?? (dbContext = new BloggerEntities());
        }

        protected override void DisposeCore()
        {
            if (dbContext != null)
                dbContext.Dispose();
        }
    }
}
