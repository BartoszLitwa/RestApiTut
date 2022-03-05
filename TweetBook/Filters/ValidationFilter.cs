using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;
using System.Threading.Tasks;
using TweetBook.Contracts.V1.Responses.Error;

namespace TweetBook.Filters
{
    public class ValidationFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            // Before controller 
            if (!context.ModelState.IsValid)
            {
                var errorInModelState = context.ModelState
                    // Select only ones that do have error
                    .Where(x => x.Value.Errors.Count() > 0)
                    .ToDictionary(kvp => kvp.Key, kvp => kvp.Value.Errors.Select(x => x.ErrorMessage))
                    .ToArray();

                var errorResponse = new ErrorResponse();

                // For Each error in Model State
                foreach(var error in errorInModelState)
                {
                    // Each error can consists other subError that it is buit from
                    foreach (var subError in error.Value)
                    {
                        var errorModel = new ErrorModel
                        {
                            // The name of field
                            FieldName = error.Key,
                            Message = subError
                        };

                        errorResponse.Errors.Add(errorModel);
                    }
                }

                // Set the contect result and leave the function
                context.Result = new BadRequestObjectResult(errorResponse);
                return;
            }
               

            // Calls next thing in our pipeline - Api Controller 
            await next();

            // After controller
        }
    }
}
