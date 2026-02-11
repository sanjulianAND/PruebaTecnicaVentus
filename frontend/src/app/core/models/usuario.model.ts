export interface Usuario {
  id: number;
  nombreUsuario: string;
  correoElectronico: string;
  rol: string;
}

export interface Login {
  correoElectronico: string;
  password: string;
}

export interface Registro {
  nombreUsuario: string;
  correoElectronico: string;
  password: string;
}

export interface AuthResponse {
  token: string;
  refreshToken: string;
  expiracion: Date;
  usuario: Usuario;
}
