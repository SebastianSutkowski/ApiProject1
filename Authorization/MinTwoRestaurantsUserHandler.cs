using _ApiProject1_.Entities;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace _ApiProject1_.Authorization
{
    public class MinTwoRestaurantsUserHandler:AuthorizationHandler<MinTwoRestaurantsUser>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, MinTwoRestaurantsUser requirement)
        {
            if (requirement.RestaurantsOfUser>=2)
            {
                context.Succeed(requirement);
            }
            
            return Task.CompletedTask;
        }
    }
}
