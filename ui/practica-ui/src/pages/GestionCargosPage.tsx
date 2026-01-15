import React, { useEffect, useState } from 'react';
import { Toaster, toast } from 'sonner';
import { getCargos, deleteCargo, Cargo } from '../services/cargo.service';
import CreateCargoModal from '../components/modals/CreateCargoModal';
import { useAuthStore } from '../store/auth.store';

const GestionCargosPage: React.FC = () => {
  const [cargos, setCargos] = useState<Cargo[]>([]);
  const [isLoading, setIsLoading] = useState(true);
  const [isCreateModalOpen, setCreateModalOpen] = useState(false);
  
  // Obtenemos el perfil para verificar permisos si es necesario
  const { userProfile } = useAuthStore();
  // Puedes ajustar este permiso según tu backend, asumo que usas 'cargo:gestionar' o es SuperAdmin
  const canManageCargos = userProfile?.permisos.includes('cargo:gestionar') || userProfile?.cargo === 'SuperAdmin';

  useEffect(() => {
    fetchCargos();
  }, []);

  const fetchCargos = async () => {
    try {
      setIsLoading(true);
      const data = await getCargos();
      setCargos(data);
    } catch (error) {
      console.error(error);
      toast.error('Error al cargar los cargos.');
    } finally {
      setIsLoading(false);
    }
  };

  const handleCreate = (newCargo: Cargo) => {
    // Agregamos el nuevo cargo a la lista sin recargar
    setCargos([...cargos, newCargo]);
  };

  const handleDelete = async (id: string, nombre: string) => {
    if (nombre === 'SuperAdmin') {
      toast.error('No se puede eliminar el cargo SuperAdmin.');
      return;
    }

    if (!window.confirm(`¿Estás seguro de eliminar el cargo "${nombre}"?`)) return;

    try {
      await deleteCargo(id);
      setCargos(cargos.filter(c => c.id !== id));
      toast.success('Cargo eliminado correctamente.');
    } catch (error) {
      console.error(error);
      toast.error('No se pudo eliminar el cargo. Puede que tenga usuarios asignados.');
    }
  };

  if (isLoading) return <div className="p-4">Cargando cargos...</div>;

  if (!canManageCargos) {
      return <div className="p-4 text-red-500 text-center">No tienes permisos para ver esta sección.</div>;
  }

  return (
    <div className="container mx-auto p-4">
      <Toaster position="top-right" />
      
      <div className="flex justify-between items-center mb-6">
        <h1 className="text-3xl font-bold text-gray-800">Gestión de Cargos</h1>
        <button
          onClick={() => setCreateModalOpen(true)}
          className="bg-green-600 hover:bg-green-700 text-white font-bold py-2 px-4 rounded shadow transition-colors"
        >
          + Nuevo Cargo
        </button>
      </div>

      <div className="bg-white p-6 rounded-lg shadow-md overflow-hidden">
        <div className="overflow-x-auto">
          <table className="min-w-full divide-y divide-gray-200">
            <thead className="bg-gray-50">
              <tr>
                <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                  Nombre del Cargo
                </th>
                <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                  ID
                </th>
                <th className="px-6 py-3 text-right text-xs font-medium text-gray-500 uppercase tracking-wider">
                  Acciones
                </th>
              </tr>
            </thead>
            <tbody className="bg-white divide-y divide-gray-200">
              {cargos.length === 0 ? (
                <tr>
                  <td colSpan={3} className="px-6 py-4 text-center text-gray-500">
                    No hay cargos registrados.
                  </td>
                </tr>
              ) : (
                cargos.map((cargo) => (
                  <tr key={cargo.id} className="hover:bg-gray-50 transition-colors">
                    <td className="px-6 py-4 whitespace-nowrap text-sm font-medium text-gray-900">
                      {cargo.nombre}
                    </td>
                    <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-400 font-mono text-xs">
                      {cargo.id}
                    </td>
                    <td className="px-6 py-4 whitespace-nowrap text-right text-sm font-medium">
                      {cargo.nombre !== 'SuperAdmin' && (
                        <button
                          onClick={() => handleDelete(cargo.id, cargo.nombre)}
                          className="text-red-600 hover:text-red-900 bg-red-50 px-3 py-1 rounded-md hover:bg-red-100 transition-colors"
                        >
                          Eliminar
                        </button>
                      )}
                    </td>
                  </tr>
                ))
              )}
            </tbody>
          </table>
        </div>
      </div>

      <CreateCargoModal
        isOpen={isCreateModalOpen}
        closeModal={() => setCreateModalOpen(false)}
        onCargoCreate={handleCreate}
      />
    </div>
  );
};

export default GestionCargosPage;