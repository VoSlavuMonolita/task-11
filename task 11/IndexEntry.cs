using System;
using System.Collections.Generic;
using System.Linq;

public class IndexEntry
{
    public string Word { get; set; }
    public List<int> PageNumbers { get; set; }


    public IndexEntry(string word, List<int> pageNumbers)
    {
        Word = word;
        PageNumbers = pageNumbers ?? new List<int>();
    }

    public override string ToString()
    {
        return $"{Word}: {string.Join(", ", PageNumbers.OrderBy(p => p))}";
    }
}