---
title: Player - Movimiento y Ataques
assignee: @gameplay-team
---

## Historia de Usuario

Como jugador, quiero controlar un personaje con movimientos fluidos y ataques diferenciados para enfrentar a los enemigos del boss de manera estratégica.

## Descripción

El Player es la entidad controlada por el usuario. Debe responder a los inputs de teclado/gamepad para ejecutar movimientos, saltos, dashes y ataques. El comportamiento puede variar según el tipo de player seleccionado.

## Elementos Requeridos

### Sistema de Movimiento

#### Movimiento Lateral

- **Tipo:** Input continuo
- **Ejes:** Horizontal (eje X)
- **Comportamiento:**
  - Input positivo → Movimiento a la derecha
  - Input negativo → Movimiento a la izquierda
  - Sin input → Detención gradual (friction)
- **Velocidad:** Variable configurable
- **States:** Idle, Walk, Run

#### Salto

- **Tipo:** Input discreto (trigger)
- **Input:** Espacio / Botón A de gamepad
- **Comportamiento:**
  - Ejecutable solo cuando está en el suelo
  - Impulso vertical inmediato
  - Gravedad aplicada durante caída
- **Configurable:** Altura de salto, velocidad de ascenso/descenso
- **Doble salto:** No implementado (pendiente si se requiere)

### Sistema de Dash

- **Tipo:** Input discreto con cooldown
- **Input:** Shift / Click derecho / Botón X de gamepad
- **Comportamiento:**
  - Desplazamiento rápido en dirección del movimiento o input
  - Invulnerabilidad durante la ejecución
  - Duración: 0.2 - 0.5 segundos
  - Cooldown: 2 - 3 segundos
- **Velocidad:** 2x - 3x la velocidad normal
- **Consumo:** No consume recursos (enfoque/stamina)

### Sistema de Ataques

#### Ataque Suave (Light Attack)

- **Tipo:** Input discreto
- **Input:** C / Click izquierdo / Botón Y de gamepad
- **Comportamiento:**
  - Daño bajo pero ejecución rápida
  - Combo posible (ataques encadenados)
  - Rango corto
  - Sin cooldown significativo
- **Stats:** Daño: 10-15, Duración: 0.3s, Recovery: 0.1s
- **Animation:** Quick jab / golpe rápido

#### Ataque Fuerte (Heavy Attack)

- **Tipo:** Input discreto
- **Input:** V / Click derecho / Botón B de gamepad
- **Comportamiento:**
  - Daño alto pero ejecución lenta
  -recovery largo si falla
  - Puede romper escudos de enemigos
  - Stunea brevemente al enemigo
- **Stats:** Daño: 25-40, Duración: 0.6s, Recovery: 0.4s
- **Animation:** Golpe cargado / swing potente

### Recursos del Player

#### Vida (Health)

- **Valor inicial:** 100%
- **Daño recibido:** Resta vida según el ataque del enemigo
- **Morte:** 0% vida = Game Over / Continuar
- **Regeneración:** No se regenera naturalmente
- **Curación:** Solo mediante items o habilidades específicas

#### Enfoque (Focus)

- **Valor inicial:** 0%
- **Ganancia:** Esquivas exitosas, derrotas de enemigos, objetivos completados
- **Consumo:** Ataques especiales, habilidades
- **Capacidad máxima:** 100%
- **Sin regeneración pasiva**

## Comportamiento General

### Inputs

| Acción | Teclado | Gamepad | Mouse |
|--------|---------|---------|-------|
| Mover Izq/Der | A/D | Left Stick | - |
| Saltar | Espacio | Button A | - |
| Dash | Shift | Button X | Right Click |
| Ataque Suave | C | Button Y | Left Click |
| Ataque Fuerte | V | Button B | - |

### Estados del Player

```
IDLE → Movimiento → WALK/RUN
     → Salto → AIRBORNE → (Suelo) → IDLE
     → Dash → DASHING → (Fin) → IDLE
     → Ataque → ATTACKING → IDLE/COMBO
     → Damage → HIT → IDLE
     → Muerte → DEAD → GAME OVER
```

### Física

- **Rigidbody:** Componente principal para física
- **Capas de colisión:** Player, Suelo, Enemigos, Proyectiles
- **Layer masks:** Configurables para optimizar detecciones
- **Gravity scale:** 1.0 (o configurable)

### Animaciones (Referencia)

- Idle (loop)
- Walk / Run
- Jump (start, loop, land)
- Dash (start, loop, end)
- Light Attack 1, 2, 3
- Heavy Attack (charge, release)
- Hit reaction
- Death

## Requisitos Técnicos

- **Componente principal:** `PlayerController.cs`
- **Input system:** Nuevo Input System de Unity
- **Physics:** Unity Physics 2D o Box2D
- **Animations:** Animator con Blend Trees
- **Frame rate objetivo:** 60 FPS
- **Resolution:** 1920x1080 base
- **Input buffering:** 0.1-0.2 segundos para combos
- **Hitboxes:** BoxCollider2D para cuerpo, PolygonCollider2D para ataques
- **Audio:** SFX para pasos, saltos, ataques, hits, dash
- **Partículas:** Trail en dash, impacto en ataques, feedback visual en daño
