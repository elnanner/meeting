using System;

namespace Challenge.Core.QueryFilter
{
    public class MeetingQueryFilter: BaseQueryFilter
    {
        public int? AdminId { get; set; }

        public DateTime? Date { get; set; }

        public string Description { get; set; }

        //// paginación
        //public int PageSize { get; set; }
        //public int PageNumber { get; set; }


    }
}
