import { Component, input, computed } from '@angular/core';
@Component({
  selector: 'app-topic-badge',
  standalone: true,
  template: `
     <!-- Note: rounded en lugar de rounded-full, y font-mono para los datos numéricos -->
    <div class="inline-flex items-center gap-2 rounded border px-2 py-1 text-xs font-medium uppercase tracking-wide transition-colors"
         [class]="colorClasses()">
      <div class="h-1.5 w-1.5 rounded-sm" [class]="dotClasses()"></div>
      <span>{{ label() }}</span>
      <span class="font-mono opacity-80 border-l border-current pl-2 ml-1">{{ score() }}%</span>
    </div>
  `
})
export class TopicBadgeComponent {
  label = input.required<string>();
  score = input.required<number>();
  colorClasses = computed(() => {
    const s = this.score();
    // Colores sólidos y técnicos (Emerald, Amber, Rose)
    if (s >= 80) return 'border-brand-500/30 bg-brand-500/10 text-brand-700 dark:text-brand-400';
    if (s >= 50) return 'border-amber-500/30 bg-amber-500/10 text-amber-700 dark:text-amber-400';
    return 'border-rose-500/30 bg-rose-500/10 text-rose-700 dark:text-rose-400';
  });
  dotClasses = computed(() => {
    const s = this.score();
    if (s >= 80) return 'bg-brand-500';
    if (s >= 50) return 'bg-amber-500';
    return 'bg-rose-500';
  });
}