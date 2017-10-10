﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlatScraper.Common.Mongo
{
    public class PagedQueryBase
    {
        public int Page { get; set; }
        public int ResultsPerPage { get; set; }
        public string OrderBy { get; set; }
        public string SortOrder { get; set; }

        public FilterQuery Filter { get; set; }
    }
}
