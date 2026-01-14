import { Dialog, Transition } from '@headlessui/react';
import React, { Fragment, useState } from 'react';
import { useForm } from 'react-hook-form';
import { toast } from 'sonner';
import { registerUser } from '../../services/usuario.service';
import type { Usuario, UsuarioRegistroDTO, Cargo } from '../../services/usuario.service';

interface CreateUserModalProps {
  isOpen: boolean;
  closeModal: () => void;
  cargos: Cargo[];
  onUserCreate: (newUser: Usuario) => void;
}

const CreateUserModal: React.FC<CreateUserModalProps> = ({ isOpen, closeModal, cargos, onUserCreate }) => {
  const { register, handleSubmit, formState: { errors } } = useForm<UsuarioRegistroDTO>();

  const onSubmit = async (data: UsuarioRegistroDTO) => {
    toast.info('Creando usuario...');

    try {
      const newUser = await registerUser(data);
      toast.success('Usuario creado correctamente.');
      onUserCreate(newUser);
      closeModal();
    } catch (error) {
      console.error(error);
      toast.error('No se pudo crear el usuario.');
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
                  Crear Nuevo Usuario
                </Dialog.Title>
                
                <form onSubmit={handleSubmit(onSubmit)} className="mt-4">
                  <div className="mb-4">
                    <label htmlFor="nombre" className="block text-sm font-medium text-gray-700">Nombre</label>
                    <input id="nombre" {...register('nombre', { required: 'El nombre es obligatorio' })} className="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:border-indigo-500 focus:ring-indigo-500 sm:text-sm" />
                    {errors.nombre && <p className="text-red-500 text-xs mt-1">{errors.nombre.message}</p>}
                  </div>
                  
                  <div className="mb-4">
                    <label htmlFor="email" className="block text-sm font-medium text-gray-700">Email</label>
                    <input id="email" type="email" {...register('email', { required: 'El email es obligatorio', pattern: { value: /^\S+@\S+$/i, message: 'Email inválido' } })} className="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:border-indigo-500 focus:ring-indigo-500 sm:text-sm" />
                    {errors.email && <p className="text-red-500 text-xs mt-1">{errors.email.message}</p>}
                  </div>

                  <div className="mb-4">
                    <label htmlFor="password" className="block text-sm font-medium text-gray-700">Contraseña</label>
                    <input id="password" type="password" {...register('password', { required: 'La contraseña es obligatoria' })} className="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:border-indigo-500 focus:ring-indigo-500 sm:text-sm" />
                    {errors.password && <p className="text-red-500 text-xs mt-1">{errors.password.message}</p>}
                  </div>

                  <div className="mb-4">
                    <label htmlFor="cargoId" className="block text-sm font-medium text-gray-700">Cargo</label>
                    <select id="cargoId" {...register('cargoId')} className="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:border-indigo-500 focus:ring-indigo-500 sm:text-sm">
                      <option value="">(Opcional) Por defecto: Invitado</option>
                      {cargos.map(cargo => <option key={cargo.id} value={cargo.id}>{cargo.nombre}</option>)}
                    </select>
                  </div>

                  <div className="mt-6 flex justify-end gap-4">
                    <button type="button" onClick={closeModal} className="inline-flex justify-center rounded-md border border-transparent bg-gray-100 px-4 py-2 text-sm font-medium text-gray-700 hover:bg-gray-200 focus:outline-none focus-visible:ring-2 focus-visible:ring-gray-500 focus-visible:ring-offset-2">
                      Cancelar
                    </button>
                    <button type="submit" className="inline-flex justify-center rounded-md border border-transparent bg-indigo-600 px-4 py-2 text-sm font-medium text-white hover:bg-indigo-700 focus:outline-none focus-visible:ring-2 focus-visible:ring-indigo-500 focus-visible:ring-offset-2">
                      Crear Usuario
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

export default CreateUserModal;
