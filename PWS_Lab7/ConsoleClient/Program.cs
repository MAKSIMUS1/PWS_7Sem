using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Linq;

class Program
{
    static async Task Main(string[] args)
    {
        while (true)  
        {
            Console.WriteLine("Введите ID студента:");
            string studentId = Console.ReadLine();

            if (string.IsNullOrEmpty(studentId))
            {
                Console.WriteLine("ID студента не может быть пустым.");
                continue; 
            }

            Console.WriteLine("Выберите формат: \n1. RSS\n2. Atom");
            string formatChoice = Console.ReadLine();

            string format;
            switch (formatChoice)
            {
                case "1":
                    format = "rss";
                    break;
                case "2":
                    format = "atom";
                    break;
                default:
                    Console.WriteLine("Неверный выбор формата. Попробуйте снова.");
                    continue;  
            }

            string url = $"http://localhost:8733/SyndicationService/feed/{studentId}?format={format}";

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = await client.GetAsync(url);

                    response.EnsureSuccessStatusCode();

                    string content = await response.Content.ReadAsStringAsync();
                    Console.WriteLine("Результат:\n");

                    if (format == "rss")
                    {
                        PrintRssFeed(content);
                    }
                    else if (format == "atom")
                    {
                        PrintAtomFeed(content);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка: {ex.Message}");
                }
            }

            Console.WriteLine("\nХотите выполнить еще один запрос? (y/n)");
            string continueChoice = Console.ReadLine()?.ToLower();
            if (continueChoice != "y")
            {
                break;  
            }
        }
    }

    static void PrintRssFeed(string content)
    {
        var xdoc = XDocument.Parse(content);
        var items = xdoc.Descendants("item");

        Console.WriteLine("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
        Console.WriteLine("<rss version=\"2.0\">");
        Console.WriteLine("  <channel>");
        Console.WriteLine("    <title>RSS Feed</title>");
        Console.WriteLine("    <link>http://localhost:8733</link>");
        Console.WriteLine("    <description>Feed for student notes</description>");

        foreach (var item in items)
        {
            var title = item.Element("title")?.Value ?? "No Title";
            var link = item.Element("link")?.Value ?? "#";
            var description = item.Element("description")?.Value ?? "No Description";
            var pubDate = item.Element("pubDate")?.Value ?? "No Date";

            Console.WriteLine("    <item>");
            Console.WriteLine($"      <title>{title}</title>");
            Console.WriteLine($"      <link>{link}</link>");
            Console.WriteLine($"      <description>{description}</description>");
            Console.WriteLine($"      <pubDate>{pubDate}</pubDate>");
            Console.WriteLine("    </item>");
        }

        Console.WriteLine("  </channel>");
        Console.WriteLine("</rss>");
    }

    static void PrintAtomFeed(string content)
    {
        var xdoc = XDocument.Parse(content);
        var entries = xdoc.Descendants("{http://www.w3.org/2005/Atom}entry");

        Console.WriteLine("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
        Console.WriteLine("<feed xmlns=\"http://www.w3.org/2005/Atom\">");
        Console.WriteLine("  <title>Atom Feed</title>");
        Console.WriteLine("  <link href=\"http://localhost:8733\" />");
        Console.WriteLine("  <updated>2024-11-17T00:00:00Z</updated>");

        foreach (var entry in entries)
        {
            var title = entry.Element("{http://www.w3.org/2005/Atom}title")?.Value ?? "No Title";
            var link = entry.Element("{http://www.w3.org/2005/Atom}link")?.Attribute("href")?.Value ?? "#";
            var contentValue = entry.Element("{http://www.w3.org/2005/Atom}content")?.Value ?? "No Content";
            var published = entry.Element("{http://www.w3.org/2005/Atom}published")?.Value ?? "No Date";

            Console.WriteLine("  <entry>");
            Console.WriteLine($"    <title>{title}</title>");
            Console.WriteLine($"    <link href=\"{link}\" />");
            Console.WriteLine($"    <content>{contentValue}</content>");
            Console.WriteLine($"    <published>{published}</published>");
            Console.WriteLine("  </entry>");
        }

        Console.WriteLine("</feed>");
    }
}
