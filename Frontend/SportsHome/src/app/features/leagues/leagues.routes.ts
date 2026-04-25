import { Routes } from '@angular/router';

export const LEAGUES_ROUTES: Routes = [
  {
    path: '',
    loadComponent: () => import('./leagues-list/leagues-list.component').then(m => m.LeaguesListComponent),
  },
  {
    path: ':id/equipos',
    loadComponent: () => import('./league-teams/league-teams.component').then(m => m.LeagueTeamsComponent),
  },
  {
    path: ':id/clasificacion',
    loadComponent: () => import('./league-classification/league-classification.component').then(m => m.LeagueClassificationComponent),
  },
  {
    path: ':id/partidos',
    loadComponent: () => import('./league-matches/league-matches.component').then(m => m.LeagueMatchesComponent),
  },
  {
    path: ':id',
    loadComponent: () => import('./league-detail/league-detail.component').then(m => m.LeagueDetailComponent),
  },
];
