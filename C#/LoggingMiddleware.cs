using FoxClub.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;


/*
 * Required implementations:
 * 
 * app.UseMiddleware<LoggingMiddleware>();                    --- After .UseRouting !!!
 * builder.Services.AddSingleton<***LogService***>();         --- *** Your Version of Logging Service / Scoped if using database
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

        public Task Invoke(HttpContext httpContext, LogService logService) // ????
        {
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


                /*Also possible to call for items like this:
                 * dataQuery.Keys = Paremeters as you can insert them into body
                 */
                if (dataQuery.Keys.Count > 0)
                {
                    logService.AddLog(  //????
                        dataQuery.Get(0) ?? string.Empty,
                        functionName,
                        method,
                        dataQuery.Keys.Count > 1 ? dataQuery.Get(1).ToString() ?? string.Empty : string.Empty,
                        dataQuery.Keys.Count > 2 ? dataQuery.Get(2).ToString() ?? string.Empty : string.Empty);
                }
            }
            else if (method == "GET") //Reads QueryString
            {
                var query = HttpUtility.ParseQueryString(httpContext.Request.QueryString.ToString());
                if (query.Keys.Count > 0)
                {
                    logService.AddLog(query.Get(0) ?? String.Empty, //????
                                     functionName,
                                     method,
                                     query.Keys.Count > 1 ? query.Get(1) ?? String.Empty : "",
                                     query.Keys.Count > 2 ? query.Get(2) ?? String.Empty : "");
                }
            }
            return _next(httpContext);
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
