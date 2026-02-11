import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { MatTableModule } from '@angular/material/table';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Autor } from '../../../core/models/autor.model';
import { AutorService } from '../../../core/services/autor.service';

@Component({
  selector: 'app-lista-autores',
  standalone: true,
  imports: [
    CommonModule,
    RouterLink,
    MatTableModule,
    MatButtonModule,
    MatIconModule,
    MatProgressSpinnerModule
  ],
  template: `
    <div class="header">
      <h1>Gestión de Autores</h1>
      <button mat-raised-button color="primary" routerLink="/autores/nuevo">
        <mat-icon>add</mat-icon>
        Nuevo Autor
      </button>
    </div>

    <div *ngIf="loading" class="loading-container">
      <mat-spinner></mat-spinner>
    </div>

    <div *ngIf="!loading" class="table-container">
      <table mat-table [dataSource]="autores" class="mat-elevation-z8">
        <ng-container matColumnDef="nombre">
          <th mat-header-cell *matHeaderCellDef>Nombre Completo</th>
          <td mat-cell *matCellDef="let autor">{{ autor.nombreCompleto }}</td>
        </ng-container>

        <ng-container matColumnDef="fechaNacimiento">
          <th mat-header-cell *matHeaderCellDef>Fecha Nacimiento</th>
          <td mat-cell *matCellDef="let autor">{{ autor.fechaNacimiento | date:'dd/MM/yyyy' }}</td>
        </ng-container>

        <ng-container matColumnDef="ciudad">
          <th mat-header-cell *matHeaderCellDef>Ciudad</th>
          <td mat-cell *matCellDef="let autor">{{ autor.ciudadProcedencia }}</td>
        </ng-container>

        <ng-container matColumnDef="email">
          <th mat-header-cell *matHeaderCellDef>Correo Electrónico</th>
          <td mat-cell *matCellDef="let autor">{{ autor.correoElectronico }}</td>
        </ng-container>

        <ng-container matColumnDef="acciones">
          <th mat-header-cell *matHeaderCellDef>Acciones</th>
          <td mat-cell *matCellDef="let autor">
            <button mat-icon-button color="primary" [routerLink]="['/autores/editar', autor.id]">
              <mat-icon>edit</mat-icon>
            </button>
            <button mat-icon-button color="warn" (click)="eliminarAutor(autor.id)">
              <mat-icon>delete</mat-icon>
            </button>
          </td>
        </ng-container>

        <tr mat-header-row *matHeaderRowDef="displayedColumns"></tr>
        <tr mat-row *matRowDef="let row; columns: displayedColumns;"></tr>
      </table>
    </div>
  `,
  styles: [`
    .header {
      display: flex;
      justify-content: space-between;
      align-items: center;
      margin-bottom: 20px;
    }
    .loading-container {
      display: flex;
      justify-content: center;
      padding: 50px;
    }
    .table-container {
      overflow-x: auto;
    }
    table {
      width: 100%;
    }
    th, td {
      padding: 12px;
    }
  `]
})
export class ListaAutoresComponent implements OnInit {
  autores: Autor[] = [];
  displayedColumns: string[] = ['nombre', 'fechaNacimiento', 'ciudad', 'email', 'acciones'];
  loading = false;

  constructor(
    private autorService: AutorService,
    private snackBar: MatSnackBar
  ) {}

  ngOnInit(): void {
    this.cargarAutores();
  }

  cargarAutores(): void {
    this.loading = true;
    this.autorService.getAll().subscribe({
      next: (response) => {
        if (response.exito && response.datos) {
          this.autores = response.datos;
        }
        this.loading = false;
      },
      error: () => {
        this.loading = false;
      }
    });
  }

  eliminarAutor(id: number): void {
    if (confirm('¿Está seguro de eliminar este autor?')) {
      this.autorService.delete(id).subscribe({
        next: (response) => {
          if (response.exito) {
            this.snackBar.open('Autor eliminado exitosamente', 'Cerrar', { duration: 3000 });
            this.cargarAutores();
          }
        }
      });
    }
  }
}
