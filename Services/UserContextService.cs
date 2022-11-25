﻿using Microsoft.AspNetCore.Http;
using System.Security.Claims;
namespace _ApiProject1_.Services
{
    public interface IUserContextService
    {
        ClaimsPrincipal User { get; }
        int? GetUserId { get; }
    }
    public class UserContextService: IUserContextService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserContextService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public ClaimsPrincipal User => _httpContextAccessor.HttpContext?.User;
        public int? GetUserId => User is null ? null : int.Parse(User.FindFirst(u => u.Type == ClaimTypes.NameIdentifier).Value);

    }

    
}
