# Documento de Requerimientos de Producto (PRD): Sistema de Inspecciones Cybercorp

## 1. Visión del Producto

El Sistema de Inspecciones Cybercorp es una aplicación web interna diseñada para modernizar y centralizar la gestión del ciclo de vida de las inspecciones técnicas. El sistema reemplaza procesos manuales y descentralizados, proporcionando una plataforma única donde los administradores pueden asignar tareas, los técnicos pueden gestionar su trabajo y la información se mantiene de forma segura y consistente.

## 2. Arquitectura y Seguridad

*   **RNF-01: Arquitectura Limpia (Clean Architecture)**: El sistema sigue una arquitectura con capas de Dominio, Aplicación, Infraestructura y Presentación (WebApi), asegurando la separación de responsabilidades.
*   **RNF-02: Seguridad por JWT**: El acceso a la API está restringido mediante JSON Web Tokens (JWT). El `AccessToken` contiene los `Claims` del usuario, incluyendo su ID, cargo y permisos.
*   **RNF-03: Persistencia**:
    *   **Base de Datos**: La información transaccional se almacena en SQL Server.
    *   **Almacenamiento de Archivos**: Los archivos físicos (PDFs, fotos) se guardan en el sistema de archivos del servidor, dentro de la carpeta `wwwroot/uploads/`.
*   **RNF-04: Comunicación en Tiempo Real**: El sistema utiliza **SignalR** para notificaciones push en tiempo real desde el servidor hacia los clientes conectados.

## 3. Cargos y Permisos

El sistema se basa en una combinación de **Cargos** (roles organizativos) y **Permisos** (capacidades específicas) para un control de acceso granular.

### 3.1. Jerarquía de Cargos

*   **SuperAdmin**: El administrador raíz del sistema. Es el único cargo que no puede ser eliminado y es el único capaz de crear o eliminar otros cargos de tipo `Admin`. Existe un solo SuperAdmin.
*   **Admin**: Un rol con altos privilegios, creado por un SuperAdmin. Puede gestionar usuarios (activarlos, asignarles cargos y permisos) y tiene control sobre el módulo de inspecciones.
*   **Técnico**: El rol principal de campo. Realiza inspecciones y reporta sus hallazgos.
*   **Invitado**: El estado por defecto para un usuario recién registrado. No tiene permisos operativos y no puede iniciar sesión hasta ser activado por un Admin.

### 3.2. Lista de Permisos del Sistema

Las acciones críticas están protegidas por los siguientes permisos, que se asignan a los usuarios en una lista de strings (`List<string> Permisos`).

*   `inspeccion:crear`
*   `inspeccion:editar`
*   `inspeccion:estado`
*   `inspeccion:archivo:subir`
*   `inspeccion:archivo:borrar`
*   `usuario:gestionar`
*   `cargo:gestionar`

## 4. Requerimientos Funcionales y Endpoints

### 4.1. Módulo de Autenticación y Perfil

*   **RF-01: Registro de Usuario**: `POST /api/usuarios/registro`
    *   Crea un usuario como `Invitado`, `Inactivo`, con el permiso `["home"]` y una foto de perfil por defecto.
*   **RF-02: Inicio de Sesión**: `POST /api/usuarios/login`
    *   Un usuario `Activo` obtiene un `AccessToken` y un `RefreshToken`.
*   **RF-03: Renovación de Sesión**: `POST /api/usuarios/refresh`
    *   Permite obtener un nuevo par de tokens usando un `RefreshToken` válido.
*   **RF-04: Obtener Perfil de Usuario**: `GET /api/usuarios/perfil`
    *   Un usuario autenticado obtiene los datos de su propio perfil. `[Authorize]`
*   **RF-05: Editar Perfil de Usuario**: `PUT /api/usuarios/perfil`
    *   Un usuario autenticado actualiza su `Nombre` y/o `Password`. `[Authorize]`
*   **RF-06: Subir Foto de Perfil**: `POST /api/usuarios/foto-perfil`
    *   Un usuario autenticado sube o reemplaza su foto de perfil. `[Authorize]`
    *   **Reglas**: `.jpg, .jpeg, .png`, max 2MB. Ubicación: `wwwroot/uploads/profiles/`.

### 4.2. Módulo de Gestión de Usuarios (Admins)

*   **RF-07: Listar y Filtrar Usuarios**: `GET /api/usuarios`
    *   Lista todos los usuarios, con opción de filtrar por cargo. `[Authorize]`
*   **RF-08: Gestionar Usuario**: `PUT /api/usuarios/gestionar`
    *   Un `Admin` o `SuperAdmin` puede activar usuarios, cambiar `Cargo` y asignar `Permisos`. `[Authorize(Policy = "usuario:gestionar")]`
    *   **Reglas de Negocio**: Un usuario no puede modificarse a sí mismo. Un `Admin` no puede modificar a otro `Admin` o a un `SuperAdmin`.

### 4.3. Módulo de Gestión de Cargos (SuperAdmin)

*   **RF-09: Gestión de Cargos**:
    *   `GET /api/cargos`
    *   `POST /api/cargos`
    *   `DELETE /api/cargos/{id}`
    *   **Seguridad**: `[Authorize(Policy = "SuperAdminPolicy")]`. Para crear/eliminar, requiere además `[Authorize(Policy = "cargo:gestionar")]`.

### 4.4. Módulo de Gestión de Inspecciones

