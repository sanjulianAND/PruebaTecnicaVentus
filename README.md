# Biblioteca API - Prueba Técnica .NET

API RESTful para gestión de biblioteca desarrollada con .NET 8 y Angular 17+, implementando arquitectura limpia, CQRS, autenticación JWT y buenas prácticas de desarrollo.

## Estructura del Proyecto

```
PruebaPracticaVentus/
├── Backend (.NET)
│   ├── Database/           # Scripts SQL
│   ├── Domain/            # Entidades y excepciones
│   ├── Application/       # Casos de uso (CQRS)
│   ├── Infrastructure/    # Repositorios y servicios
│   └── PruebaPracticaVentus/  # API Controllers
│
└── Frontend (Angular)
    └── src/app/
        ├── core/          # Servicios, guards, interceptors
        ├── features/      # Módulos de funcionalidades
        └── shared/        # Componentes compartidos
```

## Inicio Rápido

### 1. Base de Datos

Ejecutar `Database/Script_BD_Completo.sql` en SQL Server

### 2. Backend

```bash
dotnet run --project PruebaPracticaVentus
```

Backend disponible en: `https://localhost:5001`

### 3. Frontend

```bash
cd frontend
npm install
ng serve
```

Frontend disponible en: `http://localhost:4200`

---

## Backend (.NET 8)

### Arquitectura

El proyecto sigue una **Arquitectura Limpia (Clean Architecture)** con los siguientes proyectos:

```
├── Domain          # Entidades, excepciones, contratos
├── Application     # Casos de uso, DTOs, validaciones (CQRS)
├── Infrastructure  # Repositorios, DbContext, servicios externos
└── API             # Controllers, middleware, configuración
```

### Patrones Implementados

- **CQRS** con MediatR
- **Repository Pattern**
- **Dependency Injection**
- **DTOs** para transferencia de datos
- **FluentValidation** para validaciones
- **Unit of Work** implícito con EF Core

## Características

### Funcionalidades Base (Requeridas)

- Gestión completa de Autores (Nombre, Fecha Nacimiento, Ciudad, Email)
- Gestión completa de Libros (Título, Año, Género, Páginas, Autor)
- Validaciones de datos obligatorios
- Control de límite máximo de libros (100 por defecto)
- Verificación de existencia del autor
- Mensajes de error específicos

### Funcionalidades Adicionales (Versión Main)

- **Autenticación JWT** - Login y registro de usuarios
- **Autorización por Roles** - Administrador y Usuario
- **Soft Delete** - Eliminación lógica sin perder datos
- **Paginación** - Con metadatos completos
- **Logging** - Serilog con logs en consola y archivos
- **Rate Limiting** - Protección contra abuso (100 req/min)
- **Health Checks** - Endpoint /health para monitoreo
- **Middleware de Excepciones** - Manejo centralizado de errores

## Tecnologías

- **.NET 8**
- **Entity Framework Core 8**
- **SQL Server**
- **MediatR** (CQRS)
- **FluentValidation**
- **BCrypt.Net** (hash de contraseñas)
- **JWT Bearer Authentication**
- **Serilog**
- **Swagger/OpenAPI**

## Prerrequisitos

- .NET 8 SDK
- SQL Server (o SQL Server Express)
- Visual Studio 2022 / VS Code

## Configuración

### 1. Base de Datos

Ejecutar el script SQL ubicado en:

```
Database/Script_BD_Completo.sql
```

Este script creará:

- Base de datos `BibliotecaDB`
- Tablas: Autores, Libros, **Usuarios**
- Datos de prueba (autores y libros)
- Usuarios de prueba pre-configurados

### 2. Connection String

El connection string está configurado en `appsettings.json`:

```json
"ConnectionStrings": {
  "PruebaSD": "Server=DESKTOP-D867T7P\\SQLEXPRESS;Database=BibliotecaDB;Trusted_Connection=true;TrustServerCertificate=true;"
}
```

**Modificar según tu servidor SQL.**

### 3. JWT Configuration

```json
"Jwt": {
  "Key": "TuClaveSecretaSuperSeguraParaBiblioteca2024!",
  "Issuer": "BibliotecaAPI",
  "Audience": "BibliotecaClient"
}
```

## Ejecución

### Desarrollo

```bash
dotnet build
dotnet run --project PruebaPracticaVentus
```

### Producción

```bash
dotnet publish -c Release
```

## Usuarios de Prueba

La base de datos incluye dos usuarios pre-configurados:

