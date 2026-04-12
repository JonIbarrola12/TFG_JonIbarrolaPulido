import { Routes } from '@angular/router';
import { guardiaAutenticacion, guardiaInvitado } from './core/guards/auth.guard';

export const routes: Routes = [
  {
    path: '',
    loadComponent: () => import('./pages/home/home.component').then(m => m.HomeComponent),
  },
  {
    path: 'ligas',
    loadChildren: () => import('./features/leagues/leagues.routes').then(m => m.LEAGUES_ROUTES),
  },
  {
    path: 'equipos',
    loadChildren: () => import('./features/teams/teams.routes').then(m => m.TEAMS_ROUTES),
  },
  {
    path: 'jugadores',
    loadChildren: () => import('./features/players/players.routes').then(m => m.PLAYERS_ROUTES),
  },
  {
    path: 'partidos',
    loadChildren: () => import('./features/matches/matches.routes').then(m => m.MATCHES_ROUTES),
  },
  {
    path: 'auth',
    canActivate: [guardiaInvitado],
    loadChildren: () => import('./features/auth/auth.routes').then(m => m.AUTH_ROUTES),
  },
  {
    path: 'perfil',
    canActivate: [guardiaAutenticacion],
    loadComponent: () => import('./pages/profile/profile.component').then(m => m.ProfileComponent),
  },
  {
    path: '404',
    loadComponent: () => import('./pages/not-found/not-found.component').then(m => m.NotFoundComponent),
  },
  {
    path: '**',
    redirectTo: '404',
  },
];
