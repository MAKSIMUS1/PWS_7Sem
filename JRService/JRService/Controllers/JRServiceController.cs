using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Http;
using Newtonsoft.Json.Linq;

namespace JRService.Controllers
{
    public class JRServiceController : ApiController
    {
        private Dictionary<string, int> memory
        {
            get
            {
                if (HttpContext.Current.Session["memory"] == null)
                {
                    HttpContext.Current.Session["memory"] = new Dictionary<string, int>();
                }
                return (Dictionary<string, int>)HttpContext.Current.Session["memory"];
            }
            set
            {
                HttpContext.Current.Session["memory"] = value;
            }
        }

        private bool errorExit
        {
            get
            {
                return HttpContext.Current.Session["errorExit"] != null && (bool)HttpContext.Current.Session["errorExit"];
            }
            set
            {
                HttpContext.Current.Session["errorExit"] = value;
            }
        }

        [HttpPost]
        [Route("api/JRService/HandleRequest")]
        public IHttpActionResult HandleRequest([FromBody] JToken request)
        {
            if (errorExit)
            {
                memory.Clear();

                return Json(new List<object>
                {
                    GenerateErrorResponse(-32000, "Service reset", null)
                });
            }

            try
            {
                if (request is JArray requestArray)
                {
                    var responses = new List<object>();
                    foreach (var singleReq in requestArray)
                    {
                        if (errorExit)
                        {
                            errorExit = false;
                            break;
                        }
                            
                        responses.Add(ProcessSingleRequest(singleReq as JObject));
                    }
                    return Json(responses);
                }

                if (request is JObject singleRequest)
                {
                    return Json(ProcessSingleRequest(singleRequest));
                }

                return GenerateErrorResponse(-32600, "Invalid Request", null);
            }
            catch (Exception ex)
            {
                return GenerateErrorResponse(-32603, ex.Message, null);
            }
        }

        private object ProcessSingleRequest(JObject request)
        {
            if (request == null || request["jsonrpc"]?.ToString() != "2.0" || request["method"] == null)
            {
                return GenerateErrorResponse(-32600, "Invalid Request", request["id"]?.ToString());
            }

            string method = request["method"].ToString();
            var id = request["id"]?.ToString();

            try
            {
                if (errorExit)
                {
                    return null;
                }

                object result;
                var parametersToken = request["params"];
                List<object> parametersAsArray = parametersToken?.Type == JTokenType.Array
                    ? parametersToken.ToObject<List<object>>()
                    : null;

                JObject parametersAsObject = parametersToken?.Type == JTokenType.Object
                    ? parametersToken.ToObject<JObject>()
                    : null;

                switch (method)
                {
                    case "SetM":
                        result = parametersAsObject != null
                            ? SetM(parametersAsObject["key"]?.ToString(), parametersAsObject["value"]?.ToObject<int>() ?? 0)
                            : SetM(parametersAsArray?[0].ToString(), Convert.ToInt32(parametersAsArray?[1]));
                        break;
                    case "GetM":
                        result = parametersAsObject != null
                            ? GetM(parametersAsObject["key"]?.ToString())
                            : GetM(parametersAsArray?[0].ToString());
                        break;
                    case "AddM":
                        result = parametersAsObject != null
                            ? AddM(parametersAsObject["key"]?.ToString(), parametersAsObject["value"]?.ToObject<int>() ?? 0)
                            : AddM(parametersAsArray?[0].ToString(), Convert.ToInt32(parametersAsArray?[1]));
                        break;
                    case "SubM":
                        result = parametersAsObject != null
                            ? SubM(parametersAsObject["key"]?.ToString(), parametersAsObject["value"]?.ToObject<int>() ?? 0)
                            : SubM(parametersAsArray?[0].ToString(), Convert.ToInt32(parametersAsArray?[1]));
                        break;
                    case "MulM":
                        result = parametersAsObject != null
                            ? MulM(parametersAsObject["key"]?.ToString(), parametersAsObject["value"]?.ToObject<int>() ?? 0)
                            : MulM(parametersAsArray?[0].ToString(), Convert.ToInt32(parametersAsArray?[1]));
                        break;
                    case "DivM":
                        result = parametersAsObject != null
                            ? DivM(parametersAsObject["key"]?.ToString(), parametersAsObject["value"]?.ToObject<int>() ?? 0)
                            : DivM(parametersAsArray?[0].ToString(), Convert.ToInt32(parametersAsArray?[1]));
                        break;
                    case "ErrorExit":
                        result = ResetService();
                        break;
                    default:
                        throw new JsonRpcException(-32601, "Method not found.");
                }

                return new { jsonrpc = "2.0", result, id };
            }
            catch (JsonRpcException ex)
            {
                return new { jsonrpc = "2.0", error = new { code = ex.Code, message = ex.Message }, id };
            }
        }



        private int SetM(string k, int x)
        {
            if (string.IsNullOrEmpty(k))
                throw new JsonRpcException(-32602, "Invalid params: key is null or empty.");

            memory[k] = x;
            return x;
        }

        private int GetM(string k)
        {
            if (string.IsNullOrEmpty(k) || !memory.ContainsKey(k))
                throw new JsonRpcException(-32602, "Invalid params: key not found.");

            return memory[k];
        }

        private int AddM(string k, int x)
        {
            if (!memory.ContainsKey(k))
                throw new JsonRpcException(-32602, "Invalid params: key not found.");

            memory[k] += x;
            return memory[k];
        }

        private int SubM(string k, int x)
        {
            if (!memory.ContainsKey(k))
                throw new JsonRpcException(-32602, "Invalid params: key not found.");

            memory[k] -= x;
            return memory[k];
        }

        private int MulM(string k, int x)
        {
            if (!memory.ContainsKey(k))
                throw new JsonRpcException(-32602, "Invalid params: key not found.");

            memory[k] *= x;
            return memory[k];
        }

        private int DivM(string k, int x)
        {
            if (!memory.ContainsKey(k))
                throw new JsonRpcException(-32602, "Invalid params: key not found.");

            if (x == 0)
                throw new JsonRpcException(-32602, "Division by zero.");

            memory[k] /= x;
            return memory[k];
        }

        private string ResetService()
        {
            memory.Clear();
            errorExit = true;
            return "Service reset.";
        }

        private IHttpActionResult GenerateErrorResponse(int code, string message, string id)
        {
            return Json(new
            {
                jsonrpc = "2.0",
                error = new { code, message },
                id
            });
        }

        private class JsonRpcException : Exception
        {
            public int Code { get; }

            public JsonRpcException(int code, string message) : base(message)
            {
                Code = code;
            }
        }
    }
}
