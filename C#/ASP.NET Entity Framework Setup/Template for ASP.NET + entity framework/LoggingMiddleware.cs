using Template_for_ASP.NET___entity_framework.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Template_for_ASP.NET___entity_framework.Interfaces;


/*
 * Required implementations:
 * 
 * app.UseMiddleware<LoggingMiddleware>();                    --- After .UseRouting !!!
 * builder.Services.AddTransient<ILogService, LogService>();  --- *** Your Version of Logging Service / Singleton if using database
 * Log Class                                                  --- Your version of Log Class
 * 
 * Replace the the specific items on the marked lines with ????
 */


namespace FoxClub
{
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;

        public LoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext, LogService logService) // ????
        {
            await _next(httpContext);

            var method = httpContext.Request.Method.ToString(); //Get Method (Post, Get)
            string functionName = string.Empty;

            if (httpContext.GetEndpoint() != null) //Get Controller Endpoint Function Name
            {
                functionName = httpContext.GetEndpoint().ToString().Split(".").Last().Split(" ").First();
            }

            /*
             * Post Method
             * Getting information from body
             * 
             * dataReq = Body Request
             * dataStream = Body Stream
             * dataQuery = QueryParsed Stream
            */
            if (method == "POST")
            {
                var dataStream = "";

                var dataReq = httpContext.Request;
                dataReq.EnableBuffering();
                using (StreamReader reader
                      = new StreamReader(dataReq.Body, Encoding.UTF8, true, 1024, true))
                {
                    dataStream = reader.ReadToEndAsync().Result;
                }
                var dataQuery = HttpUtility.ParseQueryString(dataStream);

                dataReq.Body.Position = 0;
                dataQuery.Remove("__RequestVerificationToken"); //Removing automaticly generated line


                var calledParameters = string.Join(";;", dataQuery); // All of the called paremeters joined into a single string:
                var statusCode = httpContext.Response.StatusCode; //Get Status Code of response.

                logService.AddLog(
                        method,
                        functionName,
                        statusCode
                        calledParameters);
            }
            else if (method == "GET") //Reads QueryString
            {
                var query = HttpUtility.ParseQueryString(httpContext.Request.QueryString.ToString());
                var calledParameters = string.Join(";;", query); // All of the called paremeters joined into a single string:
                var statusCode = httpContext.Response.StatusCode; //Get Status Code of response.

                logService.AddLog(
                        method,
                        functionName,
                        statusCode,
                        calledParameters);
            }
        }
    }

    public static class LoggingMiddlewareExtensions
    {
        public static IApplicationBuilder UseLoggingMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<LoggingMiddleware>();
        }
    }
}
