using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.ServiceModel.Syndication;
using System.ServiceModel.Web;
using System.Text;
using WSKMOModel;

namespace SyndicationServiceLibrary
{
    public class Feed : IFeed
    {
        public object GetStudentNotes(string studentId)
        {
            var notes = GetStudentNotesFromDatabase(studentId);

            SyndicationFeed feed = CreateSyndicationFeed(notes);

            string queryFormat = WebOperationContext.Current.IncomingRequest.UriTemplateMatch.QueryParameters["format"];
            return FormatFeed(feed, queryFormat, studentId);
        }

        private List<note> GetStudentNotesFromDatabase(string studentId)
        {
            WSKMOEntities notes = new WSKMOEntities(new Uri("http://localhost:15015/Service1.svc/"));
            return notes.note.Where(n => n.stud_id == int.Parse(studentId)).ToList();
        }

        private SyndicationFeed CreateSyndicationFeed(List<note> notes)
        {
            SyndicationFeed feed = new SyndicationFeed("Notes", "Get list of notes by all subjects for the student", null);
            List<SyndicationItem> items = notes.Select(note =>
                new SyndicationItem($"{note.subject}", $"{note.id}. Note: {note.note1}. StudentID: {note.stud_id}", null)
            ).ToList();
            feed.Items = items;
            return feed;
        }

        private string FormatAsJson(string studentId)
        {
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create($"http://localhost:15015/Service1.svc//note?$filter=stud_id%20eq%20{studentId}&$format=json");
            request.Method = "GET";
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
            string responseString = reader.ReadToEnd();

            
            WebOperationContext.Current.OutgoingResponse.ContentType = "application/json";
            return responseString;
        }

        private object FormatFeed(SyndicationFeed feed, string format, string studentId)
        {
            if(format == "atom")
                return new Atom10FeedFormatter(feed);
            else if(format == "rss")
                 return new Rss20FeedFormatter(feed);
            return FormatAsJson(studentId);
        }
    }
}
