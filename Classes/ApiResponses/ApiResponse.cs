using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace AKK.Classes.ApiResponses {
    public class ApiResponse<T> : IActionResult {

        public T Data {get; set;}
        public bool Success {get; set;}
        public virtual object Value {
            get {
                return new {success = Success, data=Data};
            } 
        }

        public ApiResponse (bool success)
        {
            Success = success;
            Data = default(T);
        }

        public void ExecuteResult(ControllerContext context)
        {
            new JsonResult(Value).ExecuteResult(context);
        }

        public Task ExecuteResultAsync(ActionContext context)
        {
            return new JsonResult(Value).ExecuteResultAsync(context);
        }
    }
}