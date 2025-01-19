using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace Client
{
    public partial class WCFSyndicationClient : Form
    {
        public WCFSyndicationClient()
        {
            InitializeComponent();
        }

        private void ATOM_Click(object sender, EventArgs e) => DisplayFeed("atom");

        private void RSS_Click(object sender, EventArgs e) => DisplayFeed("rss");

        private void JSON_Click(object sender, EventArgs e) => DisplayFeed("json");

        private void DisplayFeed(string format)
        {
            richTextBox1.Clear();
            label2.Text = "";

            if (string.IsNullOrEmpty(textBox1.Text))
            {
                label2.Text = "Заполните поле student id";
                return;
            }

            string url = $"http://localhost:8733/SyndicationService/feed/{textBox1.Text}?format={format}";

            try
            {
                if (format == "json")
                {
                    ProcessJsonFeed(url);
                }
                else
                {
                    ProcessXmlFeed(url, format);
                }
            }
            catch (Exception ex)
            {
                richTextBox1.Text = "Ошибка: " + ex.Message;
            }
        }

        private void ProcessXmlFeed(string url, string format)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(url);
            StringBuilder content = new StringBuilder();

            if (format == "atom")
            {
                content.Append("------ ATOM Feed ------\n\n");
                var entries = xmlDoc.SelectNodes("//atom:entry", GetNamespaceManager(xmlDoc));
                foreach (XmlNode entry in entries)
                {
                    string title = entry.SelectSingleNode("atom:title", GetNamespaceManager(xmlDoc))?.InnerText ?? "Нет заголовка";
                    string updated = entry.SelectSingleNode("atom:updated", GetNamespaceManager(xmlDoc))?.InnerText ?? "Нет даты";
                    string contentText = entry.SelectSingleNode("atom:content", GetNamespaceManager(xmlDoc))?.InnerText ?? "Нет содержимого";

                    content.Append($"Заголовок: {title}\n");
                    content.Append($"Обновлено: {updated}\n");
                    content.Append($"Содержимое: {contentText}\n");
                    content.Append(new string('-', 30) + "\n");
                }
            }
            else if (format == "rss")
            {
                content.Append("------ RSS Feed ------\n\n");
                var items = xmlDoc.SelectNodes("rss/channel/item");
                foreach (XmlNode item in items)
                {
                    string title = item.SelectSingleNode("title")?.InnerText ?? "Нет заголовка";
                    string pubDate = item.SelectSingleNode("pubDate")?.InnerText ?? "Нет даты";
                    string description = item.SelectSingleNode("description")?.InnerText ?? "Нет описания";

                    content.Append($"Заголовок: {title}\n");
                    content.Append($"Дата публикации: {pubDate}\n");
                    content.Append($"Описание: {description}\n");
                    content.Append(new string('-', 30) + "\n");
                }
            }

            richTextBox1.Text = content.ToString();
        }


        private void ProcessJsonFeed(string url)
        {
            using (var webClient = new WebClient())
            {
                string jsonData = webClient.DownloadString(url);
                JObject json = JObject.Parse(jsonData);
                StringBuilder content = new StringBuilder();
                content.Append("------ JSON ------\n\n");

                foreach (var item in json["value"])
                {
                    string title = item["subj"]?.ToString() ?? "";
                    string description = item["note"]?.ToString() ?? "";
                    content.Append($"Subject: {title}\nNote: {description}\n\n");
                }

                richTextBox1.Text = content.ToString();
            }
        }

        private XmlNamespaceManager GetNamespaceManager(XmlDocument xmlDoc)
        {
            XmlNamespaceManager nsmgr = new XmlNamespaceManager(xmlDoc.NameTable);
            nsmgr.AddNamespace("atom", "http://www.w3.org/2005/Atom");
            return nsmgr;
        }
    }
}
