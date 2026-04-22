import { Component } from '@angular/core';

@Component({
  selector: 'app-match-detail',
  standalone: true,
  template: '<div class="page-placeholder"><h2>Detalle de Partido</h2><p>Próximamente...</p></div>',
  styles: ['.page-placeholder{padding:3rem 1.5rem;text-align:center;color:var(--color-text-secondary)}h2{font-size:1.75rem;color:var(--color-text-primary);margin-bottom:.5rem}']
})
export class MatchDetailComponent {}
