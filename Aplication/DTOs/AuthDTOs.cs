using System.Collections.Generic;

namespace Aplication.DTOs;

public record LoginRequestDTO(string Email, string Password);
public record AuthResponseDTO(string AccessToken, string RefreshToken, string Cargo, List<string> Permisos);
public record RefreshTokenRequestDTO(string RefreshToken);