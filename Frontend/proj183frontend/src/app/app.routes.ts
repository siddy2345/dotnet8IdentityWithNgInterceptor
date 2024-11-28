import { Routes } from '@angular/router';
import { canActivate } from './services/auth.guard';

export const routes: Routes = [
  {
    path: 'login',
    loadComponent: () =>
      import('./login/login.component').then((m) => m.LoginComponent),
  },
  {
    path: 'court-editor',
    canActivate: [canActivate],
    loadComponent: () =>
      import('./court/court-editor/court-editor.component').then(
        (m) => m.CourtEditorComponent
      ),
  },
  {
    path: 'courts',
    loadComponent: () =>
      import('./court/court-list/court-list.component').then(
        (m) => m.CourtListComponent
      ),
  },
];
