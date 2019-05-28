using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebSearcher
{
    class DirectSearcher : Searcher
    {
        public DirectSearcher(string basePath) : base(basePath)
        {

        }

        public override IQueryable<Posting> Find(List<string> words)
        {
            return new Indexer(BasePath)
                .Start(null, words)
                .AsQueryable();
        }
    }
}
