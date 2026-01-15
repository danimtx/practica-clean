import api from './api';

export interface Cargo {
  id: string;
  nombre: string;
}

export interface CargoDTO {
  nombre: string;
}

// Obtener todos los cargos
export const getCargos = async (): Promise<Cargo[]> => {
  const response = await api.get('/cargos');
  return response.data;
};

// Crear un nuevo cargo
export const createCargo = async (data: CargoDTO): Promise<Cargo> => {
  const response = await api.post('/cargos', data);
  return response.data;
};

// Eliminar un cargo
export const deleteCargo = async (id: string): Promise<void> => {
  await api.delete(`/cargos/${id}`);
};