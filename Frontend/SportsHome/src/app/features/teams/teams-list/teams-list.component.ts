import { Component, OnInit, inject } from '@angular/core';
import { ServicioEquipos } from '../teams.service';

@Component({
  selector: 'app-teams-list',
  standalone: true,
  templateUrl: './teams-list.component.html',
  styleUrls: ['./teams-list.component.css']
})
export class TeamsListComponent implements OnInit {
  private readonly servicioEquipos = inject(ServicioEquipos);

  ngOnInit(): void {
    void this.servicioEquipos;
  }
}
