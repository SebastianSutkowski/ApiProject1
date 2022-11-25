using _ApiProject1_.Entities;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace _ApiProject1_.Authorization
{
    public class OperationRequirementHandler : AuthorizationHandler<OperationRequirement, Restaurant>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, OperationRequirement requirement, Restaurant restaurant)
        {
            if(requirement.ResourceOperation==ResourceOperation.Read ||
                requirement.ResourceOperation == ResourceOperation.Create)
                {
                context.Succeed(requirement);
                }
            var userId=context.User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value;
            if (restaurant.CreatedById == int.Parse(userId))
            {
                context.Succeed(requirement);
            }
            return Task.CompletedTask;
        }
    }
}
