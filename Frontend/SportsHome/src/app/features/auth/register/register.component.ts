import { Component, OnInit, inject } from '@angular/core';
import { ServicioAuthFeature } from '../auth.service';

@Component({
  selector: 'app-register',
  standalone: true,
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  private readonly servicioAuthFeature = inject(ServicioAuthFeature);

  ngOnInit(): void {
    void this.servicioAuthFeature;
  }
}
