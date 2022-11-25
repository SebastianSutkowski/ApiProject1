using Microsoft.AspNetCore.Authorization;

namespace _ApiProject1_.Authorization
{
    public class MinTwoRestaurantsUser : IAuthorizationRequirement
    {
        public int RestaurantsOfUser { get;}
        public MinTwoRestaurantsUser(int restaurantsOfUser)
        {
            RestaurantsOfUser=restaurantsOfUser;
        }
    }
}
