export interface Libro {
  id: number;
  titulo: string;
  anio: number;
  genero: string;
  numeroPaginas: number;
  autorId: number;
  autorNombre: string;
}

export interface CrearLibro {
  titulo: string;
  anio: number;
  genero: string;
  numeroPaginas: number;
  autorId: number;
}

export interface ActualizarLibro extends CrearLibro {
  id: number;
}
