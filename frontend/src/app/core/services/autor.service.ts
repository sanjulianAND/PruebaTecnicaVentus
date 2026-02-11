import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Autor, CrearAutor, ActualizarAutor } from '../models/autor.model';
import { RespuestaApi, Paginacion } from '../models/api-response.model';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class AutorService {
  private readonly apiUrl = `${environment.apiUrl}/autores`;

  constructor(private http: HttpClient) {}

  getAll(): Observable<RespuestaApi<Autor[]>> {
    return this.http.get<RespuestaApi<Autor[]>>(this.apiUrl);
  }

  getPaginado(pagina: number = 1, tamanoPagina: number = 10): Observable<RespuestaApi<Paginacion<Autor>>> {
    return this.http.get<RespuestaApi<Paginacion<Autor>>>(
      `${this.apiUrl}/paginado?pagina=${pagina}&tamanoPagina=${tamanoPagina}`
    );
  }

  create(autor: CrearAutor): Observable<RespuestaApi<Autor>> {
    return this.http.post<RespuestaApi<Autor>>(this.apiUrl, autor);
  }

  update(id: number, autor: CrearAutor): Observable<RespuestaApi<Autor>> {
    const actualizarAutor: ActualizarAutor = { ...autor, id };
    return this.http.put<RespuestaApi<Autor>>(`${this.apiUrl}/${id}`, actualizarAutor);
  }

  delete(id: number): Observable<RespuestaApi<boolean>> {
    return this.http.delete<RespuestaApi<boolean>>(`${this.apiUrl}/${id}`);
  }
}
