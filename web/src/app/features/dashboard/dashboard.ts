import { Component, signal } from '@angular/core';
import { GlassCard } from '../../shared/components/glass-card/glass-card';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';

export type UserTier = 'guest' | 'free' | 'premium';
@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './dashboard.html'
})
export class Dashboard {

  // Cambia esto a 'premium' para ver la interfaz completa
  public currentTier = signal<UserTier>('premium');
  // Datos mockeados de la cola SRS
  public srsQueue = {
    dueToday: 12,
    overdue: 3,
    totalPending: 15
  };
  topics = [
    { name: 'Past Simple', score: 95 },
    { name: 'Phrasal Verbs', score: 65 },
    { name: 'Prepositions (In/On/At)', score: 35 },
    { name: 'Present Perfect', score: 80 }
  ];
}
