using System.Net;
using System.Net.NetworkInformation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Quest.API.Helpers.Errors
{

    public class ApiError
    {
        public static ObjectResult ProblemDetails(HttpStatusCode code, string message) => new ApiError(code, message).ToProblemDetails();

        public int Status { get; private set; }
        
        public string Title { get; private set; }

        public ApiError(HttpStatusCode code)
        {
            this.Status = (int)code;
            this.Title = code.ToString();
        }

        public ApiError(HttpStatusCode code, string title)
        {
            this.Status = (int)code;
            this.Title = title;
        }

        public ObjectResult ToProblemDetails()
        {
            var problemDetails = new ProblemDetails
            {
                Type = $"https://httpstatuses.com/{Status}",
                Title = Title,
                Status = Status
            };

            return new ObjectResult(problemDetails)
            {
                ContentTypes = { "application/problem+json" },
                StatusCode = Status,
            };
        }
    }
}
