using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Http.Dispatcher;
using TDD.Sample.API.Core.Controllers;

namespace TDD.Sample.API.Core
{
    public class CustomAssembliesResolver : DefaultAssembliesResolver
    {
        public override ICollection<Assembly> GetAssemblies()
        {
            var baseAssemblies = base.GetAssemblies().ToList();
            var assemblies = new List<Assembly>(baseAssemblies) { typeof(BlogsController).Assembly };
            baseAssemblies.AddRange(assemblies);

            return baseAssemblies.Distinct().ToList();
        }
    }
}