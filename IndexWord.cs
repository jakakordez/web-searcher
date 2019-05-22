using System;
using System.Collections.Generic;

namespace WebSearcher
{
    public partial class IndexWord
    {
        public IndexWord()
        {
            Posting = new HashSet<Posting>();
        }

        public string Word { get; set; }

        public virtual ICollection<Posting> Posting { get; set; }
    }
}
