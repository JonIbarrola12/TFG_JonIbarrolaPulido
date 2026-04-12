import { Routes } from '@angular/router';

export const LEAGUES_ROUTES: Routes = [
  {
    path: '',
    loadComponent: () => import('./pages/leagues-list/leagues-list.component').then(m => m.LeaguesListComponent),
  },
  {
    path: ':id',
    loadComponent: () => import('./pages/league-detail/league-detail.component').then(m => m.LeagueDetailComponent),
  },
];
