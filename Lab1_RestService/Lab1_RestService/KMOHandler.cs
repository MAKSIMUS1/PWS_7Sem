using System.Web;
using System.Web.SessionState;
using System.Collections.Generic;
using System.Web.Script.Serialization;

namespace Lab1_RestService
{
    public class KMOHandler : IHttpHandler, IRequiresSessionState
    {
        private static int RESULT = 0;

        public void ProcessRequest(HttpContext context)
        {
            var response = context.Response;
            response.ContentType = "application/json";

            Stack<int> stack = context.Session["stack"] as Stack<int> ?? new Stack<int>();

            switch (context.Request.HttpMethod)
            {
                case "GET":
                    int topOfStack = stack.Count > 0 ? stack.Peek() : 0;
                    var resultWithTopOfStack = RESULT + topOfStack;

                    var resultJson = new JavaScriptSerializer().Serialize(new
                    {
                        RESULT = RESULT,
                        TopOfStack = topOfStack,
                        RESULT_With_TopOfStack = resultWithTopOfStack
                    });
                    response.Write(resultJson);
                    break;

                case "POST":
                    if (int.TryParse(context.Request.Params["RESULT"], out int newResult))
                    {
                        RESULT = newResult;
                        response.Write(new JavaScriptSerializer().Serialize(new { RESULT }));
                    }
                    else
                    {
                        response.StatusCode = 400;
                        response.Write("Invalid input for RESULT.");
                    }
                    break;

                case "PUT":
                    if (int.TryParse(context.Request.Params["ADD"], out int valueToAdd))
                    {
                        stack.Push(valueToAdd);
                        context.Session["stack"] = stack;
                        response.Write(new JavaScriptSerializer().Serialize(new { stack }));
                    }
                    else
                    {
                        response.StatusCode = 400;
                        response.Write("Invalid input for ADD.");
                    }
                    break;

                case "DELETE":
                    if (stack.Count > 0)
                    {
                        stack.Pop();
                        context.Session["stack"] = stack;
                        response.Write(new JavaScriptSerializer().Serialize(new { RESULT }));
                    }
                    else
                    {
                        response.StatusCode = 400;
                        response.Write("Stack is empty.");
                    }
                    break;

                default:
                    response.StatusCode = 405;
                    response.Write("Unsupported HTTP method.");
                    break;
            }
        }

        public bool IsReusable => false;
    }
}
