import { Component, inject } from '@angular/core';
import { ActivatedRoute, RouterModule } from '@angular/router';

@Component({
  selector: 'app-league-classification',
  standalone: true,
  imports: [RouterModule],
  templateUrl: './league-classification.component.html',
  styleUrls: ['./league-classification.component.css']
})
export class LeagueClassificationComponent {
  private readonly route = inject(ActivatedRoute);

  get ligaId(): string | null {
    return this.route.snapshot.paramMap.get('id');
  }
}
