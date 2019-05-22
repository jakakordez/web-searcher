using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace WebSearcher
{
    class Searcher
    {
        string basePath;
        sqliteContext db;
        public Searcher(string basePath, sqliteContext db)
        {
            this.basePath = basePath;
            this.db = db;
        }

        public void Query(List<string> words)
        {
            words = words.Select(w => w.ToLower()).ToList();
            var stw = new Stopwatch();
            stw.Start();
            var postings = db.Posting
                .Where(p => words.Contains(p.Word))
                .GroupBy(p => p.DocumentName)
                .Select(g => new {
                    Document = g.Key,
                    Frequency = g.Sum(p => p.Frequency),
                    Indices = g.SelectMany(s => s.Indexes.Split(',', StringSplitOptions.None))
                            .Select(s => Convert.ToInt32(s))
                            .OrderBy(s => s)
                            .ToList()
                })
                .OrderBy(p => -p.Frequency)
                .ToList();
            stw.Stop();

            Console.WriteLine("Results for a query: \"{0}\"", string.Join(" ", words));
            Console.WriteLine();
            Console.WriteLine("Results found in {0}ms.", stw.ElapsedMilliseconds);
            Console.WriteLine();
            Console.WriteLine("Frequencies Document                                  Snippet");
            Console.WriteLine("----------- ----------------------------------------- -----------------------------------------------------------");
            foreach (var posting in postings)
            {

                Console.WriteLine("{0,-12}{1,-42}{2}", 
                    posting.Frequency, 
                    posting.Document, 
                    GetSnippet(posting.Document, posting.Indices));
            }
           
        }

        private string GetSnippet(string document, List<int> indices)
        {
            var doc = new Document(basePath+document, document);
            var tokens = new Regex("\\w{2,}").Matches(doc.Text);
            int idx = indices[0]-1;
            StringBuilder sb = new StringBuilder();
            
            bool dots = false;
            for (int i = 0; i < tokens.Count; i++)
            {
                var token = tokens[i].ToString();
                int distance = Math.Abs(i-idx);
                if(distance == 3 && i < idx && !dots)
                {
                    sb.Append("... ");
                    dots = true;
                }
                if (distance < 3) {
                    sb.Append(token);
                    sb.Append(" ");
                    dots = false;
                }
                else if(distance == 4 && idx < i)
                {
                    indices.RemoveAt(0);
                    if (indices.Count > 0) idx = indices[0]-1;
                    else idx = int.MaxValue;
                }
                
            }
            return sb.ToString();
        }
    }
}
