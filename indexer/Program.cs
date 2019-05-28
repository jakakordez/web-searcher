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
            var words = new List<string>(args);
            words.RemoveAt(0);
            var basePath = "../../../sites/";
            switch (args[0])
            {
                case "i":
                    Indexer indexer = new Indexer(basePath);
                    indexer.Start(db, null);
                    db.SaveChanges();
                    break;
                case "q":
                    new IndexedSearcher(basePath, db)
                        .Query(words);
                    break;
                case "d":
                    new DirectSearcher(basePath)
                        .Query(words);
                    break;
                default:
                    Console.WriteLine("WebSearcher");
                    Console.WriteLine("i - Indexing");
                    Console.WriteLine("q - Query with provided index");
                    Console.WriteLine("d - Query directly without index");
                    break;
            }
            Console.WriteLine("Program finished, press Enter to quit");
            Console.Read();
        }
    }
}
