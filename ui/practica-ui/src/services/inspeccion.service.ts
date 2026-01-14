import api from './api';
import type { Usuario } from './usuario.service';

// --- Tipos de Datos ---
export interface Inspeccion {
    id: string;
    nombreCliente: string;
    direccion: string;
    fechaRegistro: string;
    detallesTecnicos: string;
    observaciones: string;
    rutaArchivoPdf?: string;
    estado: string;
    tecnico?: Usuario;
}

export interface CreateInspeccionPayload {
    nombreCliente: string;
    direccion: string;
    detallesTecnicos: string;
    observaciones: string;
    usuarioId: string; // ID del técnico asignado
}

// --- Funciones de Servicio ---

/**
 * Obtiene la lista de usuarios con el cargo de "Técnico".
 */
export const getTecnicos = async (): Promise<Usuario[]> => {
    const response = await api.get('/usuarios?cargo=Tecnico');
    return response.data;
};

/**
 * Crea una nueva inspección.
 */
export const createInspeccion = async (payload: CreateInspeccionPayload): Promise<Inspeccion> => {
    const response = await api.post('/inspecciones', payload);
    return response.data;
};

/**
 * Obtiene las inspecciones asignadas al técnico autenticado.
 */
export const getMisInspecciones = async (): Promise<Inspeccion[]> => {
    const response = await api.get('/inspecciones/mis-inspecciones');
    return response.data;
};

/**
 * Obtiene los detalles de una inspección específica.
 */
export const getInspeccionById = async (id: string): Promise<Inspeccion> => {
    const response = await api.get(`/inspecciones/${id}`);
    return response.data;
};

/**
 * Sube el archivo PDF para una inspección.
 */
export const uploadInspeccionFile = async (id: string, formData: FormData): Promise<Inspeccion> => {
    const response = await api.post(`/inspecciones/${id}/archivo`, formData, {
        headers: { 'Content-Type': 'multipart/form-data' },
    });
    return response.data;
};

/**
 * Actualiza el estado de una inspección.
 */
export const updateInspeccionEstado = async (id: string, estado: string): Promise<Inspeccion> => {
    const response = await api.patch(`/inspecciones/${id}/estado`, { estado });
    return response.data;
};
