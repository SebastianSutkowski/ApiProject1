using Microsoft.AspNetCore.Authorization;
namespace _ApiProject1_.Authorization
{
    public enum ResourceOperation
    {
        Create,
        Read,
        Update,
        Delete
    }
    public class OperationRequirement:IAuthorizationRequirement
    {
        public OperationRequirement(ResourceOperation resourceOperation)
        {
            ResourceOperation = resourceOperation;
        }
        public ResourceOperation ResourceOperation { get;}
    }
}
