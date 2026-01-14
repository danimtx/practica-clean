import React, { useEffect, useState } from 'react';
import { useForm } from 'react-hook-form';
import { useNavigate } from 'react-router-dom';
import { Toaster, toast } from 'sonner';
import { createInspeccion } from '../services/inspeccion.service';
import type { CreateInspeccionPayload } from '../services/inspeccion.service';
import { getUsers, getCargos } from '../services/usuario.service';
import type { Usuario, Cargo } from '../services/usuario.service';
import { useAuthStore } from '../store/auth.store';

const CrearInspeccionPage: React.FC = () => {
  const { register, handleSubmit, formState: { errors } } = useForm<CreateInspeccionPayload>();
  const [users, setUsers] = useState<Usuario[]>([]);
  const [cargos, setCargos] = useState<Cargo[]>([]);
  const [selectedCargo, setSelectedCargo] = useState<string>('');
  const [isLoading, setIsLoading] = useState(false);
  const navigate = useNavigate();
  const { userProfile } = useAuthStore();

  const canCreate = userProfile?.permisos.includes('inspeccion:crear');

  useEffect(() => {
    if (!canCreate) return;
    
    getCargos()
      .then(setCargos)
      .catch(() => toast.error('No se pudieron cargar los cargos.'));
  }, [canCreate]);

  useEffect(() => {
    if (selectedCargo) {
      getUsers(selectedCargo)
        .then(setUsers)
        .catch(() => toast.error(`No se pudieron cargar los usuarios con el cargo ${selectedCargo}.`));
    } else {
      setUsers([]);
    }
  }, [selectedCargo]);

  const onSubmit = async (data: CreateInspeccionPayload) => {
    setIsLoading(true);
    toast.info('Creando inspección...');
    try {
      await createInspeccion(data);
      toast.success('Inspección creada y asignada correctamente.');
      navigate('/inspecciones');
    } catch (error) {
      console.error(error);
      toast.error('No se pudo crear la inspección.');
    } finally {
      setIsLoading(false);
    }
  };

  if (!canCreate) {
    return <div className="text-red-500 font-bold text-center mt-10">No tienes permiso para crear inspecciones.</div>;
  }

  return (
    <div className="container mx-auto p-4">
      <Toaster />
      <h1 className="text-3xl font-bold mb-6">Crear Nueva Inspección</h1>
      <div className="bg-white p-8 rounded-lg shadow-md max-w-2xl mx-auto">
        <form onSubmit={handleSubmit(onSubmit)}>
          <div className="mb-4">
            <label htmlFor="nombreCliente" className="block text-gray-700 font-bold mb-2">Nombre del Cliente</label>
            <input {...register('nombreCliente', { required: 'El nombre del cliente es obligatorio' })} className="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700" />
            {errors.nombreCliente && <p className="text-red-500 text-xs mt-1">{errors.nombreCliente.message}</p>}
          </div>

          <div className="mb-4">
            <label htmlFor="direccion" className="block text-gray-700 font-bold mb-2">Dirección</label>
            <input {...register('direccion', { required: 'La dirección es obligatoria' })} className="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700" />
            {errors.direccion && <p className="text-red-500 text-xs mt-1">{errors.direccion.message}</p>}
          </div>

          <div className="mb-4">
            <label htmlFor="cargo" className="block text-gray-700 font-bold mb-2">Filtrar por Cargo</label>
            <select id="cargo" onChange={(e) => setSelectedCargo(e.target.value)} className="shadow border rounded w-full py-2 px-3 text-gray-700">
              <option value="">Seleccione un cargo...</option>
              {cargos.map(c => <option key={c.id} value={c.nombre}>{c.nombre}</option>)}
            </select>
          </div>

          <div className="mb-4">
            <label htmlFor="usuarioId" className="block text-gray-700 font-bold mb-2">Asignar a Usuario</label>
            <select {...register('usuarioId', { required: 'Debe asignar un usuario' })} className="shadow border rounded w-full py-2 px-3 text-gray-700">
              <option value="">Seleccione un usuario...</option>
              {users.map(u => <option key={u.id} value={u.id}>{u.nombre}</option>)}
            </select>
            {errors.usuarioId && <p className="text-red-500 text-xs mt-1">{errors.usuarioId.message}</p>}
          </div>

          <div className="mb-4">
            <label htmlFor="detallesTecnicos" className="block text-gray-700 font-bold mb-2">Detalles Técnicos</label>
            <textarea {...register('detallesTecnicos')} rows={4} className="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700"></textarea>
          </div>

          <div className="mb-6">
            <label htmlFor="observaciones" className="block text-gray-700 font-bold mb-2">Observaciones</label>
            <textarea {...register('observaciones')} rows={4} className="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700"></textarea>
          </div>

          <button type="submit" disabled={isLoading} className={`w-full bg-indigo-600 text-white font-bold py-2 px-4 rounded hover:bg-indigo-700 disabled:opacity-50`}>
            {isLoading ? 'Creando...' : 'Crear Inspección'}
          </button>
        </form>
      </div>
    </div>
  );
};


export default CrearInspeccionPage;
