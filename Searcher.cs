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
            var ind = indices
                .SelectMany(i => new int[] { i-3, i-2, i-1, i, i+1 })
                .Distinct()
                .OrderBy(i => i)
                .ToList();
            int idx = indices[0]-1;
            StringBuilder sb = new StringBuilder();
            
            bool dots = false;
            int? start = null;
            for (int i = 0; i < tokens.Count; i++)
            {
                if (ind.Contains(i)) {
                    if (start == null) start = i;
                    dots = false;
                }
                else if (!dots)
                {
                    if (start != null) {
                        var txt = doc.Text.Substring(
                            tokens[start.Value].Index,
                            tokens[i-1].Index + tokens[i - 1].Length - tokens[start.Value].Index);
                        sb.Append(txt);
                    }
                    start = null;
                    sb.Append(" ... ");
                    dots = true;
                }
                
            }
            return sb.ToString();
        }
    }
}
