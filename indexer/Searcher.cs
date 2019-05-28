using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace WebSearcher
{
    abstract class Searcher
    {
        public string BasePath;
        public Searcher(string basePath)
        {
            BasePath = basePath;
        }

        public abstract IQueryable<Posting> Find(List<string> words);

        public void Query(List<string> words)
        {
            words = words.Select(w => w.ToLower()).ToList();
            var stw = new Stopwatch();
            stw.Start();
            var postings = Find(words)
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
            var doc = new Document(BasePath+document, document);
            var ind = indices
                .SelectMany(i => new int[] { i-4, i-3, i-2, i-1, i, i+1, i+2 })
                .Distinct()
                .OrderBy(i => i)
                .ToList();
            int idx = indices[0]-1;
            StringBuilder sb = new StringBuilder();
            
            bool dots = false;
            int? start = null;
            for (int i = 0; i < doc.Tokens.Count; i++)
            {
                if (ind.Contains(i)) {
                    if (start == null) start = i;
                    dots = false;
                }
                else if (!dots)
                {
                    if (start != null) {
                        var first = doc.Tokens[start.Value];
                        var last = doc.Tokens[i - 1];
                        var txt = doc.Text.Substring(
                            first.Index,
                            last.Index + last.Length - first.Index);
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
