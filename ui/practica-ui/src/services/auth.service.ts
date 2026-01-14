import axios from 'axios';
import api from './api';

// Tipos de datos basados en el PRD y los DTOs del backend
interface LoginResponse {
  accessToken: string;
  refreshToken: string;
}

interface UserProfile {
  id: string;
  nombre: string;
  email: string;
  cargo: string;
  permisos: string[];
  fotoPerfil?: string;
}

// --- Funciones de Servicio ---

/**
 * Realiza la petición de login.
 * @param email - Email del usuario.
 * @param password - Contraseña del usuario.
 * @returns Los tokens de acceso y refresco.
 */
export const login = async (email: string, password: string) => {
  const response = await api.post<LoginResponse>('/usuarios/login', { email, password });
  return response.data;
};

/**
 * Obtiene el perfil del usuario autenticado.
 * @returns El perfil del usuario.
 */
export const getProfile = async () => {
  const response = await api.get<UserProfile>('/usuarios/perfil');
  return response.data;
};

/**
 * Refresca el token de acceso usando el token de refresco.
 * Esta función usa una instancia de axios separada para evitar el bucle de interceptores.
 * @param currentRefreshToken - El token de refresco actual.
 * @returns Un nuevo par de tokens.
 */
export const refreshToken = async (currentRefreshToken: string) => {
    // Usamos una instancia de axios sin interceptores para la llamada de refresh
    // para evitar un bucle infinito si el refresh token también es inválido.
    const response = await axios.post<LoginResponse>(
      `${api.defaults.baseURL}/usuarios/refresh`, 
      { refreshToken: currentRefreshToken }
    );
    
    // El interceptor de Axios se encargará de guardar los nuevos tokens.
    return response.data;
};
