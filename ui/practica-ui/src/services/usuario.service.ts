import api from './api';

// --- Tipos de Datos ---
export interface UserProfile {
  id: string;
  nombre: string;
  email: string;
  cargo: string;
  permisos: string[];
  fotoPerfil?: string;
}

export interface Usuario {
  id: string;
  nombre: string;
  email: string;
  cargo: string;
  estaActivo: boolean;
  permisos: string[];
}

export interface Cargo {
  id: string;
  nombre: string;
}

export interface ManageUserPayload {
    id: string;
    cargoId: string;
    estaActivo: boolean;
    permisos: string[];
}

export interface UsuarioRegistroDTO {
    nombre: string;
    email: string;
    password: string;
}

// --- Funciones de Servicio ---

/**
 * Registra un nuevo usuario.
 */
export const registerUser = async (payload: UsuarioRegistroDTO): Promise<Usuario> => {
    const response = await api.post('/usuarios/registro', payload);
    return response.data;
};


/**
 * Actualiza el perfil del usuario (nombre y/o contraseña).
 */
export const updateProfile = async (nombre: string, password?: string) => {
  const payload: { nombre: string; password?: string } = { nombre };
  if (password) {
    payload.password = password;
  }
  const response = await api.put<UserProfile>('/usuarios/perfil', payload);
  return response.data;
};

/**
 * Sube o actualiza la foto de perfil del usuario.
 */
export const uploadProfilePhoto = async (formData: FormData) => {
  const response = await api.post<UserProfile>('/usuarios/foto-perfil', formData, {
    headers: {
      'Content-Type': 'multipart/form-data',
    },
  });
  return response.data;
};

/**
 * Obtiene la lista de todos los usuarios.
 */
export const getCargos = async (): Promise<Cargo[]> => {
  // Asegúrate de tener el CargosController en el backend
  const response = await api.get('/cargos'); 
  return response.data;
};

// 2. Obtener Usuarios (Con filtro opcional)
export const getUsers = async (cargo?: string): Promise<Usuario[]> => {
  // Axios eliminará "cargo" de la URL si es undefined
  const response = await api.get('/usuarios', { params: { cargo } });
  return response.data;
};

/**
 * Modifica los datos de un usuario (para Admins).
 */
export const manageUser = async (payload: ManageUserPayload): Promise<Usuario> => {
    const response = await api.put(`/usuarios/gestionar`, payload);
    return response.data;
};


