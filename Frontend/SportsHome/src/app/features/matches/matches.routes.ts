import { Routes } from '@angular/router';

export const MATCHES_ROUTES: Routes = [
  {
    path: '',
    loadComponent: () => import('./pages/matches-list/matches-list.component').then(m => m.MatchesListComponent),
  },
  {
    path: ':id',
    loadComponent: () => import('./pages/match-detail/match-detail.component').then(m => m.MatchDetailComponent),
  },
];
