import { Routes } from '@angular/router';

export const MATCHES_ROUTES: Routes = [
  {
    path: '',
    loadComponent: () => import('./matches-list/matches-list.component').then(m => m.MatchesListComponent),
  }
];
