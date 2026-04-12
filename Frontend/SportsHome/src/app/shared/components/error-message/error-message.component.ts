import { Component, input, output } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-error-message',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './error-message.component.html',
  styleUrl: './error-message.component.css'
})
export class ErrorMessageComponent {
  message = input('Ha ocurrido un error inesperado.');
  type = input<'error' | 'warning' | 'info'>('error');
  retryable = input(false);
  retry = output<void>();

  onRetry(): void {
    this.retry.emit();
  }
}
