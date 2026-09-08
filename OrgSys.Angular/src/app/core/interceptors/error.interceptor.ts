import { HttpErrorResponse, HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { catchError, throwError } from 'rxjs';
import { AuthService } from '../auth/auth.service';

/**
 * The API answers business failures with HTTP 200 (see ApiResult) — a real HTTP error here
 * means transport/auth failure. 401/403 (missing/expired/invalid token) forces a re-login.
 */
export const errorInterceptor: HttpInterceptorFn = (req, next) => {
  const auth = inject(AuthService);

  return next(req).pipe(
    catchError((error: unknown) => {
      if (error instanceof HttpErrorResponse && (error.status === 401 || error.status === 403)) {
        auth.logout();
      }

      return throwError(() => error);
    }),
  );
};
