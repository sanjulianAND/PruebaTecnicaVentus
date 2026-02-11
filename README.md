# 📚 Biblioteca API - Prueba Técnica .NET

API RESTful para gestión de biblioteca desarrollada con .NET 8, implementando arquitectura limpia, CQRS, autenticación JWT y buenas prácticas de desarrollo.

## 🌿 Estructura de Ramas

Este proyecto cuenta con **dos versiones** disponibles en diferentes ramas:

### `main` (Versión Completa - Actual)
Contiene todas las funcionalidades solicitadas más características adicionales que demuestran buenas prácticas de desarrollo:
- ✅ Autenticación JWT completa
- ✅ Soft delete (eliminación lógica)
- ✅ Paginación avanzada
- ✅ Logging con Serilog
- ✅ Rate limiting
- ✅ Health checks
- ✅ Middleware global de excepciones

### `feature/prueba-base` (Versión Básica)
Contiene únicamente lo solicitado en la prueba técnica:
- ✅ CRUD de Autores
- ✅ CRUD de Libros
- ✅ Reglas de negocio (máximo libros, autor debe existir)
- ✅ Validaciones básicas
- ✅ Sin autenticación

**Para revisar la versión básica:**
```bash
git checkout feature/prueba-base
```

## 🏗️ Arquitectura

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

## 🚀 Características

### Funcionalidades Base (Requeridas)
- 📖 Gestión completa de Autores (Nombre, Fecha Nacimiento, Ciudad, Email)
- 📚 Gestión completa de Libros (Título, Año, Género, Páginas, Autor)
- ✅ Validaciones de datos obligatorios
- ✅ Control de límite máximo de libros (100 por defecto)
- ✅ Verificación de existencia del autor
- ✅ Mensajes de error específicos

### Funcionalidades Adicionales (Versión Main)
- 🔐 **Autenticación JWT** - Login y registro de usuarios
- 🔒 **Autorización por Roles** - Administrador y Usuario
- 🗑️ **Soft Delete** - Eliminación lógica sin perder datos
- 📄 **Paginación** - Con metadatos completos
- 📝 **Logging** - Serilog con logs en consola y archivos
- 🛡️ **Rate Limiting** - Protección contra abuso (100 req/min)
- 💓 **Health Checks** - Endpoint /health para monitoreo
- 🎯 **Middleware de Excepciones** - Manejo centralizado de errores

## 🛠️ Tecnologías

- **.NET 8**
- **Entity Framework Core 8**
- **SQL Server**
- **MediatR** (CQRS)
- **FluentValidation**
- **BCrypt.Net** (hash de contraseñas)
- **JWT Bearer Authentication**
- **Serilog**
- **Swagger/OpenAPI**

## 📋 Prerrequisitos

- .NET 8 SDK
- SQL Server (o SQL Server Express)
- Visual Studio 2022 / VS Code

## ⚙️ Configuración

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

## 🚀 Ejecución

### Desarrollo
```bash
dotnet build
dotnet run --project PruebaPracticaVentus
```

### Producción
```bash
dotnet publish -c Release
```

## 👤 Usuarios de Prueba

La base de datos incluye dos usuarios pre-configurados:

| Rol | Email | Contraseña |
|-----|-------|------------|
| Administrador | admin@biblioteca.com | Admin123! |
| Usuario | usuario@biblioteca.com | User123! |

### ⚠️ Nota importante sobre usuarios

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

## 📚 API Endpoints

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

## 📄 Ejemplos de Uso

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

## 🔒 Autorización

La API usa **JWT Bearer tokens**. Incluir el token en el header:
```
Authorization: Bearer <tu-token-jwt>
```

### Roles
- **Administrador**: Acceso total (CRUD completo)
- **Usuario**: Puede crear/actualizar, pero no eliminar

## 📊 Logs

Los logs se almacenan en:
- **Consola**: Output estándar
- **Archivo**: `logs/biblioteca-.txt` (rotación diaria)

Formato:
```
[2024-01-15 10:30:45.123 +00:00 DBG] Solicitud GET /api/autores iniciada
[2024-01-15 10:30:45.234 +00:00 ERR] Error en GET /api/libros. Tipo: SqlException. Mensaje: Invalid object name...
```

## 🧪 Pruebas

### Swagger UI
Navegar a: `https://localhost:5001/swagger`

Incluye botón **"Authorize"** para probar endpoints protegidos.

## 📁 Estructura del Proyecto

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

## 📝 Decisiones de Diseño

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

## 🐛 Manejo de Errores

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

## 📈 Rendimiento

- **AsNoTracking** en consultas de solo lectura
- **Índices** en campos frecuentemente consultados
- **Rate Limiting** - 100 requests/minuto
- **Entity Framework** optimizado

## 🔐 Seguridad

- Contraseñas hasheadas con BCrypt
- JWT con firma HMAC-SHA256
- Validación de issuer y audience
- CORS configurado
- Rate limiting para prevenir DoS

## 📞 Contacto

Para dudas o sugerencias sobre el código, revisar los comentarios en el código fuente o la documentación de cada componente.

---

**Nota**: Este proyecto fue desarrollado como prueba técnica demostrando conocimientos en .NET, arquitectura limpia y buenas prácticas de desarrollo.

⭐ **Versión Main**: Incluye todas las funcionalidades adicionales
📦 **Versión Base**: Solo lo requerido en la prueba (rama `feature/prueba-base`)
