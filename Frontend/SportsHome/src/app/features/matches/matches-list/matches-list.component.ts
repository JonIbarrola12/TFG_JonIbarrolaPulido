import { Component, OnInit, inject } from '@angular/core';
import { ServicioPartidos } from '../matches.service';

@Component({
  selector: 'app-matches-list',
  standalone: true,
  templateUrl: './matches-list.component.html',
  styleUrls: ['./matches-list.component.css']
})
export class MatchesListComponent implements OnInit {
  private readonly servicioPartidos = inject(ServicioPartidos);

  ngOnInit(): void {
    void this.servicioPartidos;
  }
}
