import React, { Fragment, useEffect } from 'react';
import { Menu, Transition } from '@headlessui/react';
import { useNotificationStore } from '../../store/notification.store';
import { getUnreadNotifications, markNotificationsAsRead } from '../../services/notification.service';
import { toast } from 'sonner';

const NotificationBell: React.FC = () => {
    const { notifications, unreadCount, setNotifications, markAllAsRead } = useNotificationStore();

    useEffect(() => {
        getUnreadNotifications()
            .then(setNotifications)
            .catch(() => toast.error('No se pudieron cargar las notificaciones.'));
    }, [setNotifications]);

    const handleMarkAsRead = () => {
        if (unreadCount > 0) {
            markNotificationsAsRead()
                .then(() => {
                    markAllAsRead();
                })
                .catch(() => toast.error('No se pudieron marcar las notificaciones como leídas.'));
        }
    };

    return (
        <Menu as="div" className="relative">
            <Menu.Button className="relative text-gray-500 hover:text-gray-700 focus:outline-none">
                <svg className="w-6 h-6" viewBox="0 0 24 24" fill="none" xmlns="http://www.w3.org/2000/svg">
                    <path d="M18 8C18 6.4087 17.3679 4.88258 16.2426 3.75736C15.1174 2.63214 13.5913 2 12 2C10.4087 2 8.88258 2.63214 7.75736 3.75736C6.63214 4.88258 6 6.4087 6 8C6 15 3 17 3 17H21C21 17 18 15 18 8Z" stroke="currentColor" strokeWidth="2" strokeLinecap="round" strokeLinejoin="round"/>
                    <path d="M13.73 21C13.5542 21.3031 13.3019 21.5547 12.9982 21.7295C12.6946 21.9044 12.3504 22 12 22C11.6496 22 11.3054 21.9044 11.0018 21.7295C10.6982 21.5547 10.4458 21.3031 10.27 21" stroke="currentColor" strokeWidth="2" strokeLinecap="round" strokeLinejoin="round"/>
                </svg>
                {unreadCount > 0 && (
                    <span className="absolute -top-1 -right-1 flex h-5 w-5">
                        <span className="animate-ping absolute inline-flex h-full w-full rounded-full bg-red-400 opacity-75"></span>
                        <span className="relative inline-flex rounded-full h-5 w-5 bg-red-500 text-white text-xs items-center justify-center">
                            {unreadCount}
                        </span>
                    </span>
                )}
            </Menu.Button>
            <Transition
                as={Fragment}
                enter="transition ease-out duration-100"
                enterFrom="transform opacity-0 scale-95"
                enterTo="transform opacity-100 scale-100"
                leave="transition ease-in duration-75"
                leaveFrom="transform opacity-100 scale-100"
                leaveTo="transform opacity-0 scale-95"
                // Llama a la función para marcar como leídas al cerrar el menú
                afterLeave={handleMarkAsRead}
            >
                <Menu.Items className="absolute right-0 mt-2 w-80 origin-top-right bg-white divide-y divide-gray-100 rounded-md shadow-lg ring-1 ring-black ring-opacity-5 focus:outline-none">
                    <div className="px-4 py-3">
                        <p className="text-sm font-medium text-gray-900">Notificaciones</p>
                    </div>
                    <div className="py-1 max-h-96 overflow-y-auto">
                        {notifications.length > 0 ? notifications.map(notif => (
                            <Menu.Item key={notif.id}>
                                {({ active }) => (
                                    <a href="#" className={`block px-4 py-2 text-sm ${active ? 'bg-gray-100' : ''} ${!notif.leido ? 'font-bold' : ''}`}>
                                        <p className="text-gray-800">{notif.mensaje}</p>
                                        <p className="text-xs text-gray-500 mt-1">{new Date(notif.fecha).toLocaleString()}</p>
                                    </a>
                                )}
                            </Menu.Item>
                        )) : (
                            <p className="text-center text-sm text-gray-500 py-4">No hay notificaciones.</p>
                        )}
                    </div>
                </Menu.Items>
            </Transition>
        </Menu>
    );
};

export default NotificationBell;
