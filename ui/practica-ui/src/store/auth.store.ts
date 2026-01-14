import { create } from 'zustand';

// Define el tipo para el perfil del usuario según el PRD
type UserProfile = {
  id: string;
  nombre: string;
  email: string;
  cargo: string;
  permisos: string[];
  fotoPerfil?: string;
};

// Define el tipo para el estado de autenticación
type AuthState = {
  accessToken: string | null;
  refreshToken: string | null;
  isAuthenticated: boolean;
  userProfile: UserProfile | null;
  setTokens: (accessToken: string, refreshToken: string) => void;
  setUserProfile: (profile: UserProfile) => void;
  logout: () => void;
};

export const useAuthStore = create<AuthState>((set) => ({
  accessToken: null,
  refreshToken: null,
  isAuthenticated: false,
  userProfile: null,
  setTokens: (accessToken, refreshToken) => {
    set({
      accessToken,
      refreshToken,
      isAuthenticated: !!accessToken && !!refreshToken,
    });
  },
  setUserProfile: (profile) => {
    // Si los permisos vienen como un string, los convertimos a un array
    const permisosArray = typeof profile.permisos === 'string'
      ? (profile.permisos as any).split(',')
      : profile.permisos;

    set({ userProfile: { ...profile, permisos: permisosArray } });
  },
  logout: () => {
    set({
      accessToken: null,
      refreshToken: null,
      isAuthenticated: false,
      userProfile: null,
    });
  },
}));
