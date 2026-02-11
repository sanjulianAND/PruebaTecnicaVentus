import { Routes } from '@angular/router';
import { AuthGuard } from './core/guards/auth.guard';

export const routes: Routes = [
  {
    path: 'login',
    loadComponent: () => import('./features/auth/login/login.component').then(m => m.LoginComponent)
  },
  {
    path: 'register',
    loadComponent: () => import('./features/auth/register/register.component').then(m => m.RegisterComponent)
  },
  {
    path: '',
    loadComponent: () => import('./shared/components/layout/layout.component').then(m => m.LayoutComponent),
    canActivate: [AuthGuard],
    children: [
      {
        path: '',
        redirectTo: 'libros',
        pathMatch: 'full'
      },
      {
        path: 'libros',
        loadComponent: () => import('./features/libros/lista-libros/lista-libros.component').then(m => m.ListaLibrosComponent)
      },
      {
        path: 'libros/nuevo',
        loadComponent: () => import('./features/libros/formulario-libro/formulario-libro.component').then(m => m.FormularioLibroComponent)
      },
      {
        path: 'libros/editar/:id',
        loadComponent: () => import('./features/libros/formulario-libro/formulario-libro.component').then(m => m.FormularioLibroComponent)
      },
      {
        path: 'autores',
        loadComponent: () => import('./features/autores/lista-autores/lista-autores.component').then(m => m.ListaAutoresComponent)
      },
      {
        path: 'autores/nuevo',
        loadComponent: () => import('./features/autores/formulario-autor/formulario-autor.component').then(m => m.FormularioAutorComponent)
      },
      {
        path: 'autores/editar/:id',
        loadComponent: () => import('./features/autores/formulario-autor/formulario-autor.component').then(m => m.FormularioAutorComponent)
      }
    ]
  },
  {
    path: '**',
    redirectTo: 'login'
  }
];
