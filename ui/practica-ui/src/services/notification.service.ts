import api from './api';

export interface Notificacion {
    id: string;
    mensaje: string;
    leido: boolean;
    fecha: string;
    usuarioId: string;
}

/**
 * Obtiene las notificaciones no leídas para el usuario autenticado.
 */
export const getUnreadNotifications = async (): Promise<Notificacion[]> => {
    const response = await api.get('/notificaciones/no-leidas');
    return response.data;
};

/**
 * Marca todas las notificaciones no leídas del usuario como leídas.
 */
export const markNotificationsAsRead = async (): Promise<void> => {
    await api.patch('/notificaciones/marcar-leidas');
};
