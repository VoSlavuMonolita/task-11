using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

class Program
{
    static void Main()
    {
        var index = new SubjectIndex();

        // Создание тестового файла
        CreateTestFile("test_input.txt");

        try
        {
            // Демонстрация работы всех функций

            // 1. Загрузка из файла
            Console.WriteLine("1. Загрузка данных из файла...");
            index.LoadFromFile("test_input.txt");
            index.PrintIndex();

            // 2. Добавление новых записей
            Console.WriteLine("2. Добавление новых записей...");
            index.AddEntry("программирование", new List<int> { 15, 16, 17 });
            index.AddEntry("алгоритм", new List<int> { 25, 26 });
            index.PrintIndex();

            // 3. Сохранение в XML
            Console.WriteLine("3. Сохранение в XML...");
            index.SaveToXml("index.xml");
            Console.WriteLine("Данные сохранены в index.xml\n");

            // 4. Поиск номеров страниц для слова
            Console.WriteLine("4. Поиск номеров страниц для слова 'класс':");
            var pages = index.GetPageNumbersForWord("класс");
            Console.WriteLine($"Страницы: {string.Join(", ", pages)}\n");

            // 5. Сортировка по слову
            Console.WriteLine("5. Сортировка по алфавиту:");
            index.SortByWord();
            index.PrintIndex();

            // 6. Фильтрация
            Console.WriteLine("6. Слова, встречающиеся на 2+ страницах:");
            var filtered = index.FilterByMinPages(2);
            foreach (var entry in filtered)
            {
                Console.WriteLine(entry);
            }
            Console.WriteLine();

            // 7. Обновление записи
            Console.WriteLine("7. Обновление записи 'объект':");
            index.UpdateEntry("объект", new List<int> { 5, 6, 7, 8 });
            index.PrintIndex();

            // 8. Удаление записи
            Console.WriteLine("8. Удаление записи 'метод':");
            index.RemoveEntry("метод");
            index.PrintIndex();
            Console.WriteLine("проверим наличие информации о словах в указателе:\n");
            
            //Проверим функцию для поиска информации о слове в предметном указателе
            string WordToFind_Exists="класс";
            Console.WriteLine($"Ищем в предметном указателе информацию о слове '{WordToFind_Exists}':\n");
            index.TryFindWordInfo(WordToFind_Exists);

            string WordToFind_NotExists = "нет";
            Console.WriteLine($"Ищем в предметном указателе информацию о слове '{WordToFind_NotExists}':\n");

            index.TryFindWordInfo(WordToFind_NotExists);

        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}");
        }

        Console.WriteLine("Нажмите любую клавишу для выхода...");
        Console.ReadKey();
    }

    static void CreateTestFile(string filePath)
    {
        var testData = new[]
        {
            "класс: 1, 2, 3",
            "объект: 4, 5",
            "метод: 6, 7, 8, 9",
            "свойство: 10, 11",
            "интерфейс: 12, 13, 14"
        };

        File.WriteAllLines(filePath, testData);
    }
}