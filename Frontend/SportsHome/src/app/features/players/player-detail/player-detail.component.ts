import { Component, OnInit, inject } from '@angular/core';
import { ServicioJugadores } from '../players.service';

@Component({
  selector: 'app-player-detail',
  standalone: true,
  templateUrl: './player-detail.component.html',
  styleUrls: ['./player-detail.component.css']
})
export class PlayerDetailComponent implements OnInit {
  private readonly servicioJugadores = inject(ServicioJugadores);

  ngOnInit(): void {
    void this.servicioJugadores;
  }
}
