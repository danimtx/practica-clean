import React from 'react';
import { Link, useLocation } from 'react-router-dom';
import { useAuthStore } from '../../store/auth.store';

type NavLinkDef = {
  to: string;
  label: string;
  requiredPermission?: string;
  requiredCargo?: string;
};

const navLinks: NavLinkDef[] = [
  { to: '/', label: 'Dashboard' },
  { to: '/inspecciones', label: 'Ver Inspecciones', requiredPermission: 'usuario:gestionar' }, // Vista de Admin
  { to: '/mis-inspecciones', label: 'Mis Inspecciones' },
  { to: '/inspecciones/crear', label: 'Crear Inspección', requiredPermission: 'inspeccion:crear' },
  { to: '/gestion-usuarios', label: 'Gestión de Usuarios', requiredPermission: 'usuario:gestionar' },
  { to: '/gestion-cargos', label: 'Gestión de Cargos', requiredPermission: 'cargo:gestionar' },
];

interface SidebarProps {
  isOpen: boolean;
  toggleSidebar: () => void;
}

const Sidebar: React.FC<SidebarProps> = ({ isOpen, toggleSidebar }) => {
  const { userProfile } = useAuthStore();
  const userPermissions = userProfile?.permisos || [];
  const userCargo = userProfile?.cargo || '';
  const location = useLocation();

  const accessibleLinks = navLinks.filter(link => {
    const hasPermission = !link.requiredPermission || userPermissions.includes(link.requiredPermission);
    const hasCargo = !link.requiredCargo || userCargo === link.requiredCargo;
    return hasPermission && hasCargo;
  });

  const NavLink: React.FC<NavLinkDef> = ({ to, label }) => {
    const isActive = location.pathname === to;
    return (
      <Link
        to={to}
        onClick={toggleSidebar} // Cierra el sidebar al hacer clic en un enlace en móvil
        className={`block py-2.5 px-4 rounded transition duration-200 ${
          isActive
            ? 'bg-indigo-600 text-white'
            : 'hover:bg-gray-700 hover:text-white'
        }`}
      >
        {label}
      </Link>
    );
  };

  return (
    <aside
      className={`fixed inset-y-0 left-0 z-30 w-64 bg-gray-800 text-white transform ${
        isOpen ? 'translate-x-0' : '-translate-x-full'
      } transition-transform duration-300 ease-in-out lg:relative lg:translate-x-0`}
    >
      <div className="flex items-center justify-between p-6">
        <h2 className="text-2xl font-semibold text-white">Cybercorp</h2>
        <button onClick={toggleSidebar} className="lg:hidden text-white focus:outline-none">
          <svg className="w-6 h-6" fill="none" stroke="currentColor" viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg"><path strokeLinecap="round" strokeLinejoin="round" strokeWidth="2" d="M6 18L18 6M6 6l12 12"></path></svg>
        </button>
      </div>
      <nav className="mt-6 px-4">
        {accessibleLinks.map(link => (
          <NavLink key={link.to} {...link} />
        ))}
      </nav>
    </aside>
  );
};

export default Sidebar;