| Rol           | Email                  | Contraseña |
| ------------- | ---------------------- | ---------- |
| Administrador | admin@biblioteca.com   | Admin123!  |
| Usuario       | usuario@biblioteca.com | User123!   |

### Nota importante sobre usuarios

**Si el login falla** con los usuarios de prueba (puede ocurrir por hash de contraseña incompatible), puedes **registrar un nuevo usuario** usando el endpoint:

```http
POST /api/auth/register
Content-Type: application/json

{
  "nombreUsuario": "tuusuario",
  "correoElectronico": "tu@email.com",
  "password": "TuPassword123!"
}
```

El registro es **público** y creará un usuario con rol "Usuario".

## API Endpoints

### Autenticación

- `POST /api/auth/login` - Iniciar sesión (público)
- `POST /api/auth/register` - Registrar usuario (público)

### Autores

- `GET /api/autores` - Listar todos (público)
- `GET /api/autores/paginado` - Listar paginado (público)
- `POST /api/autores` - Crear autor (requiere auth)
- `PUT /api/autores/{id}` - Actualizar autor (requiere auth)
- `DELETE /api/autores/{id}` - Eliminar autor (solo admin)

### Libros

- `GET /api/libros` - Listar todos (público)
- `POST /api/libros` - Crear libro (requiere auth)
- `PUT /api/libros/{id}` - Actualizar libro (requiere auth)
- `DELETE /api/libros/{id}` - Eliminar libro (solo admin)

### Monitoreo

- `GET /health` - Health check (público)

## Ejemplos de Uso

### Login

```bash
curl -X POST https://localhost:5001/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{
    "correoElectronico": "admin@biblioteca.com",
    "password": "Admin123!"
  }'
```

**Respuesta:**

```json
{
  "exito": true,
  "mensaje": "Inicio de sesión exitoso",
  "datos": {
    "token": "eyJhbGciOiJIUzI1NiIs...",
    "refreshToken": "abc123...",
    "expiracion": "2024-01-15T14:30:00",
    "usuario": {
      "id": 1,
      "nombreUsuario": "admin",
      "correoElectronico": "admin@biblioteca.com",
      "rol": "Administrador"
    }
  }
}
```

### Crear Libro (Autenticado)

```bash
curl -X POST https://localhost:5001/api/libros \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer <token>" \
  -d '{
    "titulo": "El Principito",
    "anio": 1943,
    "genero": "Fábula",
    "numeroPaginas": 96,
    "autorId": 1
  }'
```

### Paginación

```bash
curl "https://localhost:5001/api/autores/paginado?pagina=1&tamañoPagina=10&ordenarPor=NombreCompleto&ordenDescendente=false"
```

## Autorización

La API usa **JWT Bearer tokens**. Incluir el token en el header:

```
Authorization: Bearer <tu-token-jwt>
```

### Roles

- **Administrador**: Acceso total (CRUD completo)
- **Usuario**: Puede crear/actualizar, pero no eliminar

## Logs

Los logs se almacenan en:

- **Consola**: Output estándar
- **Archivo**: `logs/biblioteca-.txt` (rotación diaria)

Formato:

```
[2024-01-15 10:30:45.123 +00:00 DBG] Solicitud GET /api/autores iniciada
[2024-01-15 10:30:45.234 +00:00 ERR] Error en GET /api/libros. Tipo: SqlException. Mensaje: Invalid object name...
```

## Pruebas

### Swagger UI

Navegar a: `https://localhost:5001/swagger`

Incluye botón **"Authorize"** para probar endpoints protegidos.

## Estructura del Proyecto

```
PruebaPracticaVentus/
├── Database/
│   └── Script_BD_Completo.sql    # Script de base de datos
├── Domain/
│   ├── Entities/                  # Autor, Libro, Usuario
│   └── Exceptions/                # Excepciones personalizadas
├── Infrastructure/
│   ├── Abstractions/              # Interfaces de repositorios
│   ├── Persistence/               # DbContext
│   ├── Repositories/              # Implementaciones
│   ├── Services/                  # JwtService
│   └── DependencyInjection/
├── Application/
│   ├── DTOs/                      # Data Transfer Objects
│   ├── Features/                  # Casos de uso (CQRS)
│   │   ├── Autores/
│   │   ├── Libros/
│   │   └── Auth/
│   └── DependencyInjection/
└── PruebaPracticaVentus/
    ├── Controllers/
    ├── Middleware/
    ├── logs/                      # Archivos de log
    └── appsettings.json
```

## Decisiones de Diseño

### Soft Delete vs Delete Físico

