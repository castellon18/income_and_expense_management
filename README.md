# Manejo de Ingresos y Gastos (Income and Expense Management).

Aplicación web desarrollada con ASP.NET Core MVC 9 para el control y gestión de finanzas personales, construida como parte del curso práctico de Felipe Gavilán en Udemy.

## Tecnologías Utilizadas

* **ASP.NET Core MVC 9** (Framework principal)
* **Dapper** (Micro-ORM para acceso a datos de alto rendimiento)
* **Microsoft.Data.SqlClient** (Conexión con SQL Server)
* **ClosedXML** (Generación y exportación de archivos Excel)
* **ASP.NET Core Identity** (Sistema de autenticación y gestión de usuarios)
* **SweetAlert2** (Alertas modales modernas y personalizables)
* **FullCalendar** (Visualización e interacción de eventos en formato de calendario)
* **Bootstrap** (Diseño y maquetación responsiva)

## Aprendisaje

* Estructuración limpia de proyectos bajo el patrón MVC.
* Implementación de validaciones mediante **DataAnnotations** nativos y creación de atributos personalizados para los ViewModels.
* Ejecución y manejo eficiente de consultas y procedimientos almacenados utilizando **Dapper**.
* Configuración de seguridad, autenticación y autorización con **ASP.NET Core Identity**.
* Integración de **SweetAlert2** para mejorar la experiencia de usuario con notificaciones y cuadros de diálogo interactivos.
* Uso de **FullCalendar** para la representación visual y gestión de transacciones u operaciones organizadas por fechas.
* Aplicación de **Inyección de Dependencias** para desacoplar servicios y lógica de negocio.

## Configuración y Ejecución en Modo Desarrollo

Sigue estos pasos para poner en marcha el proyecto localmente:

1. **Base de Datos:** Ejecuta los scripts SQL ubicados en la carpeta `db-income-and-expense-management` en tu servidor local de SQL Server. Esto creará la estructura de la base de datos y los procedimientos almacenados necesarios.
2. **Cadena de Conexión:** Abre el archivo `appsettings.Development.json` y actualiza el valor de `defaultConnection` con las credenciales de tu servidor:
```json
"ConnectionStrings": {
  "defaultConnection": "Server=TU_SERVIDOR;Database=NombreDeTuBaseDeDatos;Trusted_Connection=True;TrustServerCertificate=True;"
}
```
4. **Ejecución:** Inicia el servidor de desarrollo en la raíz del proyecto ejecutando el siguiente comando:
```bash
dotnet watch
```
5. **Acceso:** Abre tu navegador web e ingresa a http://localhost:5191/. El servidor cuenta con recarga en caliente (hot-reload), por lo que la aplicación se actualizará automáticamente al guardar cambios en el código fuente.