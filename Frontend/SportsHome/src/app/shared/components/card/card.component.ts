import { Component, input } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-card',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './card.component.html',
  styleUrl: './card.component.css'
})
export class CardComponent {
  title = input('');
  subtitle = input('');
  padding = input<'none' | 'sm' | 'md' | 'lg'>('md');
  hoverable = input(false);
}
