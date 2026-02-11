# Biblioteca Frontend

Aplicación Angular 17+ para gestión de biblioteca con autenticación JWT, interfaz moderna con Angular Material y arquitectura escalable.

## 🚀 Características

- **Angular 17+** con Standalone Components
- **Angular Material** para UI moderna y responsive
- **JWT Authentication** con login/registro
- **Reactive Forms** con validaciones
- **HTTP Interceptors** para manejo de tokens y errores
- **Route Guards** para protección de rutas
- **Arquitectura por Features** escalable

## 📁 Estructura del Proyecto

```
src/app/
├── core/                    # Servicios, modelos, guards, interceptors
│   ├── models/             # Interfaces TypeScript
│   ├── services/           # Servicios HTTP
│   ├── guards/             # Route guards
│   └── interceptors/       # HTTP interceptors
├── features/               # Módulos de funcionalidades
│   ├── auth/              # Login y registro
│   ├── autores/           # CRUD de autores
│   └── libros/            # CRUD de libros
├── shared/                # Componentes compartidos
│   └── components/
│       └── layout/        # Layout principal con navegación
├── app.component.ts       # Componente raíz
├── app.config.ts          # Configuración de la app
└── app.routes.ts          # Definición de rutas
```

## 🛠️ Instalación

### Prerrequisitos
- Node.js 18+
- Angular CLI 17+

### 1. Instalar dependencias
```bash
cd frontend
npm install
```

### 2. Configurar API URL
Editar `src/environments/environment.ts`:
```typescript
export const environment = {
  production: false,
  apiUrl: 'https://localhost:5001/api'  // URL del backend
};
```

### 3. Ejecutar aplicación
```bash
ng serve
```

La aplicación estará disponible en: `http://localhost:4200`

## 🔐 Funcionalidades

### Autenticación
- Login con correo y contraseña
- Registro de nuevos usuarios
- Almacenamiento de JWT en localStorage
- Protección de rutas privadas
- Cierre de sesión

### Gestión de Libros
- Listado con tabla responsive
- Crear nuevo libro (con selección de autor)
- Editar libro existente
- Eliminar libro
- Validaciones de campos obligatorios

### Gestión de Autores
- Listado completo
- Crear nuevo autor
- Editar autor existente
- Eliminar autor
- Datepicker para fecha de nacimiento

## 🎨 Componentes UI

### Angular Material Components utilizados:
- **MatToolbar** - Barra de navegación
- **MatSidenav** - Menú lateral
- **MatTable** - Tablas de datos
- **MatFormField** - Campos de formulario
- **MatInput** - Inputs de texto
- **MatSelect** - Selectores
- **MatDatepicker** - Selector de fechas
- **MatButton** - Botones
- **MatIcon** - Iconos
- **MatCard** - Tarjetas
- **MatSnackBar** - Notificaciones
- **MatProgressSpinner** - Indicadores de carga
- **MatDialog** - Diálogos modales

## 🔗 Comunicación con Backend

### Servicios HTTP
Los servicios se encuentran en `src/app/core/services/`:

- **AuthService** - Autenticación (login, register, logout)
- **LibroService** - CRUD de libros
- **AutorService** - CRUD de autores

### Interceptors
- **JwtInterceptor** - Añade token Bearer a requests
- **ErrorInterceptor** - Manejo global de errores HTTP

### Guards
- **AuthGuard** - Protege rutas que requieren autenticación

## 📱 Responsive Design

La aplicación es completamente responsive:
- Sidebar colapsable en móviles
- Tablas con scroll horizontal
- Formularios adaptativos
- Botones táctiles

## 🚀 Build para Producción

```bash
ng build --configuration production
```

Los archivos se generarán en: `dist/biblioteca-frontend/`

## 📝 Notas de Desarrollo

### Convenciones
- Componentes standalone (sin NgModules)
- Reactive Forms para todos los formularios
- Observables con async pipe en templates
- Validaciones sincrónicas y asíncronas
- Manejo de errores con MatSnackBar

### Buenas Prácticas Implementadas
- Lazy loading de componentes
- Inyección de dependencias
- Separación de responsabilidades
- Tipado estricto de TypeScript
- Unsubscribe automático con async pipe

## 🔧 Solución de Problemas

### Error de CORS
Si el backend rechaza las peticiones por CORS, asegúrate de que esté configurado:
```csharp
// En el backend Program.cs
app.UseCors("AllowAll");
```

### Problemas con certificados SSL
```bash
# Generar certificado local para desarrollo
dotnet dev-certs https --trust
```

## 📦 Dependencias Principales

```json
{
  "@angular/core": "^17.0.0",
  "@angular/material": "^17.0.0",
  "@angular/forms": "^17.0.0",
  "rxjs": "~7.8.0"
}
```

## 📄 Licencia

Este proyecto fue desarrollado como prueba técnica.
