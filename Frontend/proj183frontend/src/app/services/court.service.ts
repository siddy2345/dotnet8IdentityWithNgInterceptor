import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';
import { CourtViewModel } from '../models/shared.models';

@Injectable({
  providedIn: 'root',
})
export class CourtService {
  private readonly http = inject(HttpClient);

  public getCourts(): Observable<CourtViewModel[]> {
    return this.http.get<CourtViewModel[]>('https://localhost:7286/courts');
  }
}
