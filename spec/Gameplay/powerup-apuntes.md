---
title: Potenciador - Apuntes
assignee: johny
---

## Historia de Usuario

Como jugador, quiero recoger el potenciador de Apuntes para obtener un escudo protector que bloquee un golpe enemigo, dándome una segunda oportunidad de supervivencia contra ataques sorpresivos.

## Descripción

Potenciador defensivo que otorga un escudo temporal al jugador. El escudo bloquea exactamente 1 golpe de cualquier fuente de daño y luego se rompe con un efecto visual satisfactorio.

## Elementos Requeridos

### Visual del Potenciador

- **Sprite:** Icono de libreta/apuntes o documento enrollado
- **Animación:** Flotante, rotation suave, partículas sutiles
- **Color:** Blanco/papel con detalles azules o amarillos
- **Hitbox:** Circular, radio de collisión ~0.5 unidades

### Efecto de Escudo

- **Protección:** Bloquea 1 hit de cualquier daño
- **Tipos de daño bloqueado:**
  - Ataques cuerpo a cuerpo de enemigos
  - Proyectiles enemigos
  - Daño ambiental (trampas, spikes)
- **No bloquea:** Daño de caída (fall damage)

### Visual del Escudo Activo

- **Apariencia:** Burbuja o aura alrededor del jugador
- **Color:** Azul/cían translúcido o efecto de notas flotantes
- **Animación:** Rotación o pulso suave
- **Posición:** Centrado en el jugador

### Romperse Visualmente

- **Trigger:** El escudo recibe daño por primera vez
- **Efecto de ruptura:**
  - Partículas de papel volando
  - Sonido de papel rasgándose
  - Flash de impacto
  - Onda de choque visual
- **Desaparición:** El escudo se desvanece con efecto碎玻璃 (glass shatter)

### Indicador UI

- **Icono:** Libreta/apuntes en HUD
- **Estado:** Cuando está activo, mostrar claramente
- **Feedback:** opacity alta cuando activo, baja cuando consumido

### Sonido

- **Recoger:** Sonido de papel/pickup
- **Bloquear:** Sonido de defensa/impacto sordo
- **Romperse:** Sonido de rasgado de papel +碎裂

## Comportamiento de Recolección

- **Trigger:** Collider del jugador toca el potenciador
- **Consumición:** Automática al contacto
- **Desaparición:** El potenciador desaparece del nivel
- **Stack:** Permite solo 1 escudo activo (no stackeable)

## Notas de Diseño

- Potenciador defensivo para situaciones riesgosas
- Feedback visual muy importante para que jugador sepa que bloqueó
- Considerar: ¿El escudo se rompe instantáneamente o tiene frames de invencibilidad?
- Balance: 1 golpe bloqueado = muy fuerte, considerar cooldown

## Requisitos Técnulos

- Frame rate objetivo: 60 FPS
- Hitbox del escudo: CircleCollider ligeramente más grande que el jugador
- Particle system para ruptura: Pre-instanciado para performance
- Communication con PlayerController para estado de escudo
- Layer collision: Escudo ignore player layer
