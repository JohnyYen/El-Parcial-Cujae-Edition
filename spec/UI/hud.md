---
title: HUD (Heads-Up Display)
assignee: @ui-team
---

## Historia de Usuario

Como jugador, quiero ver en tiempo real mi estado de vida, enfoque y tener acceso rápido a opciones para tomar decisiones estratégicas durante el combate.

## Descripción

Interfaz persistente que muestra información crítica del jugador durante todo el gameplay. Debe ser visible pero no intrusiva, permitiendo enfoque en la acción mientras mantiene al jugador informado.

## Elementos Requeridos

### Barra de Vida (Estrés)

- **Tipo:** Progress Bar / Health Bar
- **Posición:** Esquina superior izquierda o inferior-center
- **Comportamiento:**
  - Llena al inicio (100%)
  - Decrease cuando el jugador recibe ataques
  - Empty = pérdida de vida/continuar
- **Umbral crítico:** < 20% → Animación de parpadeo / color rojo
- **Efecto acumulado:** No se regenera naturalmente, requiere curas

#### Mecánica de Estrés

- El estrés aumenta con cada ataque recibido
- Si la barra de estrés se llena completamente: Game Over o pérdida de vida
- **Decision pendiente:** El estrés es vida o un medidor separado?

### Barra de Enfoque

- **Tipo:** Progress Bar / Resource Bar
- **Posición:** Adyacente a la barra de vida o debajo
- **Comportamiento:**
  - Comienza vacía o parcialmente llena
  - Increase cuando el jugador:
    - Esquiva ataques exitosamente
    - Derrota enemigos
    - Completa objetivos
  - Se consume para:
    - Super ataques
    - Habilidades especiales
    - Defensas perfectas
- **Regeneración:** No se regenera pasivamente

#### Usos del Enfoque

- Super ataques: Costo X de enfoque para daño aumentado
- Habilidades especiales: Costo variable

### Botón Menú Opciones

- **Tipo:** Button / Icon
- **Posición:** Esquina superior derecha o inferior-derecha
- **Comportamiento:** Abre el menú de opciones durante el juego
- **Icono:** Engranaje o tres líneas (hamburger menu)
- **Keyboard shortcut:** Escape, P, Start
- **Tooltip:** "Opciones" al hacer hover

## Comportamiento General

### Visibilidad

- Siempre visible durante gameplay
- Semi-transparente para no bloquear vista
- Ocultar durante cinemáticas o diálogos

### Animaciones

- Transiciones suaves en cambios de valor
- Feedback visual en damage taken (flash rojo)
- Efecto visual al llenar barra de enfoque

### Responsive

- Adaptarse a diferentes resoluciones de pantalla
- Mantener proporción en aspect ratios extremos

## Requisitos Técnicos

- Frame rate objetivo: 60 FPS (UI debe actualizarse smoothness)
- Comunicación con PlayerController para valores en tiempo real
- Optimización para mínima impacto en rendimiento
- Canvas render mode: Screen Space - Overlay
- Resolución de referencia: 1920x1080
