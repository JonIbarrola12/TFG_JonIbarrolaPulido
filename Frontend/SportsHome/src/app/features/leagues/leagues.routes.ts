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
    path: ':id/estadisticas',
    loadComponent: () => import('./league-stadistics/league-stadistics.component').then(m => m.LeagueStadisticsComponent),
  },
  {
    path: ':id/partidos',
    loadComponent: () => import('./league-matches/league-matches.component').then(m => m.LeagueMatchesComponent),
  },
  {
    path: ':id',
    loadComponent: () => import('./league-detail/league-detail.component').then(m => m.LeagueDetailComponent),
  },
  {
    path: ':id/equipos/:equipoId/jugadores',
    loadComponent: () => import('./league-players/league-players.component').then(m => m.LeaguePlayersComponent)
  }
];
