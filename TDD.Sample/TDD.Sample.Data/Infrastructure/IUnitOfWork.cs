using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDD.Sample.Data
{
    /// <summary>
    /// Unit of work for transaction
    /// </summary>
    public interface IUnitOfWork
    {
        void Commit();
    }
}
