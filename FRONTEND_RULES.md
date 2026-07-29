# Inversor Frontend - System Architecture & UI/UX Philosophy

Este documento es la fuente de verdad (Single Source of Truth) para todo el desarrollo del Frontend de Inversor. Cualquier código, diseño o componente nuevo debe alinearse estrictamente con estas reglas.

## 1. Visión del Producto (Product Philosophy)
- **Es una HERRAMIENTA, no un "Chat de IA":** La inteligencia artificial es un motor invisible, no el protagonista. El protagonista es el sistema analítico de aprendizaje (Topic Mastery) y la repetición espaciada (SRS).
- **Valor Inmediato (Dashboard First):** El usuario no debe enfrentarse a un lienzo en blanco preguntándose "qué hacer". La aplicación debe guiarlo proactivamente ("Tienes 3 debilidades pendientes por repasar hoy").
- **Flujo de "Code Review":** Las evaluaciones se presentan como un análisis clínico (diferencias de texto en rojo/verde, etiquetas de gramática), no como una conversación casual.

## 2. Arquitectura de Software (Angular 18+)
- **Standalone Components:** PROHIBIDO el uso de `NgModules`. Todo el desarrollo utilizará componentes, directivas y pipes independientes (`standalone: true`).
- **Control de Estado Híbrido:**
  - Usar **Signals** (`signal`, `computed`, `effect`) para el estado síncrono y reactividad local de la UI.
  - Usar **RxJS** estrictamente para flujos asíncronos complejos (SignalR WebSockets, HTTP requests, debouncing).
- **Estructura de Carpetas (Domain-Driven):**
  - `/core`: Servicios Singleton (SignalR, Auth, Theme), Guards, Interceptors.
  - `/shared`: Componentes UI reutilizables (Botones, Tarjetas, Inputs), Pipes, Directivas.
  - `/features`: Módulos de dominio (Dashboard, Evaluation, PracticeMode).
- **Tipado Estricto (TypeScript):** Los DTOs del frontend DEBEN coincidir exactamente con los del backend de .NET. (Recomendado: autogeneración vía OpenAPI).

## 3. UI/UX Design System (Estética iOS / GitHub Mobile)
- **Filosofía Base (Zero Clutter & Reducción Extrema):** La interfaz debe sentirse limpia, utilitaria y silenciosa. Sin cabeceras genéricas, sin "bienvenidas" redundantes. El usuario debe ver directamente la acción y las métricas. 
  - Prohibido el uso de íconos decorativos grandes (con fondo sólido) que sumen "carga visual".
  - Los widgets informativos (ej. estados, alertas) deben ser banners compactos e inline, no titulares gigantes.
  - Priorizar el diseño tipográfico, la densidad de datos y el formato "Píldora/Bento Grid" (compacto).
- **Framework CSS:** Tailwind CSS. (PROHIBIDO Angular Material, Bootstrap o librerías monolíticas).
- **Paleta de Colores (Card-based UI):**
  - *Fondos de Pantalla:* Gris perlado en claro (`#f6f8fa`) y negro suavizado en oscuro (`#010409`).
  - *Superficies (Tarjetas):* **Prohibidos los bloques sólidos** y los bordes duros. En modo claro usar cristal translúcido (`bg-white/60` y `border-transparent`), y en modo oscuro usar `bg-white/5` y `border-transparent`. Todo debe tener un sutil difuminado (`backdrop-blur-xl`) para que el blur de fondo traspase orgánicamente.
  - *Accentos (Iconos y Botones):* Tonos **Mate/Pastel** inspirados en GitHub Copilot Mobile. 
    - Gris Mate: `bg-neutral-500/15 text-neutral-400`
    - Azul Mate: `bg-blue-500/15 text-blue-400`
    - Verde Mate: `bg-emerald-500/15 text-emerald-400`
    Prohibidos los colores sólidos brillantes. Todos los fondos de botones e iconos deben ser translúcidos con opacidades bajas (`/15` a `/30`) para lograr un efecto elegante. Los contenedores de iconos deben ser redondos (`rounded-full`).
  - *Ambient Glow:* Permitido usar un sutil resplandor en la parte superior del background (`blur-3xl bg-gradient-to-br`) para evitar que el fondo sea totalmente plano, manteniendo una opacidad bajísima.
  - *Sombreado / Flat Design:* **Prohibido el uso de sombras (`shadow-sm`) en botones.** Todo debe sentirse plano y amigable. Las tarjetas pueden tener una sutil elevación solo por contraste de color, no por sombras gruesas.
  - *Textos de Alerta:* Colores de error/atraso deben ser atenuados (ej. `text-red-400/80` en lugar de `text-red-500`) para no ser agresivos.
  - *Acento:* Verde Esmeralda Técnico (`#10b981`), sólido y sin gradientes para indicar éxito/acción.
  - *Bordes:* Sutiles y definidos (`border-neutral-800` a `border-neutral-700`).
