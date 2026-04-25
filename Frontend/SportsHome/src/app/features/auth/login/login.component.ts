import { Component, OnInit, inject } from '@angular/core';
import { ServicioAuthFeature } from '../auth.service';

@Component({
  selector: 'app-login',
  standalone: true,
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  private readonly servicioAuthFeature = inject(ServicioAuthFeature);

  ngOnInit(): void {
    void this.servicioAuthFeature;
  }
}
