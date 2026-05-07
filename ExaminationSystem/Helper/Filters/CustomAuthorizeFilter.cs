using ExaminationSystem.Enums;
using ExaminationSystem.Enums.JWT_Role;
using ExaminationSystem.Services;
using ExaminationSystem.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace ExaminationSystem.Helper.Filters
{
    // API/Filters/CustomAuthorizeFilter.cs
    public class CustomAuthorizeFilter : IAsyncActionFilter
    {
        private readonly RoleFeatureService _roleFeatureService;
        private readonly Feature _requiredFeature;

        public CustomAuthorizeFilter(Feature requiredFeature,RoleFeatureService roleFeatureService)
        {
            _requiredFeature = requiredFeature;
            _roleFeatureService = roleFeatureService;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var roleClaim = context.HttpContext.User.FindFirst(ClaimTypes.Role);

            if (roleClaim is null)
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            if (!Enum.TryParse<Role>(roleClaim.Value, out Role userRole))
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            bool hasAccess = await _roleFeatureService.HasAccessAsync(_requiredFeature, userRole);

            if (!hasAccess)
            {
                context.Result = new ObjectResult(ResponseViewModel<string>.Failure(ErrorCode.AccessDenied, "sorry you dont have the permission to access this feature"))
                {
                    StatusCode = StatusCodes.Status403Forbidden
                };
                return;
            }

            await next();
        }
    }
}
