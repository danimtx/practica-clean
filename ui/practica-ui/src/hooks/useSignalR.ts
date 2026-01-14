import { useEffect } from 'react';
import * as signalR from '@microsoft/signalr';
import { useAuthStore } from '../store/auth.store';
import { useNotificationStore } from '../store/notification.store';
import { toast } from 'sonner';

const HUB_URL = 'http://localhost:5012/notificationHub';

let connection: signalR.HubConnection | null = null;

export const useSignalR = () => {
  const { isAuthenticated, accessToken } = useAuthStore();
  const { addNotification } = useNotificationStore();

  useEffect(() => {
    if (isAuthenticated && accessToken && !connection) {
      // Crear y configurar la conexión
      connection = new signalR.HubConnectionBuilder()
        .withUrl(HUB_URL, {
          accessTokenFactory: () => accessToken,
        })
        .withAutomaticReconnect()
        .build();

      // Iniciar la conexión
      connection.start()
        .then(() => {
          console.log('Conectado al Hub de Notificaciones.');
        })
        .catch(err => console.error('Error de conexión con SignalR: ', err));

      // Escuchar el evento 'RecibirNotificacion'
      connection.on('RecibirNotificacion', (notificacion) => {
        console.log('Nueva notificación recibida:', notificacion);
        addNotification(notificacion);
        toast.info(notificacion.mensaje, {
            action: {
                label: 'Ver',
                onClick: () => console.log('Ir a la notificación'), // TODO: Implementar navegación
            },
        });
      });
      
    } else if (!isAuthenticated && connection) {
      // Detener la conexión si el usuario cierra sesión
      connection.stop().then(() => {
        console.log('Desconectado del Hub de Notificaciones.');
        connection = null;
      });
    }

    // Limpieza al desmontar el componente
    return () => {
      if (connection && connection.state === signalR.HubConnectionState.Connected) {
        // No detenemos la conexión aquí para que persista entre páginas
        // Se detendrá solo al hacer logout.
      }
    };
  }, [isAuthenticated, accessToken, addNotification]);
};
