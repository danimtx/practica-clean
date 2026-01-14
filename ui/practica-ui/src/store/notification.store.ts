import { create } from 'zustand';
import type { Notificacion } from '../services/notification.service';

interface NotificationState {
    notifications: Notificacion[];
    unreadCount: number;
    setNotifications: (notifications: Notificacion[]) => void;
    addNotification: (notification: Notificacion) => void;
    markAllAsRead: () => void;
}

export const useNotificationStore = create<NotificationState>((set) => ({
    notifications: [],
    unreadCount: 0,
    setNotifications: (notifications) => {
        set({
            notifications,
            unreadCount: notifications.filter(n => !n.leido).length,
        });
    },
    addNotification: (notification) => {
        set(state => ({
            notifications: [notification, ...state.notifications],
            unreadCount: state.unreadCount + 1,
        }));
    },
    markAllAsRead: () => {
        set(state => ({
            notifications: state.notifications.map(n => ({ ...n, leido: true })),
            unreadCount: 0,
        }));
    },
}));
