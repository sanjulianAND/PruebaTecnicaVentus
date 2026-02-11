import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, BehaviorSubject, tap } from 'rxjs';
import { Login, Registro, AuthResponse, Usuario } from '../models/usuario.model';
import { RespuestaApi } from '../models/api-response.model';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private readonly apiUrl = `${environment.apiUrl}/auth`;
  private readonly tokenKey = 'token';
  private readonly usuarioKey = 'usuario';
  
  private usuarioSubject = new BehaviorSubject<Usuario | null>(this.getUsuario());
  usuario$ = this.usuarioSubject.asObservable();

  constructor(private http: HttpClient) {}

  login(credentials: Login): Observable<RespuestaApi<AuthResponse>> {
    return this.http.post<RespuestaApi<AuthResponse>>(`${this.apiUrl}/login`, credentials)
      .pipe(
        tap(response => {
          if (response.exito && response.datos) {
            this.setSession(response.datos);
          }
        })
      );
  }

  register(data: Registro): Observable<RespuestaApi<AuthResponse>> {
    return this.http.post<RespuestaApi<AuthResponse>>(`${this.apiUrl}/register`, data)
      .pipe(
        tap(response => {
          if (response.exito && response.datos) {
            this.setSession(response.datos);
          }
        })
      );
  }

  logout(): void {
    localStorage.removeItem(this.tokenKey);
    localStorage.removeItem(this.usuarioKey);
    this.usuarioSubject.next(null);
  }

  isLoggedIn(): boolean {
    return !!this.getToken();
  }

  getToken(): string | null {
    return localStorage.getItem(this.tokenKey);
  }

  getUsuario(): Usuario | null {
    const usuarioStr = localStorage.getItem(this.usuarioKey);
    return usuarioStr ? JSON.parse(usuarioStr) : null;
  }

  private setSession(authResponse: AuthResponse): void {
    localStorage.setItem(this.tokenKey, authResponse.token);
    localStorage.setItem(this.usuarioKey, JSON.stringify(authResponse.usuario));
    this.usuarioSubject.next(authResponse.usuario);
  }
}
