namespace Telemetry.Web.Services.Jwt
{
    /// <summary>Factory interface for token</summary>
    public interface IJwtFactory
    {
        /// <summary>Generate token string</summary>
        /// <param name="email">User email</param>
        /// <returns></returns>
        string GenerateToken(string email);
    }
}