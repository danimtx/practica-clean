# Práctica de Clean Architecture en .NET

Este repositorio está dedicado a la práctica y aplicación de los principios de **Clean Architecture** (Arquitectura Limpia) utilizando el ecosistema de **.NET**.

El proyecto toma como caso de uso un **Sistema de Inspecciones**, organizando la solución en capas bien definidas para garantizar el desacoplamiento, la mantenibilidad y la testeabilidad del código.

---

## 🏛️ Estructura del Proyecto

La solución está estructurada siguiendo la separación de responsabilidades recomendada por Clean Architecture:

* **`Domain` (Dominio)**:
  Contiene las entidades de negocio, objetos de valor, excepciones de dominio e interfaces base. No tiene dependencias de librerías ni de otras capas del sistema.

* **`Aplication` (Aplicación)**:
  Implementa los casos de uso del sistema, validaciones, mapeos, DTOs y contratos/interfaces para servicios e infraestructura. Solo depende de la capa de Dominio.

* **`Infraestructure` (Infraestructura)**:
  Contiene las implementaciones técnicas de los contratos definidos en la capa de Aplicación: persistencia de datos (Entity Framework Core / SQL Server), repositorios, servicios externos, tokens JWT y sistema de archivos.

* **`WebApi` (Presentación / API)**:
  Punto de entrada de la aplicación HTTP (ASP.NET Core Web API). Configura la inyección de dependencias, controladores o endpoints mínimos, middlewares y autenticación/autorización.

* **`ui` (Frontend / Interfaz)**:
  Componentes de interfaz de usuario para interactuar con la Web API.

---

## 🚀 Tecnologías y Herramientas

* **Plataforma**: .NET / C#
* **API**: ASP.NET Core Web API
* **Autenticación**: JWT (JSON Web Tokens)
* **Persistencia**: SQL Server / Entity Framework Core
* **Patrones**: Clean Architecture, Repository Pattern, CQRS / Use Cases

---

## 🛠️ Requisitos e Inicio Rápido

1. Clonar el repositorio:
   ```bash
   git clone https://github.com/danimtx/practica-clean.git
   ```
2. Restaurar dependencias:
   ```bash
   dotnet restore
   ```
3. Ejecutar la Web API:
   ```bash
   dotnet run --project WebApi
   ```
