export interface RespuestaApi<T> {
  exito: boolean;
  mensaje?: string;
  datos?: T;
  errores?: string[];
}

export interface Paginacion<T> {
  items: T[];
  totalItems: number;
  paginaActual: number;
  totalPaginas: number;
  tamanoPagina: number;
  tienePaginaAnterior: boolean;
  tienePaginaSiguiente: boolean;
}
