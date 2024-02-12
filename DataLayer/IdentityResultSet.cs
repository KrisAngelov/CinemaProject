using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public class IdentityResultSet<T>
    {
        public IdentityResult IdentityResult { get; private set; }
        public T Entity { get; private set; }

        public IdentityResultSet(IdentityResult identityResult, T entity)
        {
            IdentityResult = identityResult;
            Entity = entity;
        }

    }
}
