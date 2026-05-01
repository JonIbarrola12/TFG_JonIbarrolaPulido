import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { UsuarioService } from '../../core/services/usuarios.service';
import { inject } from '@angular/core';
import { RouterLink } from "@angular/router";

@Component({
  selector: 'app-profile',
  standalone: true,
  imports: [FormsModule, RouterLink],
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.css']
})
export class ProfileComponent {
  private readonly usuarioService = inject(UsuarioService);
  perfil: any = {};

  ngOnInit() {
    const usuarioStr = localStorage.getItem('usuario');

    if (!usuarioStr) {
      console.error('No hay usuario en localStorage');
      return;
    }

    const usuario = JSON.parse(usuarioStr);

    this.usuarioService.getUsuario(usuario.UsuarioId)
      .subscribe(res => {
        this.perfil = res;
        console.log('Perfil API:', this.perfil);
      });
  }

  guardar() {
    const usuario = JSON.parse(localStorage.getItem('usuario')!);

    this.usuarioService.updatePerfil(usuario.UsuarioId, this.perfil)
      .subscribe((res: any) => {

        const actualizado = {
          UsuarioId: res.UsuarioId,
          NombreUsuario: res.NombreUsuario,
          Correo: res.Correo,
          UrlAvatar: res.UrlAvatar,
          Nombre : res.Nombre,
          Apellidos : res.Apellidos
        };

        localStorage.setItem('usuario', JSON.stringify(actualizado));

        alert("Perfil actualizado");

        location.reload();
      });
  }
  selectedFile: File | null = null;

  onFileSelected(event: any) {
    const file = event.target.files[0];

    if (!file) return;

    if (!file.type.startsWith('image/')) {
      alert('Solo se permiten archivos de imagen');
      return;
    }

    this.selectedFile = file;
  }

  subirAvatar() {
    if (!this.selectedFile) return;

    const usuario = JSON.parse(localStorage.getItem('usuario')!);

    const formData = new FormData();
    formData.append('file', this.selectedFile);

    this.usuarioService.uploadAvatar(usuario.UsuarioId, formData)
      .subscribe((res: any) => {

        this.perfil.UrlAvatar = res.url;

        usuario.UrlAvatar = res.url;
        localStorage.setItem('usuario', JSON.stringify(usuario));

        alert("Avatar actualizado");

        location.reload(); 
      });
  }

}
