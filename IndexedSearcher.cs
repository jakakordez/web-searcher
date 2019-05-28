using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebSearcher
{
    class IndexedSearcher : Searcher
    {
        sqliteContext db;

        public IndexedSearcher(string basePath, sqliteContext db):base(basePath)
        {
            this.db = db;
        }

        public override IQueryable<Posting> Find(List<string> words)
        {
            return db.Posting
                .Where(p => words.Contains(p.Word));
        }
    }
}
