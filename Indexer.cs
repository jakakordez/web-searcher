using System;
using System.Collections.Generic;
using System.IO;
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

        public void Start(sqliteContext db)
        {
            Index("e-prostor.gov.si", db);
            Index("e-uprava.gov.si", db);
            Index("evem.gov.si", db);
            Index("podatki.gov.si", db);
        }

        public void Index(string path, sqliteContext db)
        {
            var files = Directory.GetFiles(basePath+path, "*.html");
            Console.WriteLine("Indexing {0} files from {1}", files.Length, path);
            foreach (var file in files)
            {
                var doc = new Document(file, file.Replace(basePath, ""));
                doc.Index(db);
            }
        }
    }
}
