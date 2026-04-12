import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { ServicioAuth } from '../services/auth.service';
import { RUTAS_APP } from '../constants/api.constants';

export const guardiaAutenticacion: CanActivateFn = () => {
  const servicioAuth = inject(ServicioAuth);
  const router = inject(Router);

  if (servicioAuth.estaAutenticado()) {
    return true;
  }

  router.navigate([RUTAS_APP.LOGIN]);
  return false;
};

export const guardiaInvitado: CanActivateFn = () => {
  const servicioAuth = inject(ServicioAuth);
  const router = inject(Router);

  if (!servicioAuth.estaAutenticado()) {
    return true;
  }

  router.navigate([RUTAS_APP.INICIO]);
  return false;
};

export const guardiaAdmin: CanActivateFn = () => {
  const servicioAuth = inject(ServicioAuth);
  const router = inject(Router);

  if (servicioAuth.estaAutenticado() && servicioAuth.esAdmin()) {
    return true;
  }

  router.navigate([RUTAS_APP.INICIO]);
  return false;
};
