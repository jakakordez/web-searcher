using System;
using System.Collections.Generic;
using System.IO;

namespace WebSearcher
{
    class Program
    {
        static void Main(string[] args)
        {
            var db = new sqliteContext();

            if (args[0] == "i")
            {
                Indexer indexer = new Indexer("../../../sites/");
                indexer.Start(db);
                db.SaveChanges();
            }
            if(args.Length > 1)
            {
                var searcher = new Searcher("../../../sites/", db);
                var words = new List<string>(args);
                words.RemoveAt(0);
                searcher.Query(words);
            }
            Console.WriteLine("Indexer finished!");
        }
    }
}
