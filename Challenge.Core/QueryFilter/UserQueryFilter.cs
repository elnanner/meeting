using System;
using System.Collections.Generic;
using System.Text;

namespace Challenge.Core.QueryFilter
{
    public class UserQueryFilter : BaseQueryFilter
    {
        public string Name { get; set; }

        public string Email { get; set; }

        public string Role { get; set; }
    }
}
