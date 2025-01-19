using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Xml.Linq;
using Web.Models;

namespace Web.Controllers
{
    public class HomeController : Controller
    {
        private static readonly HttpClient _httpClient = new HttpClient();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";
            return View();
        }

        public async Task<ActionResult> GetFeed(string studentId, string format)
        {
            if (string.IsNullOrEmpty(studentId))
            {
                return new HttpStatusCodeResult(400, "Student ID is required.");
            }

            var url = $"http://localhost:8733/SyndicationService/feed/{studentId}?format={format}";
            string content;

            try
            {
                // Получение данных с API
                content = await _httpClient.GetStringAsync(url);

                // В зависимости от формата, обработка данных
                if (format == "json")
                {
                    return Content(ProcessJsonFeed(content), "application/json");
                }
                else
                {
                    return Content(ProcessXmlFeed(content, format), "application/xml");
                }
            }
            catch (Exception ex)
            {
                return Content($"Error: {ex.Message}");
            }
        }

        // Обработка JSON ленты
        private string ProcessJsonFeed(string content)
        {
            var json = JObject.Parse(content);
            var grades = new List<StudentGrade>();

            foreach (var item in json["value"])
            {
                grades.Add(new StudentGrade
                {
                    Subject = item["subj"]?.ToString(),
                    Note = int.TryParse(item["note"]?.ToString(), out var note) ? note : 0
                });
            }

            return JsonConvert.SerializeObject(grades, Formatting.Indented);
        }

        // Обработка XML ленты (Atom и RSS)
        private string ProcessXmlFeed(string content, string format)
        {
            var xdoc = XDocument.Parse(content);
            var grades = new List<StudentGrade>();

            if (format == "atom")
            {
                foreach (var entry in xdoc.Descendants("{http://www.w3.org/2005/Atom}entry"))
                {
                    grades.Add(new StudentGrade
                    {
                        Subject = entry.Element("{http://www.w3.org/2005/Atom}title")?.Value,
                        Note = int.TryParse(entry.Element("{http://www.w3.org/2005/Atom}content")?.Value, out var note) ? note : 0
                    });
                }
            }
            else if (format == "rss")
            {
                foreach (var item in xdoc.Descendants("item"))
                {
                    grades.Add(new StudentGrade
                    {
                        Subject = item.Element("title")?.Value,
                        Note = int.TryParse(item.Element("description")?.Value, out var note) ? note : 0
                    });
                }
            }

            // Преобразуем данные в XML
            var xml = new XDocument(new XElement("grades", grades.Select(g => new XElement("grade",
                new XElement("subject", g.Subject),
                new XElement("note", g.Note)))));

            return xml.ToString();
        }
    }
}
