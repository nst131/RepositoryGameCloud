
using System.Net;
using System.Text.Json;

namespace AuthTraining.Middleware
{
    public class ErrorMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorMiddleware> logger;
        private readonly IHostEnvironment environment;

        public ErrorMiddleware(
            RequestDelegate next,
            ILogger<ErrorMiddleware> logger,
            IHostEnvironment environment)
        {
            this._next = next;
            this.logger = logger;
            this.environment = environment;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                switch (ex)
                {
                    case TaskCanceledException:
                        logger.LogInformation(ex.Message);
                        break;
                    default:
                        logger.LogError($"Error Message {ex.Message}, StackTrace {ex.StackTrace}");
                        break;
                }

                await ErrorHandler(context, ex, environment);
            }
        }

        public async Task ErrorHandler(HttpContext context, Exception error, IHostEnvironment environment)
        {
            var response = context.Response;
            response.ContentType = "application/json";

            response.StatusCode = error switch
            {
                UnauthorizedAccessException => (int)HttpStatusCode.Unauthorized,
                NullReferenceException => (int)HttpStatusCode.NotFound,
                     ArgumentNullException
                    or ArgumentException
                    or ArgumentOutOfRangeException
                    or KeyNotFoundException => (int)HttpStatusCode.BadRequest,
                TaskCanceledException => (int)HttpStatusCode.Conflict,
                _ => (int)HttpStatusCode.InternalServerError
            };

            var result = JsonSerializer.Serialize(new
            {
                message = error.Message,
                innerException = error.InnerException?.Message,
                stackTrace = environment.IsDevelopment() ? error.StackTrace : null,
            });

            await response.WriteAsync(result);
        }
    }
}
