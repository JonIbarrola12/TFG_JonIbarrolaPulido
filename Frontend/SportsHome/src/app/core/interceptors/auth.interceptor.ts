import { inject } from '@angular/core';
import { HttpInterceptorFn } from '@angular/common/http';
import { ServicioAuth } from '../services/auth.service';

export const interceptorAuth: HttpInterceptorFn = (req, next) => {
  const servicioAuth = inject(ServicioAuth);
  const token = servicioAuth.obtenerTokenAcceso();

  if (token) {
    const peticionConAuth = req.clone({
      setHeaders: {
        Authorization: `Bearer ${token}`
      }
    });
    return next(peticionConAuth);
  }

  return next(req);
};
