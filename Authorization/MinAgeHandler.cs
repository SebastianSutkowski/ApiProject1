using Microsoft.AspNetCore.Authorization;

namespace _ApiProject1_.Authorization
{
    public class MinAgeHandler : AuthorizationHandler<MinimumAge>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, MinimumAge requirement)
        {
            var birthDate = DateTime.Parse(context.User.FindFirst(c => c.Type == "DateOfBirth").Value);
            if (birthDate.AddYears(requirement.MinAge) < DateTime.Now)
            {
                context.Succeed(requirement);
            }
            return Task.CompletedTask;
        }
    }
}
