using Aplication.DTOs;
using Domain.Interfaces;
using System;
using System.Threading.Tasks;

namespace Aplication.UseCases
{
    public class RefrescarToken
    {
        private readonly IUsuarioRepositorio _userRepository;
        private readonly ITokenService _tokenService;

        public RefrescarToken(IUsuarioRepositorio userRepository, ITokenService tokenService)
        {
            _userRepository = userRepository;
            _tokenService = tokenService;
        }

        public async Task<AuthResponseDTO?> Ejecutar(RefreshTokenRequestDTO request)
        {
            var user = await _userRepository.ObtenerPorRefreshTokenAsync(request.RefreshToken);

            if (user == null || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
            {
                return null;
            }

            var newAccessToken = _tokenService.GenerarAccessToken(user);
            var newRefreshToken = _tokenService.GenerarRefreshToken();

            user.RefreshToken = newRefreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
            await _userRepository.ActualizarUsuarioAsync(user);

            return new AuthResponseDTO(
                newAccessToken,
                newRefreshToken,
                user.Cargo,
                user.Permisos
            );
        }
    }
}
