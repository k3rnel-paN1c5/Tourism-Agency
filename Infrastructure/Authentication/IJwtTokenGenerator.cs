using System;

namespace Infrastructure.Authentication;


/// <summary>
/// Defines the interface for generating JSON Web Tokens (JWTs).
/// </summary>
public interface IJwtTokenGenerator
{

    /// <summary>
    /// Generates a JWT for the specified user.
    /// </summary>
    /// <param name="userId">The unique identifier of the user.</param>
    /// <param name="email">The email of the user.</param>
    /// <param name="role">The role of the user.</param>
    /// <returns>A string representing the generated JWT.</returns>
    public string GenerateToken(string userId, string email, string role);
}
