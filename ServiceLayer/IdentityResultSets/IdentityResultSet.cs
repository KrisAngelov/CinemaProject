using BusinessLayer;
using DataLayer;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.IdentityResultSets
{
    public class CustomerResultSet : IdentityResultSet<User>
    {
        public CustomerResultSet(IdentityResult identityResult, User entity) : base(identityResult, entity)
        {
        }

    }
}
