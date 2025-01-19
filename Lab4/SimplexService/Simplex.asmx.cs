using SimplexService.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;

namespace SimplexService
{
    [WebService(Namespace = "http://KMO/")]
    [System.Web.Script.Services.ScriptService]
    public class Simplex : System.Web.Services.WebService
    {
        [WebMethod(Description = "Returns the sum of two integers.", MessageName = "Add")]
        public int Add(int x, int y)
        {
            return x + y;
        }

        [WebMethod(Description = "Concatenates a string and a double.", MessageName = "Concat")]
        public string Concat(string s, double d)
        {
            return s + d.ToString();
        }

        [WebMethod(Description = "Combines two objects of type A.", MessageName = "Sum")]
        public A Sum(A a1, A a2)
        {
            string logFilePath = HttpContext.Current.Server.MapPath("~/RequestLog.txt");

            using (var writer = new StreamWriter(logFilePath, true))
            {
                writer.WriteLine($"--- Новый запрос {DateTime.Now} ---");
                writer.WriteLine($"Request HttpMethod: {HttpContext.Current.Request.HttpMethod}");
                writer.WriteLine($"Request URL: {HttpContext.Current.Request.Url}");

                foreach (string header in HttpContext.Current.Request.Headers)
                {
                    writer.WriteLine($"Header: {header} = {HttpContext.Current.Request.Headers[header]}");
                }

                if (HttpContext.Current.Request.HttpMethod == "POST")
                {
                    HttpContext.Current.Request.InputStream.Position = 0;
                    using (var reader = new StreamReader(HttpContext.Current.Request.InputStream))
                    {
                        string body = reader.ReadToEnd();
                        writer.WriteLine("Request Body:");
                        writer.WriteLine(body);
                    }
                }

                writer.WriteLine($"--- Конец запроса {DateTime.Now} ---");
                writer.WriteLine();
            }

            return new A
            {
                S = a1.S + a2.S,
                K = a1.K + a2.K,
                F = a1.F + a2.F
            };
        }

        [WebMethod(Description = "Adds two integers for AJAX calls.", MessageName = "AddS")]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public int AddS(int x, int y)
        {
            return x + y;
        }
    }
}
