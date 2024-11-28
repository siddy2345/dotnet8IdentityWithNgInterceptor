import { inject } from '@angular/core';
import {
  ActivatedRouteSnapshot,
  CanActivateFn,
  Router,
  RouterStateSnapshot,
} from '@angular/router';
import { AuthService } from './auth.service';
import { catchError, map, of } from 'rxjs';
import { HttpErrorResponse } from '@angular/common/http';
import { USER_EMAIL } from '../models/shared.models';

export const canActivate: CanActivateFn = (
  route: ActivatedRouteSnapshot,
  state: RouterStateSnapshot
) => {
  const router = inject(Router);
  const authService = inject(AuthService);

  return authService.getMe().pipe(
    map((userInfo) => {
      if (userInfo.email.length > 0) {
        localStorage.setItem(USER_EMAIL, userInfo.email);
        return true;
      }
      return false;
    }),
    catchError((err: HttpErrorResponse) => {
      console.log(err.status);

      return of(router.createUrlTree(['login']));
    })
  );
};
