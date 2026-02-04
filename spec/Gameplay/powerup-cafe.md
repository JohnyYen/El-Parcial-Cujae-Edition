---
title: Potenciador - Café
assignee: @gameplay-team
---

## Historia de Usuario

Como jugador, quiero recoger el potenciador de Café para aumentar temporalmente mi velocidad de movimiento y cadencia de disparo, permitiéndome eliminar enemigos más rápido en situaciones de emergencia.

## Descripción

Potenciador consumible que otorga beneficios ofensivos temporales al jugador. Aparece como una taza de café o similar y otorga +Velocidad de movimiento y +Cadencia de disparo durante 5 segundos.

## Elementos Requeridos

### Visual del Potenciador

- **Sprite:** Taza de café con vapor rising
- **Animación:** Flotante, rotation suave, partículas de vapor
- **Color:** Marrón cálido, destacar del fondo
- **Hitbox:** Circular, radio de collisión ~0.5 unidades

### Efectos al Recoger

- **Velocidad:** +30% velocidad base durante 5 segundos
- **Cadencia de disparo:** +25% velocidad de ataque durante 5 segundos
- **Indicador visual:** Jugador con efecto de "speed lines" o aura dorada
- **Indicador UI:** Icono de café en HUD con timer decreciente

### Timer de Duración

- **Duración:** 5 segundos
- **Visual:** Barra circular o número decreciente sobre el icono en HUD
- **Comportamiento:** El timer se muestra mientras el efecto está activo
- **Stacking:** No stackeable (renueva duración si se recoge otro)

### Sonido

- **Recoger:** Sonido de pickup satisfactorio
- **Activar:** Sonido de "power up" al activarse
- **Expirar:** Sonido suave de expiración

## Comportamiento de Recolección

- **Trigger:** Collider del jugador toca el potenciador
- **Consumición:** Automática al contacto
- **Desaparición:** El potenciador desaparece del nivel al ser recogido
- **Respawn:** Opcional - puede reaparecer después de cierto tiempo

## Efectos de Gameplay

### Velocidad
- Modificador aplicado: 1.3x velocidad base
- Afecta: Movement speed del PlayerController
- Visual: Trail renderer o speed lines

### Cadencia de Disparo
- Modificador aplicado: 1.25x velocidad de ataque
- Afecta: Fire rate del weapon system
- Visual: Efecto de partículas al disparar

## Notas de Diseño

- Ideal para situaciones de presión alta
- Buen balance: Efectivo pero de corta duración
- Posible cooldown entre pickups del mismo tipo
- Considerar enemigo que suelte este potenciador

## Requisitos Técnicos

- Frame rate objetivo: 60 FPS
- Duración precisa: 5 segundos +/- 0.1s
- Hitbox precisa para pickup
- Particle effects optimizados
- Communication con GameManager para buffs activos
