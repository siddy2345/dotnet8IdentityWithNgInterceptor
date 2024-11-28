import { Injectable, signal } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class EventService {
  public shouldRefreshToken$ = new BehaviorSubject(false);
  public desiredRouteSig = signal('');
}
