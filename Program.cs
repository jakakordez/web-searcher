using System;
using System.IO;

namespace WebSearcher
{
    class Program
    {
        static void Main(string[] args)
        {
            var db = new sqliteContext();

           

            Indexer indexer = new Indexer("../../../sites/");
            indexer.Start(db);
            db.SaveChanges();
            Console.WriteLine("Indexer finished!");
        }
    }
}
