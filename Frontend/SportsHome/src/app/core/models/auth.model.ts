export interface Usuario {
  id: number;
  correo: string;
  nombreUsuario: string;
  nombre?: string;
  apellidos?: string;
  urlAvatar?: string;
  rol: RolUsuario;
  creadoEn: string;
}

export type RolUsuario = 'ADMIN' | 'USUARIO' | 'MODERADOR';

export interface PeticionLogin {
  correo: string;
  contrasena: string;
}

export interface PeticionRegistro {
  correo: string;
  nombreUsuario: string;
  contrasena: string;
  nombre?: string;
  apellidos?: string;
}

export interface RespuestaAuth {
  tokenAcceso: string;
  tokenRefresco: string;
  expiraEn: number;
  usuario: Usuario;
}

export interface PeticionRefrescarToken {
  tokenRefresco: string;
}
