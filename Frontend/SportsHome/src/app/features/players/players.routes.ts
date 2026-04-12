import { Routes } from '@angular/router';

export const PLAYERS_ROUTES: Routes = [
  {
    path: '',
    loadComponent: () => import('./pages/players-list/players-list.component').then(m => m.PlayersListComponent),
  },
  {
    path: ':id',
    loadComponent: () => import('./pages/player-detail/player-detail.component').then(m => m.PlayerDetailComponent),
  },
];
