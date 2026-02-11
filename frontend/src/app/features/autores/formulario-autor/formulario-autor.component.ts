import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { MatCardModule } from '@angular/material/card';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatSnackBar } from '@angular/material/snack-bar';
import { AutorService } from '../../../core/services/autor.service';

@Component({
  selector: 'app-formulario-autor',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    RouterLink,
    MatCardModule,
    MatInputModule,
    MatButtonModule,
    MatDatepickerModule,
    MatNativeDateModule,
    MatProgressSpinnerModule
  ],
  template: `
    <div class="form-container">
      <mat-card>
        <mat-card-header>
          <mat-card-title>{{ isEdit ? 'Editar Autor' : 'Nuevo Autor' }}</mat-card-title>
        </mat-card-header>
        <mat-card-content>
          <form [formGroup]="autorForm" (ngSubmit)="onSubmit()">
            <mat-form-field appearance="outline" class="full-width">
              <mat-label>Nombre Completo *</mat-label>
              <input matInput formControlName="nombreCompleto" placeholder="Nombre del autor">
              <mat-error *ngIf="autorForm.get('nombreCompleto')?.hasError('required')">
                El nombre es obligatorio
              </mat-error>
              <mat-error *ngIf="autorForm.get('nombreCompleto')?.hasError('maxlength')">
                Máximo 200 caracteres
              </mat-error>
            </mat-form-field>

            <mat-form-field appearance="outline" class="full-width">
              <mat-label>Fecha de Nacimiento *</mat-label>
              <input matInput [matDatepicker]="picker" formControlName="fechaNacimiento">
              <mat-datepicker-toggle matSuffix [for]="picker"></mat-datepicker-toggle>
              <mat-datepicker #picker></mat-datepicker>
              <mat-error *ngIf="autorForm.get('fechaNacimiento')?.hasError('required')">
                La fecha es obligatoria
              </mat-error>
            </mat-form-field>

            <mat-form-field appearance="outline" class="full-width">
              <mat-label>Ciudad de Procedencia *</mat-label>
              <input matInput formControlName="ciudadProcedencia" placeholder="Ciudad, País">
              <mat-error *ngIf="autorForm.get('ciudadProcedencia')?.hasError('required')">
                La ciudad es obligatoria
              </mat-error>
              <mat-error *ngIf="autorForm.get('ciudadProcedencia')?.hasError('maxlength')">
                Máximo 100 caracteres
              </mat-error>
            </mat-form-field>

            <mat-form-field appearance="outline" class="full-width">
              <mat-label>Correo Electrónico *</mat-label>
              <input matInput type="email" formControlName="correoElectronico" placeholder="autor@email.com">
              <mat-error *ngIf="autorForm.get('correoElectronico')?.hasError('required')">
                El correo es obligatorio
              </mat-error>
              <mat-error *ngIf="autorForm.get('correoElectronico')?.hasError('email')">
                Ingrese un correo válido
              </mat-error>
              <mat-error *ngIf="autorForm.get('correoElectronico')?.hasError('maxlength')">
                Máximo 150 caracteres
              </mat-error>
            </mat-form-field>

            <div class="button-row">
              <button mat-button type="button" routerLink="/autores">Cancelar</button>
              <button mat-raised-button color="primary" type="submit" [disabled]="loading || autorForm.invalid">
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
export class FormularioAutorComponent implements OnInit {
  autorForm: FormGroup;
  isEdit = false;
  autorId: number | null = null;
  loading = false;

  constructor(
    private fb: FormBuilder,
    private autorService: AutorService,
    private route: ActivatedRoute,
    private router: Router,
    private snackBar: MatSnackBar
  ) {
    this.autorForm = this.fb.group({
      nombreCompleto: ['', [Validators.required, Validators.maxLength(200)]],
      fechaNacimiento: ['', Validators.required],
      ciudadProcedencia: ['', [Validators.required, Validators.maxLength(100)]],
      correoElectronico: ['', [Validators.required, Validators.email, Validators.maxLength(150)]]
    });
  }

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.isEdit = true;
      this.autorId = +id;
    }
  }

  onSubmit(): void {
    if (this.autorForm.valid) {
      this.loading = true;
      const autorData = this.autorForm.value;

      const request = this.isEdit && this.autorId
        ? this.autorService.update(this.autorId, autorData)
        : this.autorService.create(autorData);

      request.subscribe({
        next: (response) => {
          if (response.exito) {
            const mensaje = this.isEdit ? 'Autor actualizado' : 'Autor creado';
            this.snackBar.open(`${mensaje} exitosamente`, 'Cerrar', { duration: 3000 });
            this.router.navigate(['/autores']);
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
