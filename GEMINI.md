

# Implementación del Patrón Observer y Sistema de Notificaciones

Este documento guía la implementación de un sistema de notificaciones en tiempo real para **Cybercorp**, utilizando el patrón de diseño **Observer** para mantener el sistema desacoplado y eficiente.

## 1. ¿Por qué y Cómo usamos el Patrón Observer?

* **El Problema**: No queremos que la lógica de "Crear Inspección" tenga que saber cómo enviar notificaciones, guardar registros de alerta o manejar WebSockets.
* **La Solución (Observer)**:
* **Sujeto**: El caso de uso `CrearInspeccion`. Cuando termina su trabajo, simplemente dispara un **Evento de Dominio** (`InspeccionAsignadaEvent`).
* **Observadores**: Son "Manejadores" (Handlers) independientes que están "escuchando" ese evento. Un observador guarda la notificación en la base de datos y otro la envía por **SignalR** al técnico.
* **Beneficio**: Podemos añadir nuevos observadores (ej: enviar un email o un SMS) en el futuro sin tocar una sola línea de código del creador de inspecciones.



---

## 2. Instrucciones para el Backend

### A. MediatR y Eventos de Dominio

1. **Instalación**: Añadir la librería `MediatR` en los proyectos de **Aplicación** e **Infraestructura**.
2. **Evento**: Crear `InspeccionAsignadaEvent` con: `IdInspeccion`, `UsuarioIdTecnico`, `NombreAdmin`, `TituloInspeccion` y `FechaProgramada`.
3. **Disparador**: En `CrearInspeccion.cs`, tras confirmar el guardado en la DB, usar `_mediator.Publish(new InspeccionAsignadaEvent(...))`.

### B. Persistencia (Entidad Notificacion)

1. **Dominio**: Crear la entidad `Notificacion`:
* `Id` (Guid)
* `UsuarioId` (Guid) -> Para quién es la notificación.
* `Mensaje` (string) -> El texto: *"El usuario [NombreAdmin] te asignó la inspección '[Titulo]' para la [Fecha]"*.
* `Leido` (bool) -> Por defecto `false`.
* `Fecha` (DateTime) -> Fecha de creación de la alerta.


2. **Repositorio**: Implementar `MarcarComoLeida(Guid id)` y `ObtenerNoLeidas(Guid usuarioId)`.

### C. SignalR (Tiempo Real)

1. **Configuración**: En `Program.cs`, habilitar `builder.Services.AddSignalR()` y mapear el endpoint `/notificationHub`.
2. **Seguridad**: El Hub debe extraer el `UserId` del **Token JWT** para que cada técnico reciba solo sus propias alertas.
3. **Envío**: El manejador del evento debe usar `IHubContext` para enviar el mensaje `RecibirNotificacion` al usuario específico.

---

## 3. Instrucciones para el Frontend (UX de la Campanita)

El agente debe preparar los endpoints para que el frontend pueda realizar el siguiente flujo:

1. **El Icono**: Un componente de campana que muestra un número en rojo (el contador de notificaciones donde `Leido == false`).
2. **La Interacción**:
* Al hacer clic, se despliega una pestaña/menú con la lista de notificaciones (historial).
* Cada notificación debe mostrar el mensaje formateado: *"El usuario [NombreAdmin] te asignó la inspección '[Título]' para la [Fecha]"*.


3. **El Cierre**:
* Al ver las nuevas notificaciones y cerrar la pestaña, el frontend debe llamar a un endpoint (ej: `PATCH /api/Notificaciones/marcar-leidas`).
* Esto cambia el estado en la DB y el numerito rojo desaparece, pero el historial sigue siendo visible cuando se vuelva a abrir la pestaña.



---

### Tareas Críticas de Infraestructura:

* **Migración**: Añadir la tabla `Notificaciones` a SQL Server y relacionarla con la tabla `Usuarios`.
* **Service Registration**: Registrar MediatR y SignalR en el contenedor de dependencias del `Program.cs` de la WebApi.