import { inject } from '@angular/core';
import { HttpInterceptorFn, HttpErrorResponse } from '@angular/common/http';
import { Router } from '@angular/router';
import { catchError, throwError, switchMap } from 'rxjs';
import { ServicioAuth } from '../services/auth.service';
import { RUTAS_APP } from '../constants/api.constants';

export const interceptorError: HttpInterceptorFn = (req, next) => {
  const servicioAuth = inject(ServicioAuth);
  const router = inject(Router);

  return next(req).pipe(
    catchError((error: HttpErrorResponse) => {
      if (error.status === 401) {
        const tokenRefresco = servicioAuth.obtenerTokenRefresco();
        if (tokenRefresco) {
          return servicioAuth.refrescarToken().pipe(
            switchMap((respuestaAuth) => {
              const peticionReintento = req.clone({
                setHeaders: {
                  Authorization: `Bearer ${respuestaAuth.tokenAcceso}`
                }
              });
              return next(peticionReintento);
            }),
            catchError(() => {
              servicioAuth.cerrarSesion();
              return throwError(() => error);
            })
          );
        }
        servicioAuth.cerrarSesion();
      }

      if (error.status === 403) {
        router.navigate([RUTAS_APP.INICIO]);
      }

      if (error.status === 404) {
        router.navigate([RUTAS_APP.NO_ENCONTRADO]);
      }

      return throwError(() => error);
    })
  );
};
