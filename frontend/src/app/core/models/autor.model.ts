export interface Autor {
  id: number;
  nombreCompleto: string;
  fechaNacimiento: Date;
  ciudadProcedencia: string;
  correoElectronico: string;
}

export interface CrearAutor {
  nombreCompleto: string;
  fechaNacimiento: Date;
  ciudadProcedencia: string;
  correoElectronico: string;
}

export interface ActualizarAutor extends CrearAutor {
  id: number;
}