*   **RF-10: Crear Inspección**: `POST /api/inspecciones`
    *   **Seguridad**: `[Authorize(Policy = "inspeccion:crear")]`. Dispara un evento para notificar al técnico asignado.
*   **RF-11: Ver Inspecciones Propias**: `GET /api/inspecciones/mis-inspecciones`
    *   Un técnico ve sus inspecciones. El ID se extrae del token. `[Authorize(Roles = "Tecnico")]`.
*   **RF-12: Gestionar Archivo PDF**:
    *   `POST /api/inspecciones/{id}/archivo` - **Seguridad**: `[Authorize(Policy = "inspeccion:archivo:subir")]`
    *   `DELETE /api/inspecciones/{id}/archivo` - **Seguridad**: `[Authorize(Policy = "inspeccion:archivo:borrar")]`
    *   **Reglas**: `.pdf`, max 5MB. Ubicación: `wwwroot/uploads/inspecciones/`.
*   **RF-13: Descargar Archivo PDF**: `GET /api/inspecciones/{id}/descargar`
    *   **Seguridad**: `[Authorize]`
*   **RF-14: Actualizar Estado de Inspección**: `PATCH /api/inspecciones/{id}/estado`
    *   **Seguridad**: `[Authorize(Policy = "inspeccion:estado")]`

### 4.5. Módulo de Notificaciones

*   **RF-15: Obtener Notificaciones No Leídas**: `GET /api/notificaciones/no-leidas`
    *   Obtiene la lista de notificaciones pendientes para el usuario autenticado. `[Authorize]`
*   **RF-16: Marcar Notificaciones como Leídas**: `PATCH /api/notificaciones/marcar-leidas`
    *   Marca todas las notificaciones no leídas del usuario como leídas. `[Authorize]`
*   **RF-17: Recibir Notificaciones en Tiempo Real**: `HUB /notificationHub`
    *   Endpoint para la conexión persistente con **SignalR**.

---

## 5. Guía de Implementación para el Frontend

Esta sección describe cómo el frontend debe interactuar con el sistema de notificaciones.

### 5.1. Conexión a SignalR

1.  **Instalar Librería**: Añade la librería cliente de SignalR a tu proyecto de frontend.
    ```shell
    npm install @microsoft/signalr
    ```
2.  **Establecer Conexión**: La conexión debe configurarse para enviar el `AccessToken` (JWT) del usuario para la autenticación.

    ```javascript
    import * as signalR from "@microsoft/signalr";

    const connection = new signalR.HubConnectionBuilder()
        .withUrl("http://localhost:5012/notificationHub", {
            accessTokenFactory: () => your_access_token // Obtener el token del estado de la app
        })
        .withAutomaticReconnect()
        .build();
    ```
3.  **Iniciar Conexión**: Inicia la conexión después de que el usuario haya iniciado sesión.

    ```javascript
    connection.start().then(() => {
        console.log("Conectado al Hub de Notificaciones.");
    }).catch(err => console.error("Error de conexión con SignalR: ", err));
    ```

### 5.2. Flujo de la Interfaz (UI)

1.  **Escuchar Nuevas Notificaciones**: El cliente debe registrar un manejador para el evento `RecibirNotificacion` que el servidor envía.

    ```javascript
    connection.on("RecibirNotificacion", (notificacion) => {
        console.log("Nueva notificación recibida:", notificacion);
        // Lógica para añadir la notificación al estado de la UI
        // y mostrar un indicador visual (ej. un "toast").
    });
    ```
    El objeto `notificacion` tendrá la estructura de la entidad `Notificacion` del backend.

2.  **Icono de Campana y Contador**:
    *   Al cargar la aplicación, haz una llamada a `GET /api/notificaciones/no-leidas` para obtener el número inicial de notificaciones no leídas y muéstralo en un badge sobre el icono de la campana.
    *   Cuando llegue una nueva notificación a través de SignalR (`RecibirNotificacion`), incrementa este contador.

3.  **Desplegar y Marcar como Leídas**:
    *   Al hacer clic en la campana, muestra la lista de notificaciones (puedes usar las que ya tienes en el estado o volver a pedirlas).
    *   Cuando el usuario cierre el menú de notificaciones (haciendo clic fuera o en un botón de "cerrar"), envía una petición a `PATCH /api/notificaciones/marcar-leidas`.
    *   Una vez que la petición `PATCH` sea exitosa, resetea el contador de notificaciones no leídas a `0` en la UI.

---

## 6. Modelo de Datos Final

*   **Usuario**: `Id`, `Nombre`, `Email`, `PasswordHash`, `EstaActivo`, `FotoPerfil?`, `Permisos`, `RefreshToken?`, `RefreshTokenExpiryTime?`, `CargoId`, `Cargo`.
*   **Cargo**: `Id`, `Nombre`, `Usuarios`.
*   **Inspeccion**: `Id`, `NombreCliente`, `Direccion`, `FechaRegistro`, `DetallesTecnicos`, `Observaciones`, `RutaArchivoPdf?`, `Estado`, `UsuarioId?`, `Tecnico?`.
*   **Notificacion**:
    *   `Id` (Guid)
    *   `UsuarioId` (Guid) - _Foreign Key a `Usuario.Id`_
    *   `Usuario` (Usuario) - _Propiedad de navegación_
    *   `Mensaje` (string)
    *   `Leido` (bool)
    *   `Fecha` (DateTime)
