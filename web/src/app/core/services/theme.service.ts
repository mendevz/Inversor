import { Injectable, signal, effect } from '@angular/core';

export type Theme = 'dark' | 'light';

@Injectable({
    providedIn: 'root'
})
export class ThemeService {

    readonly theme = signal<Theme>(this.getInitialTheme());

    constructor() {
        effect(() => {
            const currentTheme = this.theme();
            const root = document.documentElement;

            if (currentTheme === 'dark') {
                root.classList.add('dark');
            } else {
                root.classList.remove('dark');
            }

            localStorage.setItem('inversor-theme', currentTheme);
        });
    }

    toggleTheme(): void {
        this.theme.update(current => (current === 'dark' ? 'light' : 'dark'));
    }

    private getInitialTheme(): Theme {
        const saved = localStorage.getItem('inversor-theme') as Theme;
        if (saved === 'dark' || saved === 'light') {
            return saved;
        }
        return window.matchMedia('(prefers-color-scheme: dark)').matches ? 'dark' : 'light';
    }
}
