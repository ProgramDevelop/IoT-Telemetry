using Microsoft.Extensions.Options;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Telemetry.Web.Services.Jwt
{
    /// <summary>JWT token factory</summary>
    public class JwtFactory : IJwtFactory
    {
        private readonly JwtIssuerOptions _jwtOptions;

        /// <summary>Create instance of <see cref="JwtFactory" /> and set options</summary>
        /// <param name="jwtOptions">Oprions</param>
        public JwtFactory(IOptions<JwtIssuerOptions> jwtOptions)
        {
            _jwtOptions = jwtOptions.Value;
            ThrowIfInvalidOptions(_jwtOptions);
        }

        /// <inheritdoc />
        public string GenerateToken(string email)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, email),
                new Claim(JwtRegisteredClaimNames.Sub, email),
            };

            // Create the JWT security token and encode it.
            var jwt = new JwtSecurityToken(
                _jwtOptions.Issuer,
                _jwtOptions.Audience,
                claims,
                _jwtOptions.NotBefore,
                _jwtOptions.Expiration,
                _jwtOptions.SigningCredentials);

            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            return encodedJwt;
        }

        private static void ThrowIfInvalidOptions(JwtIssuerOptions options)
        {
            if (options == null)
                throw new ArgumentNullException(nameof(options));
            if (options.ValidFor <= TimeSpan.Zero)
                throw new ArgumentException("Must be a non-zero TimeSpan.", nameof(JwtIssuerOptions.ValidFor));
            if (options.SigningCredentials == null)
                throw new ArgumentNullException(nameof(JwtIssuerOptions.SigningCredentials));
            if (options.JtiGenerator == null)
                throw new ArgumentNullException(nameof(JwtIssuerOptions.JtiGenerator));
        }
    }
}