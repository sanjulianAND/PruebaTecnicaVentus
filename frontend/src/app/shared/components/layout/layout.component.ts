import { Component } from '@angular/core';
import { RouterOutlet, RouterLink } from '@angular/router';
import { CommonModule } from '@angular/common';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatListModule } from '@angular/material/list';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { AuthService } from '../../../core/services/auth.service';

@Component({
  selector: 'app-layout',
  standalone: true,
  imports: [
    CommonModule,
    RouterOutlet,
    RouterLink,
    MatToolbarModule,
    MatSidenavModule,
    MatListModule,
    MatIconModule,
    MatButtonModule
  ],
  template: `
    <mat-toolbar color="primary">
      <button mat-icon-button (click)="sidenav.toggle()">
        <mat-icon>menu</mat-icon>
      </button>
      <span>Biblioteca</span>
      <span class="spacer"></span>
      <span *ngIf="usuario$ | async as usuario" class="user-name">
        {{ usuario.nombreUsuario }} ({{ usuario.rol }})
      </span>
      <button mat-icon-button (click)="logout()" matTooltip="Cerrar sesión">
        <mat-icon>logout</mat-icon>
      </button>
    </mat-toolbar>

    <mat-sidenav-container class="sidenav-container">
      <mat-sidenav #sidenav mode="side" opened class="sidenav">
        <mat-nav-list>
          <a mat-list-item routerLink="/libros" routerLinkActive="active">
            <mat-icon matListItemIcon>book</mat-icon>
            <span matListItemTitle>Libros</span>
          </a>
          <a mat-list-item routerLink="/autores" routerLinkActive="active">
            <mat-icon matListItemIcon>person</mat-icon>
            <span matListItemTitle>Autores</span>
          </a>
        </mat-nav-list>
      </mat-sidenav>

      <mat-sidenav-content class="content">
        <div class="container">
          <router-outlet></router-outlet>
        </div>
      </mat-sidenav-content>
    </mat-sidenav-container>
  `,
  styles: [`
    .spacer {
      flex: 1 1 auto;
    }
    .sidenav-container {
      height: calc(100vh - 64px);
    }
    .sidenav {
      width: 250px;
    }
    .content {
      background-color: #f5f5f5;
      min-height: 100%;
    }
    .user-name {
      font-size: 14px;
      margin-right: 16px;
    }
    .active {
      background-color: rgba(0, 0, 0, 0.04);
    }
  `]
})
export class LayoutComponent {
  usuario$ = this.authService.usuario$;

  constructor(private authService: AuthService) {}

  logout(): void {
    this.authService.logout();
  }
}
