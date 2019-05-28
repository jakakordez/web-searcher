using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace WebSearcher
{
    class Indexer
    {
        string basePath;
        public Indexer(string basePath)
        {
            this.basePath = basePath;
        }

        public IEnumerable<Posting> Start(sqliteContext db, List<string> words)
        {
            return new List<string>(new string[] {
                "e-prostor.gov.si",
                "e-uprava.gov.si",
                "evem.gov.si",
                "podatki.gov.si" })
                .SelectMany(p => Index(p, db, words))
                .ToList();
        }

        public IEnumerable<Posting> Index(string path, sqliteContext db, List<string> words)
        {
            var files = Directory.GetFiles(basePath+path, "*.html");
            if(db != null) Console.WriteLine("Indexing {0} files from {1}", files.Length, path);

            return new List<string>(files)
                .SelectMany(f => new Document(f, f.Replace(basePath, "")).Index(db, words));
        }
    }
}
