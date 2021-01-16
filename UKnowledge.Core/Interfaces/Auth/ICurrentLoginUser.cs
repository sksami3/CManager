using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace UKnowledge.Core.Interfaces.Auth
{
    public interface ICurrentLoginUser
    {
        Guid AccountId { get; }
        void SetClaims(IEnumerable<Claim> claims);
    }
}
