using ExaminationSystem.DAL.Enums;

namespace ExaminationSystem.Middlewares
{
    public class GlobalErrorHandlerMiddleware : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                // Log the exception details here later (e.g., using ILogger).

                var response = ResponseViewModel<bool>.Failure(ErrorCode.None,"An unexpected error occurred. Please try again later.");

                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                await context.Response.WriteAsJsonAsync(response);
            }
        }
    }
}