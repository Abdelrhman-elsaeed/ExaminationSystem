using ExaminationSystem.DAL.Enums;
using ExaminationSystem.Helper.BusinessExceptions;

namespace ExaminationSystem.Middlewares
{
    using System.Security.Claims;

    public class GlobalErrorHandlerMiddleware : IMiddleware
    {
        private readonly ILogger<GlobalErrorHandlerMiddleware> _logger;

        public GlobalErrorHandlerMiddleware(ILogger<GlobalErrorHandlerMiddleware> logger)
        {
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (BusinessException ex)
            {
                var traceId = context.TraceIdentifier;
                var path = context.Request.Path;
                var method = context.Request.Method;
                var userId = context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "Anonymous";

                _logger.LogWarning(
                    ex,
                    "Business exception occurred. Method: {Method}, Path: {Path}, UserId: {UserId}, TraceId: {TraceId}, ErrorCode: {ErrorCode}",
                    method,
                    path,
                    userId,
                    traceId,
                    ex.ErrorCode);

                if (!context.Response.HasStarted)
                {
                    context.Response.Clear();
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    context.Response.ContentType = "application/json";

                    var response = ResponseViewModel<bool>.Failure(ex.ErrorCode, ex.Message);
                    await context.Response.WriteAsJsonAsync(response);
                }
            }
            catch (Exception ex)
            {
                var traceId = context.TraceIdentifier;
                var path = context.Request.Path;
                var method = context.Request.Method;
                var userId = context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "Anonymous";

                _logger.LogError(
                    ex,
                    "Unhandled exception occurred. Method: {Method}, Path: {Path}, UserId: {UserId}, TraceId: {TraceId}",
                    method,
                    path,
                    userId,
                    traceId);

                if (!context.Response.HasStarted)
                {
                    context.Response.Clear();
                    context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                    context.Response.ContentType = "application/json";

                    var response = ResponseViewModel<bool>.Failure(
                        ErrorCode.UnexpectedError,
                        "An unexpected error occurred. Please try again later.");

                    await context.Response.WriteAsJsonAsync(response);
                }
            }
        }
    }
}