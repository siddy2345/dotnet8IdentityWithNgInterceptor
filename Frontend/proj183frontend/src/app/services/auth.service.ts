import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import {
  AUTH_TOKEN_LOCAL_STORAGE,
  Authentication,
  REFRESH_TOKEN_LOCAL_STORAGE,
  User,
  UserInfo,
} from '../models/shared.models';
import { EventService } from './event.service';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private readonly http = inject(HttpClient);
  private readonly eventService = inject(EventService);

  constructor() {
    this.eventService.shouldRefreshToken$.subscribe(() => {
      const refreshToken = localStorage.getItem(REFRESH_TOKEN_LOCAL_STORAGE);
      if (refreshToken) this.refresh(refreshToken);
    });
  }

  public register(user: User): Observable<void> {
    return this.http.post<void>('https://localhost:7286/register', user);
  }

  public login(user: User): Observable<Authentication> {
    return this.http.post<Authentication>('https://localhost:7286/login', user);
  }

  public getMe(): Observable<UserInfo> {
    return this.http.get<UserInfo>('https://localhost:7286/manage/info');
  }

  public refresh(refreshToken: string): void {
    this.http
      .post<Authentication>('https://localhost:7286/refresh', {
        refreshToken,
      })
      .subscribe({
        next(authReturn) {
          localStorage.setItem(
            AUTH_TOKEN_LOCAL_STORAGE,
            authReturn.accessToken
          );
          localStorage.setItem(
            REFRESH_TOKEN_LOCAL_STORAGE,
            authReturn.refreshToken
          );
        },
      });
  }
}
