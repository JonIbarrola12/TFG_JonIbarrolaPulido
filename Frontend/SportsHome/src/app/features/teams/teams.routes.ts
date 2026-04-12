import { Routes } from '@angular/router';

export const TEAMS_ROUTES: Routes = [
  {
    path: '',
    loadComponent: () => import('./pages/teams-list/teams-list.component').then(m => m.TeamsListComponent),
  },
  {
    path: ':id',
    loadComponent: () => import('./pages/team-detail/team-detail.component').then(m => m.TeamDetailComponent),
  },
];
