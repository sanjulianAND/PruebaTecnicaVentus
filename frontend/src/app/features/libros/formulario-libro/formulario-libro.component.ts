import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { MatCardModule } from '@angular/material/card';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatSelectModule } from '@angular/material/select';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatSnackBar } from '@angular/material/snack-bar';
import { LibroService } from '../../../core/services/libro.service';
import { AutorService } from '../../../core/services/autor.service';
import { Autor } from '../../../core/models/autor.model';

@Component({
  selector: 'app-formulario-libro',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    RouterLink,
    MatCardModule,
    MatInputModule,
    MatButtonModule,
    MatSelectModule,
    MatProgressSpinnerModule
  ],
  template: `
    <div class="form-container">
      <mat-card>
        <mat-card-header>
          <mat-card-title>{{ isEdit ? 'Editar Libro' : 'Nuevo Libro' }}</mat-card-title>
        </mat-card-header>
        <mat-card-content>
          <form [formGroup]="libroForm" (ngSubmit)="onSubmit()">
            <mat-form-field appearance="outline" class="full-width">
              <mat-label>Título *</mat-label>
              <input matInput formControlName="titulo" placeholder="Título del libro">
              <mat-error *ngIf="libroForm.get('titulo')?.hasError('required')">
                El título es obligatorio
              </mat-error>
              <mat-error *ngIf="libroForm.get('titulo')?.hasError('maxlength')">
                Máximo 300 caracteres
              </mat-error>
            </mat-form-field>

            <mat-form-field appearance="outline" class="full-width">
              <mat-label>Autor *</mat-label>
              <mat-select formControlName="autorId">
                <mat-option *ngFor="let autor of autores" [value]="autor.id">
                  {{ autor.nombreCompleto }}
                </mat-option>
              </mat-select>
              <mat-error *ngIf="libroForm.get('autorId')?.hasError('required')">
                El autor es obligatorio
              </mat-error>
            </mat-form-field>

            <mat-form-field appearance="outline" class="full-width">
              <mat-label>Año *</mat-label>
              <input matInput type="number" formControlName="anio" placeholder="2024">
              <mat-error *ngIf="libroForm.get('anio')?.hasError('required')">
                El año es obligatorio
              </mat-error>
              <mat-error *ngIf="libroForm.get('anio')?.hasError('min') || libroForm.get('anio')?.hasError('max')">
                Año inválido
              </mat-error>
            </mat-form-field>

            <mat-form-field appearance="outline" class="full-width">
              <mat-label>Género *</mat-label>
              <input matInput formControlName="genero" placeholder="Novela, Poesía, etc.">
              <mat-error *ngIf="libroForm.get('genero')?.hasError('required')">
                El género es obligatorio
              </mat-error>
            </mat-form-field>

            <mat-form-field appearance="outline" class="full-width">
              <mat-label>Número de Páginas *</mat-label>
              <input matInput type="number" formControlName="numeroPaginas" placeholder="300">
              <mat-error *ngIf="libroForm.get('numeroPaginas')?.hasError('required')">
                El número de páginas es obligatorio
              </mat-error>
              <mat-error *ngIf="libroForm.get('numeroPaginas')?.hasError('min')">
                Debe ser mayor a 0
              </mat-error>
            </mat-form-field>

            <div class="button-row">
              <button mat-button type="button" routerLink="/libros">Cancelar</button>
              <button mat-raised-button color="primary" type="submit" [disabled]="loading || libroForm.invalid">
                <mat-spinner diameter="20" *ngIf="loading"></mat-spinner>
                <span *ngIf="!loading">{{ isEdit ? 'Actualizar' : 'Guardar' }}</span>
              </button>
            </div>
          </form>
        </mat-card-content>
      </mat-card>
    </div>
  `,
  styles: [`
    .form-container {
      max-width: 600px;
      margin: 0 auto;
    }
    .full-width {
      width: 100%;
      margin-bottom: 15px;
    }
    .button-row {
      display: flex;
      justify-content: flex-end;
      gap: 10px;
      margin-top: 20px;
    }
    mat-spinner {
      display: inline-block;
      margin-right: 8px;
    }
  `]
})
export class FormularioLibroComponent implements OnInit {
  libroForm: FormGroup;
  autores: Autor[] = [];
  isEdit = false;
  libroId: number | null = null;
  loading = false;

  constructor(
    private fb: FormBuilder,
    private libroService: LibroService,
    private autorService: AutorService,
    private route: ActivatedRoute,
    private router: Router,
    private snackBar: MatSnackBar
  ) {
    const currentYear = new Date().getFullYear();
    this.libroForm = this.fb.group({
      titulo: ['', [Validators.required, Validators.maxLength(300)]],
      autorId: ['', Validators.required],
      anio: ['', [Validators.required, Validators.min(1000), Validators.max(currentYear)]],
      genero: ['', [Validators.required, Validators.maxLength(100)]],
      numeroPaginas: ['', [Validators.required, Validators.min(1)]]
    });
  }

  ngOnInit(): void {
    this.cargarAutores();
    
    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.isEdit = true;
      this.libroId = +id;
      // En una implementación real, deberías cargar los datos del libro
    }
  }

  cargarAutores(): void {
    this.autorService.getAll().subscribe({
      next: (response) => {
        if (response.exito && response.datos) {
          this.autores = response.datos;
        }
      }
    });
  }

  onSubmit(): void {
    if (this.libroForm.valid) {
      this.loading = true;
      const libroData = this.libroForm.value;

      const request = this.isEdit && this.libroId
        ? this.libroService.update(this.libroId, libroData)
        : this.libroService.create(libroData);

      request.subscribe({
        next: (response) => {
          if (response.exito) {
            const mensaje = this.isEdit ? 'Libro actualizado' : 'Libro creado';
            this.snackBar.open(`${mensaje} exitosamente`, 'Cerrar', { duration: 3000 });
            this.router.navigate(['/libros']);
          }
          this.loading = false;
        },
        error: () => {
          this.loading = false;
        }
      });
    }
  }
}
