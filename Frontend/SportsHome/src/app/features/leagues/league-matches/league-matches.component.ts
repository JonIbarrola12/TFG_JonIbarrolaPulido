import { Component, inject } from '@angular/core';
import { ActivatedRoute, RouterModule } from '@angular/router';

@Component({
  selector: 'app-league-matches',
  standalone: true,
  imports: [RouterModule],
  templateUrl: './league-matches.component.html',
  styleUrls: ['./league-matches.component.css']
})
export class LeagueMatchesComponent {
  private readonly route = inject(ActivatedRoute);

  get ligaId(): string | null {
    return this.route.snapshot.paramMap.get('id');
  }
}
