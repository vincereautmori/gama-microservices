﻿using Gama.Application.UseCases.UserAgg.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Gama.Domain.Entities.UsersAgg;

namespace Gama.Application.UseCases.UserAgg.Implementations
{
    internal class HttpContextCurrentUserAcessor : ICurrentUserAccessor
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public HttpContextCurrentUserAcessor(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string GetUsername()
        {
            var claimsPrincipal = GetClaimsPrincipal();

            var username = claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier);
            return username?.Value ?? string.Empty;
        }

        public int GetUserId()
        {
            var claimsPrincipal = GetClaimsPrincipal();

            var id = claimsPrincipal.FindFirst("Id");
            return int.Parse(id?.Value!);
        }

        public User GetUser()
        {
            var claimsPrincipal = GetClaimsPrincipal();

            var id = GetUserId();
            var userName = GetUsername();
            var roles = claimsPrincipal.FindAll(ClaimTypes.Role).Select(x => new UserRoles()
            {
                Role = new Role()
                {
                    Name = x.Value
                }
            }).ToList();

            return new User()
            {
                Id = id,
                Roles = roles,
                Username = userName
            };
        }

        internal ClaimsPrincipal GetClaimsPrincipal()
        {
            return _httpContextAccessor.HttpContext.User ?? throw new InvalidOperationException("Cannot retrieve user from null claims principal.");
        }
    }
}
