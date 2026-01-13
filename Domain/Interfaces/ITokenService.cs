namespace Domain.Interfaces;
public interface ITokenService {
    string GenerarAccessToken(Domain.Entities.Usuario usuario);
    string GenerarRefreshToken();
}