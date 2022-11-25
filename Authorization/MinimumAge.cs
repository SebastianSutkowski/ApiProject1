using Microsoft.AspNetCore.Authorization;
namespace _ApiProject1_.Authorization
{
    public class MinimumAge:IAuthorizationRequirement
    {
        public int MinAge { get; }
        public MinimumAge(int minAge)
        {
            MinAge = minAge;
        }
    }
}
