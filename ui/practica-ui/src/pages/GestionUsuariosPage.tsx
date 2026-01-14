import React, { useEffect, useState } from 'react';
import type { Usuario, Cargo } from '../services/usuario.service';
import { getUsers, getCargos } from '../services/usuario.service';
import EditUserModal from '../components/modals/EditUserModal';
import { useAuthStore } from '../store/auth.store';
import { Toaster, toast } from 'sonner';

import CreateUserModal from '../components/modals/CreateUserModal';

const GestionUsuariosPage: React.FC = () => {
  const [users, setUsers] = useState<Usuario[]>([]);
  const [cargos, setCargos] = useState<Cargo[]>([]);
  const [isLoading, setIsLoading] = useState(true);
  const [selectedUser, setSelectedUser] = useState<Usuario | null>(null);
  const [isEditModalOpen, setEditModalOpen] = useState(false);
  const [isCreateModalOpen, setCreateModalOpen] = useState(false);
  const { userProfile } = useAuthStore();

  const canManageUsers = userProfile?.permisos.includes('usuario:gestionar');

  useEffect(() => {
    if (!canManageUsers) {
      setIsLoading(false);
      return;
    }

    const fetchData = async () => {
      try {
        setIsLoading(true);
        const [usersData, cargosData] = await Promise.all([getUsers(), getCargos()]);
        setUsers(usersData);
        setCargos(cargosData);
      } catch (error) {
        console.error(error);
        toast.error('No se pudieron cargar los datos.');
      } finally {
        setIsLoading(false);
      }
    };

    fetchData();
  }, [canManageUsers]);

  const openEditModal = (user: Usuario) => {
    setSelectedUser(user);
    setEditModalOpen(true);
  };

  const closeEditModal = () => {
    setEditModalOpen(false);
    setSelectedUser(null);
  };

  const openCreateModal = () => {
    setCreateModalOpen(true);
  };

  const closeCreateModal = () => {
    setCreateModalOpen(false);
  };

  const handleUserUpdate = (updatedUser: Usuario) => {
    setUsers(prevUsers => prevUsers.map(u => u.id === updatedUser.id ? updatedUser : u));
  };

  const handleUserCreate = (newUser: Usuario) => {
    setUsers(prevUsers => [...prevUsers, newUser]);
  };
  
  if (!canManageUsers) {
    return <div className="text-red-500 font-bold text-center mt-10">No tienes permiso para gestionar usuarios.</div>;
  }

  if (isLoading) {
    return <div>Cargando usuarios...</div>;
  }

  return (
    <div className="container mx-auto p-4">
      <Toaster />
      <div className="flex justify-between items-center mb-6">
        <h1 className="text-3xl font-bold">Gestión de Usuarios</h1>
        <button
          onClick={openCreateModal}
          className="bg-green-500 hover:bg-green-700 text-white font-bold py-2 px-4 rounded"
        >
          Crear Usuario
        </button>
      </div>
      <div className="bg-white p-6 rounded-lg shadow-md overflow-x-auto">
        <table className="min-w-full divide-y divide-gray-200">
          <thead className="bg-gray-50">
            <tr>
              <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Nombre</th>
              <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Email</th>
              <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Cargo</th>
              <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Estado</th>
              <th className="px-6 py-3 text-right text-xs font-medium text-gray-500 uppercase tracking-wider">Acciones</th>
            </tr>
          </thead>
          <tbody className="bg-white divide-y divide-gray-200">
            {users.map((user) => (
              <tr key={user.id}>
                <td className="px-6 py-4 whitespace-nowrap">{user.nombre}</td>
                <td className="px-6 py-4 whitespace-nowrap">{user.email}</td>
                <td className="px-6 py-4 whitespace-nowrap">{user.cargo}</td>
                <td className="px-6 py-4 whitespace-nowrap">
                  <span className={`px-2 inline-flex text-xs leading-5 font-semibold rounded-full ${
                      user.estaActivo ? 'bg-green-100 text-green-800' : 'bg-red-100 text-red-800'
                    }`}>
                    {user.estaActivo ? 'Activo' : 'Inactivo'}
                  </span>
                </td>
                <td className="px-6 py-4 whitespace-nowrap text-right text-sm font-medium">
                  {user.id !== userProfile?.id && (
                    <button 
                      onClick={() => openEditModal(user)} 
                      className="text-indigo-600 hover:text-indigo-900"
                      // No se puede editar a un superadmin si no lo eres
                      disabled={user.cargo === 'SuperAdmin' && userProfile?.cargo !== 'SuperAdmin'}
                    >
                      Editar
                    </button>
                  )}
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>
      
      {selectedUser && (
        <EditUserModal 
          isOpen={isEditModalOpen}
          closeModal={closeEditModal}
          user={selectedUser}
          cargos={cargos}
          onUserUpdate={handleUserUpdate}
        />
      )}

      <CreateUserModal
        isOpen={isCreateModalOpen}
        closeModal={closeCreateModal}
        cargos={cargos}
        onUserCreate={handleUserCreate}
      />
    </div>
  );
};

export default GestionUsuariosPage;