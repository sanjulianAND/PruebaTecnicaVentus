import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Libro, CrearLibro } from '../models/libro.model';
import { RespuestaApi } from '../models/api-response.model';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class LibroService {
  private readonly apiUrl = `${environment.apiUrl}/libros`;

  constructor(private http: HttpClient) {}

  getAll(): Observable<RespuestaApi<Libro[]>> {
    return this.http.get<RespuestaApi<Libro[]>>(this.apiUrl);
  }

  create(libro: CrearLibro): Observable<RespuestaApi<Libro>> {
    return this.http.post<RespuestaApi<Libro>>(this.apiUrl, libro);
  }

  update(id: number, libro: CrearLibro): Observable<RespuestaApi<Libro>> {
    return this.http.put<RespuestaApi<Libro>>(`${this.apiUrl}/${id}`, { ...libro, id });
  }

  delete(id: number): Observable<RespuestaApi<boolean>> {
    return this.http.delete<RespuestaApi<boolean>>(`${this.apiUrl}/${id}`);
  }
}
