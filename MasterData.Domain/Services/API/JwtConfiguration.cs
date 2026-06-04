using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace MasterData.Domain.Services.API;

public static class JwtConfiguration
{
    public const string SecretKeyPath = "Jwt:SecretKey";

    public static SymmetricSecurityKey GetSecurityKey(IConfiguration configuration)
    {
        var secretKey = configuration[SecretKeyPath];

        if (string.IsNullOrWhiteSpace(secretKey))
            throw new InvalidOperationException($"Configure a chave JWT em '{SecretKeyPath}'. Em desenvolvimento, " +
                $"prefira user-secrets ou variavel de ambiente Jwt__SecretKey.");


        return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
    }
}
