using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDD.Sample.Data
{
    public interface IDbFactory : IDisposable
    {
        BloggerEntities Init();
    }
}
