using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PracticeConcept.API.Models
{
    public abstract class RequestParameter
    {
        const int maxPageSize = 50;
        public int pageIndex = 1;
        private int _pageSize = 10;

        public int pageSize 
        { 
            get { return _pageSize; } 
            set { _pageSize = (value > maxPageSize) ? maxPageSize : value; } 
        }

        //public int pageNumber { get; set; }
    }
}
