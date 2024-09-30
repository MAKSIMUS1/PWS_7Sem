using System.Collections.Generic;
using System.Web.Http;

namespace Lab2_RestService.Controllers
{
    public class ResultModel
    {
        public int RESULT { get; set; }
    }
    public class AddModel
    {
        public int ADD { get; set; }
    }
    public class ResultController : ApiController
    {
        private static int RESULT = 0;
        private static Stack<int> stack = new Stack<int>();

        // GET: api/result
        [HttpGet]
        public IHttpActionResult Get()
        {
            int topOfStack = stack.Count > 0 ? stack.Peek() : 0;
            var resultWithTopOfStack = RESULT + topOfStack;

            return Ok(new
            {
                RESULT = RESULT,
                TopOfStack = topOfStack,
                RESULT_With_TopOfStack = resultWithTopOfStack
            });
        }

        // POST: api/result
        [HttpPost]
        public IHttpActionResult Post([FromBody] ResultModel model)
        {
            if (model == null || model.RESULT == 0)
            {
                return BadRequest("RESULT cannot be null or zero.");
            }

            RESULT = model.RESULT;
            return Ok(new { RESULT });
        }

        // PUT: api/result
        [HttpPut]
        public IHttpActionResult Put([FromBody] AddModel model)
        {
            if (model == null || model.ADD == 0)
            {
                return BadRequest("ADD value cannot be null or zero.");
            }

            stack.Push(model.ADD);
            return Ok(new { Stack = stack });
        }

        // DELETE: api/result
        [HttpDelete]
        public IHttpActionResult Delete()
        {
            if (stack.Count == 0)
            {
                return BadRequest("Stack is empty.");
            }

            int poppedValue = stack.Pop();

            return Ok(new { RESULT, PoppedValue = poppedValue });
        }
    }
}