- **Tipografía (Data Density):** 
  - `Inter` para lectura general.
  - **Monospace** (`font-mono`) obligatoria para métricas, porcentajes, botones técnicos y etiquetas, emulando un Code Editor (tipo VS Code).
- **Geometría y Efectos:**
  - Cero *Glassmorphism* y cero brillos (*glows*). 
  - Radios de borde secos (`rounded-md` o `rounded-lg`). PROHIBIDOS los `rounded-2xl`.
  - Botones y tarjetas planas (Flat Design) con transiciones rápidas de estado (`hover:border-neutral-700`).


## 4. Estrategia Mobile-First (PWA)
- La aplicación se empaquetará como Progressive Web App (PWA).
- **Bottom Sheets:** En resoluciones móviles, evitar popups en el centro de la pantalla; usar paneles deslizables desde la base de la pantalla (como las apps nativas de iOS/Android).
- **Touch Targets:** Todo botón interactivo debe tener al menos 44x44px.

## 5. Estrategias Potenciadoras (Boosters)
- **Optimistic UI:** Cuando un usuario realice una acción local (cerrar una tarjeta, marcar como leído), la UI debe reaccionar al instante, sin esperar la respuesta del servidor.
- **Feedback Háptico/Visual:** Para eventos importantes (ej. Topic Mastery alcanzó el 100%), emitir animaciones sutiles (ej. un flash verde esmeralda en el borde de la pantalla) para gamificar la experiencia y generar dopamina en el aprendizaje.

## 6. Arquitectura UX por Tiers (Guest, Free, Premium)
El Dashboard y la navegación deben reaccionar dinámicamente al nivel de autenticación del usuario, reflejando estrictamente las reglas de negocio del Backend:
- **Guest (Sin Cuenta):** Solo acceso al CTA de "Evaluación Manual" (Ephemeral). *Topic Mastery Metrics* e *Historial* ocultos o con estado de "Upsell" (Blur/Candado). No se permite el modo Práctica.
- **Freemium (Cuenta Gratuita):** Acceso a "Evaluación Manual" (sujeto a cuota diaria). Visibilidad total de *Topic Mastery Metrics* y acceso al *Historial* de evaluaciones. El botón de "Práctica Espaciada (SRS)" se muestra bloqueado (Upsell a Premium).
- **Premium:** Todo desbloqueado. Acceso ilimitado al generador de "Práctica Espaciada (SRS)".
- **Reusabilidad Estricta (Single Component):** Los detalles de cualquier evaluación (ya sea un resultado en vivo, consultando el historial, o al hacer clic en un error dentro del Topic Mastery) **DEBEN** renderizarse usando exactamente el mismo componente UI compartido (`EvaluationResultCard`).

## 7. Componentes Clave y Comportamiento (Interaction Map)
Para mantener la cohesión visual tipo "Herramienta Industrial" orientada al móvil, todos los nuevos componentes deben seguir este mapa:
- **Navegación (Mobile-First & Minimalismo):**
  - *Dispositivos Móviles (`< sm`):* Top bar ultra minimalista (solo logo) y un **Bottom Navigation Bar** fijo para acciones principales.
  - *Escritorio (`>= sm`):* Top bar flotante extremadamente limpio (Logo a la izquierda, Avatar/Theme a la derecha). La navegación secundaria (History, Profile) debe ir oculta en un Avatar Dropdown, no expuesta para evitar sobrecarga cognitiva.
- **Acciones y Shortcuts (Bento Grid):** No usar botones genéricos sueltos. Usar **Action Cards** con íconos de alto contraste y subtextos descriptivos.
- **Visualización de Datos (Data Density):** Prohibidas las tarjetas gigantes para listas. Usar layouts estilo tabla/lista compacta para métricas.
- **Accesibilidad y Contraste (Light Mode Strict Rule):** 
  - Todo texto secundario debe ser como mínimo `text-neutral-600` en Light Mode para pasar ratios WCAG. (Ej: `text-neutral-600 dark:text-neutral-400`).
