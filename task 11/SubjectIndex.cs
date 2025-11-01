using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

public class SubjectIndex
{
    private List<IndexEntry> entries;

    public SubjectIndex()
    {
        entries = new List<IndexEntry>();
    }

    //поиск информации в указателе по слову 
    public void TryFindWordInfo(string word)
    {
        var entry = entries.FirstOrDefault(e =>
            e.Word.Equals(word, StringComparison.OrdinalIgnoreCase));

        if (entry != null)
        {
            Console.WriteLine("═".PadRight(50, '═'));
            Console.WriteLine("НАЙДЕНА ЗАПИСЬ:");
            Console.WriteLine("═".PadRight(50, '═'));
            Console.WriteLine($"Слово: {entry.Word}");
            Console.WriteLine($"Количество страниц: {entry.PageNumbers.Count}");
            Console.WriteLine($"Номера страниц: {string.Join(", ", entry.PageNumbers.OrderBy(p => p))}");
            Console.WriteLine("═".PadRight(50, '═'));
            Console.WriteLine();
        }
        else
        {
            Console.WriteLine("═".PadRight(50, '═'));
            Console.WriteLine("ОШИБКА ПОИСКА:");
            Console.WriteLine("═".PadRight(50, '═'));
            Console.WriteLine($"Слово '{word}' не найдено в предметном указателе!");
            Console.WriteLine("═".PadRight(50, '═'));
            Console.WriteLine();
        }
    }

    // Формирование указателя из текстового файла
    public void LoadFromFile(string filePath)
    {
        if (!File.Exists(filePath))
            throw new FileNotFoundException($"Файл {filePath} не найден");

        entries.Clear();
        var lines = File.ReadAllLines(filePath);

        foreach (var line in lines)
        {
            var parts = line.Split(':');
            if (parts.Length == 2)
            {
                var word = parts[0].Trim();
                var pageNumbers = parts[1].Split(',')
                    .Select(p => int.Parse(p.Trim()))
                    .ToList();

                entries.Add(new IndexEntry(word, pageNumbers));
            }
        }
    }

    // Сохранение в XML файл
    public void SaveToXml(string filePath)
    {
        var xmlDoc = new XDocument(
            new XElement("SubjectIndex",
                entries.Select(entry =>
                    new XElement("Entry",
                        new XElement("Word", entry.Word),
                        new XElement("PageNumbers",
                            entry.PageNumbers.Select(page =>
                                new XElement("Page", page)
                            )
                        )
                    )
                )
            )
        );

        xmlDoc.Save(filePath);
    }


    // Печать предметного указателя
    public void PrintIndex()
    {
        Console.WriteLine("ПРЕДМЕТНЫЙ УКАЗАТЕЛЬ:");
        Console.WriteLine("====================");

        foreach (var entry in entries.OrderBy(e => e.Word))
        {
            Console.WriteLine(entry);
        }
        Console.WriteLine();
    }

    // Получение номеров страниц для заданного слова
    public List<int> GetPageNumbersForWord(string word)
    {
        var entry = entries.FirstOrDefault(e =>
            e.Word.Equals(word, StringComparison.OrdinalIgnoreCase));

        return entry?.PageNumbers.OrderBy(p => p).ToList() ?? new List<int>();
    }

    // Добавление элемента
    public void AddEntry(string word, List<int> pageNumbers)
    {
        var existingEntry = entries.FirstOrDefault(e =>
            e.Word.Equals(word, StringComparison.OrdinalIgnoreCase));

        if (existingEntry != null)
        {
            // Если слово уже существует, добавляем новые номера страниц
            foreach (var page in pageNumbers)
            {
                if (!existingEntry.PageNumbers.Contains(page))
                    existingEntry.PageNumbers.Add(page);
            }
        }
        else
        {
            // Создаем новую запись
            entries.Add(new IndexEntry(word, pageNumbers));
        }
    }

    // Удаление элемента
    public bool RemoveEntry(string word)
    {
        var entry = entries.FirstOrDefault(e =>
            e.Word.Equals(word, StringComparison.OrdinalIgnoreCase));

        if (entry != null)
        {
            entries.Remove(entry);
            return true;
        }
        return false;
    }

    // Изменение элемента
    public bool UpdateEntry(string word, List<int> newPageNumbers)
    {
        var entry = entries.FirstOrDefault(e =>
            e.Word.Equals(word, StringComparison.OrdinalIgnoreCase));

        if (entry != null)
        {
            entry.PageNumbers = newPageNumbers;
            return true;
        }
        return false;
    }

    // Сортировка по слову
    public void SortByWord()
    {
        entries = entries.OrderBy(e => e.Word).ToList();
    }

    // Сортировка по количеству страниц
    public void SortByPageCount()
    {
        entries = entries.OrderByDescending(e => e.PageNumbers.Count).ToList();
    }

    // Фильтрация по минимальному количеству страниц
    public List<IndexEntry> FilterByMinPages(int minPages)
    {
        return entries.Where(e => e.PageNumbers.Count >= minPages).ToList();
    }

    // Фильтрация по слову (поиск)
    public List<IndexEntry> FilterByWord(string searchWord)
    {
        return entries.Where(e => e.Word.Contains(searchWord, StringComparison.OrdinalIgnoreCase)).ToList();
    }

    // Получение всех записей
    public List<IndexEntry> GetAllEntries()
    {
        return new List<IndexEntry>(entries);
    }
}