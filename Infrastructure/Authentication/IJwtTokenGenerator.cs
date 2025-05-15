using System;

namespace Infrastructure.Authentication;

public interface IJwtTokenGenerator
{
    public string GenerateToken();
}
