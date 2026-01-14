import { Dialog, Transition } from '@headlessui/react';
import React, { Fragment, useState, useEffect } from 'react';
import { useForm } from 'react-hook-form';
import type { Cargo, ManageUserPayload, Usuario } from '../../services/usuario.service';
import { toast } from 'sonner';
import { manageUser } from '../../services/usuario.service';

interface EditUserModalProps {
  isOpen: boolean;
  closeModal: () => void;
  user: Usuario | null;
  cargos: Cargo[];
  onUserUpdate: (updatedUser: Usuario) => void;
}

const allPermissions = [
  'inspeccion:crear',
  'inspeccion:editar',
  'inspeccion:estado',
  'inspeccion:archivo:subir',
  'inspeccion:archivo:borrar',
  'usuario:gestionar',
  'cargo:gestionar',
];

const EditUserModal: React.FC<EditUserModalProps> = ({ isOpen, closeModal, user, cargos, onUserUpdate }) => {
  const { register, handleSubmit, reset, setValue } = useForm<ManageUserPayload>();
  const [selectedPermissions, setSelectedPermissions] = useState<string[]>([]);

  useEffect(() => {
    if (user) {
      const cargo = cargos.find(c => c.nombre === user.cargo);
      setValue('id', user.id);
      setValue('cargoId', cargo?.id || '');
      setValue('estaActivo', user.estaActivo);
      setSelectedPermissions(user.permisos);
    }
  }, [user, cargos, setValue]);

  const handlePermissionChange = (permission: string) => {
    setSelectedPermissions(prev =>
      prev.includes(permission) ? prev.filter(p => p !== permission) : [...prev, permission]
    );
  };

  const onSubmit = async (data: ManageUserPayload) => {
    if (!user) return;

    const payload = { ...data, id: user.id, permisos: selectedPermissions };
    toast.info('Actualizando usuario...');

    try {
      const updatedUser = await manageUser(payload);
      toast.success('Usuario actualizado correctamente.');
      onUserUpdate(updatedUser);
      closeModal();
    } catch (error) {
      console.error(error);
      toast.error('No se pudo actualizar el usuario.');
    }
  };
  
  return (
    <Transition appear show={isOpen} as={Fragment}>
      <Dialog as="div" className="relative z-10" onClose={closeModal}>
        <Transition.Child as={Fragment} enter="ease-out duration-300" enterFrom="opacity-0" enterTo="opacity-100" leave="ease-in duration-200" leaveFrom="opacity-100" leaveTo="opacity-0">
          <div className="fixed inset-0 bg-black bg-opacity-25" />
        </Transition.Child>

        <div className="fixed inset-0 overflow-y-auto">
          <div className="flex min-h-full items-center justify-center p-4 text-center">
            <Transition.Child as={Fragment} enter="ease-out duration-300" enterFrom="opacity-0 scale-95" enterTo="opacity-100 scale-100" leave="ease-in duration-200" leaveFrom="opacity-100 scale-100" leaveTo="opacity-0 scale-95">
              <Dialog.Panel className="w-full max-w-md transform overflow-hidden rounded-2xl bg-white p-6 text-left align-middle shadow-xl transition-all">
                <Dialog.Title as="h3" className="text-lg font-medium leading-6 text-gray-900">
                  Editar Usuario: {user?.nombre}
                </Dialog.Title>
                
                <form onSubmit={handleSubmit(onSubmit)} className="mt-4">
                  <div className="mb-4">
                    <label htmlFor="cargoId" className="block text-sm font-medium text-gray-700">Cargo</label>
                    <select id="cargoId" {...register('cargoId')} className="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:border-indigo-500 focus:ring-indigo-500 sm:text-sm">
                      {cargos.map(cargo => <option key={cargo.id} value={cargo.id}>{cargo.nombre}</option>)}
                    </select>
                  </div>
                  
                  <div className="mb-4">
                    <label className="flex items-center">
                      <input type="checkbox" {...register('estaActivo')} className="h-4 w-4 rounded border-gray-300 text-indigo-600 focus:ring-indigo-500" />
                      <span className="ml-2 text-sm text-gray-900">Usuario Activo</span>
                    </label>
                  </div>

                  <div className="mb-4">
                    <label className="block text-sm font-medium text-gray-700">Permisos</label>
                    <div className="mt-2 grid grid-cols-2 gap-2">
                        {allPermissions.map(permission => (
                            <label key={permission} className="flex items-center">
                                <input 
                                    type="checkbox" 
                                    value={permission}
                                    checked={selectedPermissions.includes(permission)}
                                    onChange={() => handlePermissionChange(permission)}
                                    className="h-4 w-4 rounded border-gray-300 text-indigo-600 focus:ring-indigo-500"
                                />
                                <span className="ml-2 text-sm text-gray-600">{permission}</span>
                            </label>
                        ))}
                    </div>
                  </div>

                  <div className="mt-6 flex justify-end gap-4">
                    <button type="button" onClick={closeModal} className="inline-flex justify-center rounded-md border border-transparent bg-gray-100 px-4 py-2 text-sm font-medium text-gray-700 hover:bg-gray-200 focus:outline-none focus-visible:ring-2 focus-visible:ring-gray-500 focus-visible:ring-offset-2">
                      Cancelar
                    </button>
                    <button type="submit" className="inline-flex justify-center rounded-md border border-transparent bg-indigo-600 px-4 py-2 text-sm font-medium text-white hover:bg-indigo-700 focus:outline-none focus-visible:ring-2 focus-visible:ring-indigo-500 focus-visible:ring-offset-2">
                      Guardar Cambios
                    </button>
                  </div>
                </form>
              </Dialog.Panel>
            </Transition.Child>
          </div>
        </div>
      </Dialog>
    </Transition>
  );
};

export default EditUserModal;
