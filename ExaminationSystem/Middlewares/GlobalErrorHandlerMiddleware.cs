using ExaminationSystem.DAL.Enums;
using ExaminationSystem.Helper.BusinessExceptions;

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
            catch (BusinessException ex)
            {
                var response = ResponseViewModel<bool>.Failure(ex.ErrorCode, ex.Message);
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                await context.Response.WriteAsJsonAsync(response);
            }
            catch (Exception ex)
            {
                var response = ResponseViewModel<bool>.Failure(ErrorCode.UnexpectedError,"An unexpected error occurred. Please try again later.");
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                await context.Response.WriteAsJsonAsync(response);
            }
        }
    }
}