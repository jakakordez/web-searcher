using System;
using System.Collections.Generic;

namespace WebSearcher
{
    public partial class Posting
    {
        public string Word { get; set; }
        public string DocumentName { get; set; }
        public long Frequency { get; set; }
        public string Indexes { get; set; }

        public virtual IndexWord WordNavigation { get; set; }
    }
}
