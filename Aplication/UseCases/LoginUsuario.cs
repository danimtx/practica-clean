using Aplication.DTOs;
using Domain.Interfaces;
using System.Threading.Tasks;

namespace Aplication.UseCases
{
    public class LoginUsuario
    {
        private readonly IUsuarioRepositorio _repo;
        private readonly ITokenService _tokenService;

        public LoginUsuario(IUsuarioRepositorio repo, ITokenService tokenService)
        {
            _repo = repo;
            _tokenService = tokenService;
        }

        public async Task<AuthResponseDTO?> Ejecutar(LoginRequestDTO request)
        {
            var usuario = await _repo.ObtenerPorEmailAsync(request.Email);
            
            if (usuario == null || usuario.PasswordHash != request.Password || !usuario.EstaActivo) return null;

            var refreshToken = _tokenService.GenerarRefreshToken();
            usuario.RefreshToken = refreshToken;
            usuario.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);

            await _repo.ActualizarUsuarioAsync(usuario);
            
            return new AuthResponseDTO(
                _tokenService.GenerarAccessToken(usuario),
                refreshToken,
                usuario.Cargo,
                usuario.Permisos
            );
        }
    }
}