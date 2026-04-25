import { Component, OnInit, inject } from '@angular/core';
import { ServicioPartidos } from '../matches.service';

@Component({
  selector: 'app-match-detail',
  standalone: true,
  templateUrl: './match-detail.component.html',
  styleUrls: ['./match-detail.component.css']
})
export class MatchDetailComponent implements OnInit {
  private readonly servicioPartidos = inject(ServicioPartidos);

  ngOnInit(): void {
    void this.servicioPartidos;
  }
}