Se implementó **Soft Delete** para mantener integridad de datos históricos. Los registros eliminados se marcan con `Activo = false`.

### Paginación

Implementada en `/api/autores/paginado` con:

- Parámetros queryables
- Ordenamiento dinámico
- Metadatos completos

### Validaciones

Usamos **FluentValidation** para:

- Separar lógica de validación
- Mensajes de error personalizados
- Reutilización de validadores

### Autenticación JWT

- Tokens de 2 horas de duración
- Refresh tokens para renovación
- Claims con información del usuario
- BCrypt para hash de contraseñas

## Manejo de Errores

El middleware global captura y formatea todas las excepciones:

```json
{
  "exito": false,
  "mensaje": "El autor no está registrado",
  "datos": null,
  "errores": null
}
```

Tipos de errores manejados:

- `SqlException` - Errores de base de datos
- `AutorNoEncontradoException` - Autor no existe
- `MaximoLibrosException` - Límite alcanzado
- `ValidacionException` - Errores de validación

## Rendimiento

- **AsNoTracking** en consultas de solo lectura
- **Índices** en campos frecuentemente consultados
- **Rate Limiting** - 100 requests/minuto
- **Entity Framework** optimizado

## Seguridad

- Contraseñas hasheadas con BCrypt
- JWT con firma HMAC-SHA256
- Validación de issuer y audience
- CORS configurado
- Rate limiting para prevenir DoS

---

## Frontend (Angular 17+)

### Características

- **Angular 17+** con Standalone Components
- **Angular Material** para UI moderna
- **JWT Authentication** con guards e interceptors
- **Reactive Forms** con validaciones
- **Arquitectura por Features** escalable

### Estructura del Frontend

```
frontend/src/app/
├── core/                    # Infraestructura
│   ├── models/             # Interfaces TypeScript
│   ├── services/           # HTTP Services
│   ├── guards/             # Route guards
│   └── interceptors/       # HTTP interceptors
│
├── features/               # Módulos funcionales
│   ├── auth/              # Login/Register
│   ├── autores/           # CRUD Autores
│   └── libros/            # CRUD Libros
│
└── shared/                # Componentes compartidos
    └── components/
        └── layout/        # Layout con navegación
```

### Instalación Frontend

```bash
cd frontend
npm install
ng serve
```

### Configuración

Editar `frontend/src/environments/environment.ts`:

```typescript
export const environment = {
  production: false,
  apiUrl: "https://localhost:5001/api", // URL del backend
};
```

### Funcionalidades UI

#### Autenticación

- Pantalla de login con gradiente
- Pantalla de registro
- Protección de rutas
- Cierre de sesión

#### Gestión de Libros

- Tabla responsive con Material Table
- Formulario con validaciones
- Selector de autor
- Datepicker para fechas
- Confirmación para eliminar

#### Gestión de Autores

- Listado con todas las columnas
- Formulario con datepicker
- Validaciones de email

### Tecnologías Frontend

- Angular 17+ (Standalone Components)
- Angular Material (UI Components)
- RxJS (Observables)
- TypeScript (Tipado estricto)

### Componentes Angular Material

- MatToolbar, MatSidenav, MatList (Navegación)
- MatTable (Tablas de datos)
- MatFormField, MatInput, MatSelect (Formularios)
- MatDatepicker (Fechas)
- MatButton, MatIcon (Acciones)
- MatCard (Contenedores)
- MatSnackBar (Notificaciones)
- MatProgressSpinner (Loading)

---

## Notas Finales

### Para Desarrolladores

1. **Backend**: Arquitectura limpia con CQRS, fácil de extender
2. **Frontend**: Angular moderno con buenas prácticas, componentes standalone
3. **Base de Datos**: Script completo con datos de prueba

### Próximos Pasos Sugeridos

- [ ] Agregar tests unitarios (xUnit para backend, Jasmine para frontend)
- [ ] Implementar paginación en frontend
- [ ] Agregar filtros de búsqueda
- [ ] Implementar caché con Redis
- [ ] Agregar Docker para deployment

### Soporte

Para dudas técnicas revisar:

- Documentación del backend en Swagger: `/swagger`
- Logs del backend en: `logs/biblioteca-.txt`
- README específico del frontend en: `frontend/README.md`

---

Desarrollado como prueba técnica demostrando conocimientos en:

- **Backend**: .NET 8, Clean Architecture, CQRS, JWT, EF Core
- **Frontend**: Angular 17+, TypeScript, Angular Material, RxJS
- **Base de Datos**: SQL Server
