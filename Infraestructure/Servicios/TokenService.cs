using Domain.Entities;
using Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Infraestructure.Servicios
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _config;

        public TokenService(IConfiguration config)
        {
            _config = config;
        }

        public string GenerarAccessToken(Usuario usuario)
        {
            // TODO: Mover el 'Secret_key' a la configuración (appsettings.json)
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("Super_secret_key_that_is_at_least_32_chars_long"));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Sub, usuario.Id.ToString()),
                new(JwtRegisteredClaimNames.Email, usuario.Email),
                new(ClaimTypes.Name, usuario.Nombre),
                new(ClaimTypes.Role, usuario.Cargo?.Nombre ?? "Invitado"),
                new("cargo", usuario.Cargo?.Nombre ?? "Invitado"),
                new("permisos", string.Join(",", usuario.Permisos))
            };

            var token = new JwtSecurityToken(
                issuer: "yourdomain.com",  // TODO: Mover a config
                audience: "yourdomain.com", // TODO: Mover a config
                claims: claims,
                expires: DateTime.Now.AddMinutes(15),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public string GenerarRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
    }
}
