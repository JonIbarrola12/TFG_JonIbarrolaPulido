using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace SportsHome.IL.Repository.EF.Helpers
{
    public class UserHelper
    {
        private IHttpContextAccessor _httpContextAccessor;

        public UserHelper(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public int? GetUsuarioId()
        {
            if (_httpContextAccessor.HttpContext != null)
            {
                IEnumerable<Claim> claims = _httpContextAccessor.HttpContext.User.Claims;
                Claim claim = claims.FirstOrDefault(c => c.Type == "UsuarioId");
                int? usuarioId = int.Parse(claim.Value);

                return usuarioId;
            }
            return null;
        }
    }
}
