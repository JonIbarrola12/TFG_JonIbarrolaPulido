import { Component, OnInit, inject } from '@angular/core';
import { ServicioJugadores } from '../players.service';

@Component({
  selector: 'app-players-list',
  standalone: true,
  templateUrl: './players-list.component.html',
  styleUrls: ['./players-list.component.css']
})
export class PlayersListComponent implements OnInit {
  private readonly servicioJugadores = inject(ServicioJugadores);

  ngOnInit(): void {
    void this.servicioJugadores;
  }
}
