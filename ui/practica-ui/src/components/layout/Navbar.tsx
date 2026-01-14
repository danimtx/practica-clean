import React, { useState, Fragment } from 'react';
import { useAuthStore } from '../../store/auth.store';
import { useNavigate, Link } from 'react-router-dom';
import { Menu, Transition } from '@headlessui/react';
import NotificationBell from './NotificationBell';

const Navbar: React.FC<{ toggleSidebar: () => void }> = ({ toggleSidebar }) => {
  const { userProfile, logout } = useAuthStore();
  const navigate = useNavigate();
  const API_URL = 'http://localhost:5012/';

  const handleLogout = () => {
    logout();
    navigate('/login');
  };

  return (
    <header className="bg-white shadow-md p-4 flex justify-between items-center">
      <div>
        {/* Botón para abrir/cerrar sidebar en móvil */}
        <button onClick={toggleSidebar} className="text-gray-500 focus:outline-none lg:hidden">
          <svg className="w-6 h-6" viewBox="0 0 24 24" fill="none" xmlns="http://www.w3.org/2000/svg">
            <path d="M4 6H20M4 12H20M4 18H11Z" stroke="currentColor" strokeWidth="2" strokeLinecap="round" strokeLinejoin="round" />
          </svg>
        </button>
      </div>
      <div className="flex items-center gap-4">
        <NotificationBell />
        <span className="mr-4">Hola, {userProfile?.nombre}</span>
        <Menu as="div" className="relative">
          <div>
            <Menu.Button className="block h-10 w-10 rounded-full overflow-hidden border-2 border-gray-300 focus:outline-none focus:border-indigo-500">
              <img className="h-full w-full object-cover" src={userProfile?.fotoPerfil ? `${API_URL}${userProfile.fotoPerfil}?t=${new Date().getTime()}` : `https://ui-avatars.com/api/?name=${userProfile?.nombre}&background=random`} alt="Foto de perfil" />
            </Menu.Button>
          </div>
          <Transition
            as={Fragment}
            enter="transition ease-out duration-100"
            enterFrom="transform opacity-0 scale-95"
            enterTo="transform opacity-100 scale-100"
            leave="transition ease-in duration-75"
            leaveFrom="transform opacity-100 scale-100"
            leaveTo="transform opacity-0 scale-95"
          >
            <Menu.Items className="absolute right-0 mt-2 w-48 bg-white rounded-md overflow-hidden shadow-xl z-10">
              <Menu.Item>
                {({ active }) => (
                  <Link
                    to="/perfil"
                    className={`block px-4 py-2 text-sm ${active ? 'bg-indigo-500 text-white' : 'text-gray-700'}`}
                  >
                    Editar Perfil
                  </Link>
                )}
              </Menu.Item>
              <Menu.Item>
                {({ active }) => (
                  <button
                    onClick={handleLogout}
                    className={`w-full text-left block px-4 py-2 text-sm ${active ? 'bg-indigo-500 text-white' : 'text-gray-700'}`}
                  >
                    Cerrar Sesión
                  </button>
                )}
              </Menu.Item>
            </Menu.Items>
          </Transition>
        </Menu>
      </div>
    </header>
  );
};

export default Navbar;
