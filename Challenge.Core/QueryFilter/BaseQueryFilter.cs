using System;
using System.Collections.Generic;
using System.Text;

namespace Challenge.Core.QueryFilter
{
    public abstract class BaseQueryFilter
    {
        // paginación
        public int PageSize { get; set; }
        public int PageNumber { get; set; }

    }
}
