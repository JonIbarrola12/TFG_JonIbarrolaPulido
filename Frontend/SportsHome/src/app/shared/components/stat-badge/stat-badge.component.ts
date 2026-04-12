import { Component, input } from '@angular/core';

@Component({
  selector: 'app-stat-badge',
  standalone: true,
  templateUrl: './stat-badge.component.html',
  styleUrl: './stat-badge.component.css'
})
export class StatBadgeComponent {
  label = input.required<string>();
  value = input.required<string | number>();
  unit = input('');
  variant = input<'default' | 'primary' | 'success' | 'warning' | 'danger'>('default');
  size = input<'sm' | 'md'>('md');
}
