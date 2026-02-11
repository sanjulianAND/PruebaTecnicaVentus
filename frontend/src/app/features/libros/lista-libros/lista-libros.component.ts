import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { MatTableModule } from '@angular/material/table';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { Libro } from '../../../core/models/libro.model';
import { LibroService } from '../../../core/services/libro.service';

@Component({
  selector: 'app-lista-libros',
  standalone: true,
  imports: [
    CommonModule,
    RouterLink,
    MatTableModule,
    MatButtonModule,
    MatIconModule,
    MatProgressSpinnerModule,
    MatDialogModule
  ],
  template: `
    <div class="header">
      <h1>Gestión de Libros</h1>
      <button mat-raised-button color="primary" routerLink="/libros/nuevo">
        <mat-icon>add</mat-icon>
        Nuevo Libro
      </button>
    </div>

    <div *ngIf="loading" class="loading-container">
      <mat-spinner></mat-spinner>
    </div>

    <div *ngIf="!loading" class="table-container">
      <table mat-table [dataSource]="libros" class="mat-elevation-z8">
        <ng-container matColumnDef="titulo">
          <th mat-header-cell *matHeaderCellDef>Título</th>
          <td mat-cell *matCellDef="let libro">{{ libro.titulo }}</td>
        </ng-container>

        <ng-container matColumnDef="autor">
          <th mat-header-cell *matHeaderCellDef>Autor</th>
          <td mat-cell *matCellDef="let libro">{{ libro.autorNombre }}</td>
        </ng-container>

        <ng-container matColumnDef="anio">
          <th mat-header-cell *matHeaderCellDef>Año</th>
          <td mat-cell *matCellDef="let libro">{{ libro.anio }}</td>
        </ng-container>

        <ng-container matColumnDef="genero">
          <th mat-header-cell *matHeaderCellDef>Género</th>
          <td mat-cell *matCellDef="let libro">{{ libro.genero }}</td>
        </ng-container>

        <ng-container matColumnDef="paginas">
          <th mat-header-cell *matHeaderCellDef>Páginas</th>
          <td mat-cell *matCellDef="let libro">{{ libro.numeroPaginas }}</td>
        </ng-container>

        <ng-container matColumnDef="acciones">
          <th mat-header-cell *matHeaderCellDef>Acciones</th>
          <td mat-cell *matCellDef="let libro">
            <button mat-icon-button color="primary" [routerLink]="['/libros/editar', libro.id]">
              <mat-icon>edit</mat-icon>
            </button>
            <button mat-icon-button color="warn" (click)="eliminarLibro(libro.id)">
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
export class ListaLibrosComponent implements OnInit {
  libros: Libro[] = [];
  displayedColumns: string[] = ['titulo', 'autor', 'anio', 'genero', 'paginas', 'acciones'];
  loading = false;

  constructor(
    private libroService: LibroService,
    private snackBar: MatSnackBar,
    private dialog: MatDialog
  ) {}

  ngOnInit(): void {
    this.cargarLibros();
  }

  cargarLibros(): void {
    this.loading = true;
    this.libroService.getAll().subscribe({
      next: (response) => {
        if (response.exito && response.datos) {
          this.libros = response.datos;
        }
        this.loading = false;
      },
      error: () => {
        this.loading = false;
      }
    });
  }

  eliminarLibro(id: number): void {
    if (confirm('¿Está seguro de eliminar este libro?')) {
      this.libroService.delete(id).subscribe({
        next: (response) => {
          if (response.exito) {
            this.snackBar.open('Libro eliminado exitosamente', 'Cerrar', { duration: 3000 });
            this.cargarLibros();
          }
        }
      });
    }
  }
}
