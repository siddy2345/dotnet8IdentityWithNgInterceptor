import {
  HttpErrorResponse,
  HttpHandlerFn,
  HttpRequest,
  HttpStatusCode,
} from '@angular/common/http';
import { AUTH_TOKEN_LOCAL_STORAGE } from '../models/shared.models';
import { catchError } from 'rxjs';
import { inject } from '@angular/core';
import { EventService } from './event.service';

export function authInterceptor(
  req: HttpRequest<unknown>,
  next: HttpHandlerFn
) {
  const token = localStorage.getItem(AUTH_TOKEN_LOCAL_STORAGE);
  const eventService = inject(EventService);

  if (token)
    req = req.clone({
      setHeaders: { Authorization: `Bearer ${token}` },
    });

  return next(req).pipe(
    catchError((err: HttpErrorResponse) => {
      if (err.status === HttpStatusCode.Unauthorized)
        eventService.shouldRefreshToken$.next(true);

      return next(req);
    })
  );
}
