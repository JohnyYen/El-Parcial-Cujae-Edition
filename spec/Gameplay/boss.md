---
title: Boss - Fases, Ataques y Spawning
assignee: @gameplay-team
---

## Historia de Usuario

Como jugador, quiero enfrentar un boss desafiante con múltiples fases y patrones de ataque variados que requiera habilidad y estrategia para derrotar.

## Descripción

El Boss es el enemigo principal del nivel. Tiene 3 fases con comportamientos distintos, puede spawnear minions para apoyar sus ataques, y cambia su estrategia según le quede menos vida. Implementa IBoss para estandarizar su comportamiento.

## Elementos Requeridos

### Sistema de Fases

#### Fase 1 (100% - 66% vida)

- **Comportamiento:** Intro + ataques básicos
- **Patrones de ataque:** 2-3 tipos básicos
- **Velocidad:** Normal (1.0x)
- **Spawning:** Minions nivel fácil (Facil)
- **Frecuencia ataques:** Media (cada 2-3 segundos)
- **Mensajes:** Taunts iniciales

#### Fase 2 (66% - 33% vida)

- **Comportamiento:** Aumenta agresividad
- **Patrones de ataque:** 4-5 tipos (incluye ataques en área)
- **Velocidad:** Aumentada (1.25x)
- **Spawning:** Minions nivel medio (Medio)
- **Frecuencia ataques:** Alta (cada 1.5-2 segundos)
- **Mensajes:** Frustración / ira

#### Fase 3 (33% - 0% vida)

- **Comportamiento:** Modo desesperación - ataques máximos
- **Patrones de ataque:** 6+ tipos (incluye super ataques)
- **Velocidad:** Máxima (1.5x)
- **Spawning:** Minions nivel difícil (Dificil) + combinaciones
- **Frecuencia ataques:** Muy alta (cada 1 segundo)
- **Mensajes:** Desesperación / peligro

### Sistema de Cambios de Fase

```csharp
void IBoss.CambiarFase(FaseBoss nuevaFase) {
  // Animación de transición
  PlayAnimation("PhaseTransition");
  
  // Efecto visual (partículas, flash, etc.)
  TriggerPhaseEffect();
  
  // Cambiar stats del boss
  AjustarVelocidad(nuevaFase);
  AjustarFrecuenciaAtaques(nuevaFase);
  
  // Mensaje de diálogo
  MostrarMensajeBoss(ObtenerMensajeFase(nuevaFase));
  
  // Spawn de minions de transición
  if (nuevaFase == FaseBoss.Fase2) {
    SpawnOleadaInicial(TipoMinion.Medio, 2);
  } else if (nuevaFase == FaseBoss.Fase3) {
    SpawnOleadaInicial(TipoMinion.Dificil, 3);
  }
  
  // Notificar a sistemas externos
  NotifyPhaseChanged(nuevaFase);
}
```

### Sistema de Ataques por Fase

#### Fase 1 - Ataques Disponibles

| Ataque | Daño | Rango | Cooldown | Descripción |
|--------|------|-------|----------|-------------|
| Golpe Basic | 10-15 | Cuerpo a cuerpo | 2s | Ataque simple |
| Embestida | 15-20 | Medio | 3s | Charge hacia adelante |
| Combo Doble | 20-25 | Cuerpo a cuerpo | 4s | Dos golpes seguidos |

#### Fase 2 - Ataques Disponibles

| Ataque | Daño | Rango | Cooldown | Descripción |
|--------|------|-------|----------|-------------|
| Golpe Basic+ | 15-20 | Cuerpo a cuerpo | 1.5s | Versión mejorada |
| Embestida | 20-25 | Medio | 2.5s | Más rápida |
| Onda de Choque | 25-30 | Largo | 5s | Proyectil en arco |
| Ataque Giratorio | 30-35 | Area circular | 6s | Spin attack 360° |

#### Fase 3 - Ataques Disponibles

| Ataque | Daño | Rango | Cooldown | Descripción |
|--------|------|-------|----------|-------------|
| Furia Total | 35-45 | Todo el escenario | 8s | Ataque masivo |
| LLuvia de Proyectiles | 10-15 c/u | Largo | 7s | Múltiples proyectiles |
| Embestida Furiosa | 40-50 | Largo | 4s | Charge muy rápido |
| Super Combo | 50-60 | Cuerpo a cuerpo | 10s | Combo final |
| Desesperación | 45-55 | Area grande | 12s | Ultimo recurso |

### Sistema de Spawning de Enemigos

```csharp
void IBoss.SpawnEnemigo(TipoMinion tipo) {
  // Obtener posición de spawn disponible
  Vector2[] spawnPoints = ObtenerPosicionSpawn();
  Vector2 posicion = spawnPoints[Random.Range(0, spawnPoints.Length)];
  
  // Crear minion según tipo
  IMinion nuevoMinion = MinionFactory.Crear(tipo, posicion);
  
  // Registrar en sistema de minions
  MinionManager.Registrar(nuevoMinion);
  
  // Feedback visual
  SpawnEffect(posicion);
}
```

#### Pool de Spawn

- **Posiciones fijas:** 4-6 puntos alrededor del boss
- **Spawning aleatorio:** Elige posición al azar
- **Spawning estratégico:** Elige posición alejada del player
- **Límite:** Máximo 5-8 minions activos simultáneamente

### Sistema de Mensajes del Boss

#### Fase 1 - Mensajes de Intro

- "¡No podrás vencerme!"
- "Esto es solo el comienzo"
- "¡Prepárate para sufrir!"

#### Fase 2 - Transiciones

- "¡Esto apenas empieza!"
- "¡Ahora verás mi verdadero poder!"
- "¡No me vas a derrotar tan fácilmente!"

#### Fase 3 - Desesperación

- "¡Imposible! ¡¿Cómo puedes ser tan fuerte?!"
- "¡No... no puede ser!"
- "¡Juntos caeremos!"

## Comportamiento General

### Máquina de Estados del Boss

```
IDLE → (Timer) → ElegirAtaque → EJECUTAR_ATAQUE → IDLE
       → (Player cerca) → ATAQUE_CERCANO
       → (Health < 66%) → TRANSICION_FASE_2
       → (Health < 33%) → TRANSICION_FASE_3
       → (Health <= 0%) → MORIR
       
TRANSICION_FASE → Animación → CambiarFase() → IDLE (nueva fase)
```

### Lógica de IA

- **Decision tree:** Basado en distancia al player, vida propia, fase actual
- **Patrones de ataque:** Seleccionados aleatoriamente pero ponderados por fase
- **Spawning:** Intervalos fijos + triggers de fase
- **Aggro:** Siempre enfocado en el player

### Hitboxes del Boss

- **Cuerpo principal:** BoxCollider2D para colisiones físicas
- **Hitbox ataques:** PolygonCollider2D por cada tipo de ataque
- **Invulnerabilidad:** Durante animaciones de transición de fase

## Requisitos Técnicos

- **Componente principal:** `BossController.cs`
- **Implementa:** `IBoss`, `IAtaqueBoss`, `ISpawnEnemigo`
- **Animator:** State machine con sub-states por fase
- **Frame rate:** 60 FPS
- **Assets:** Sprites para cada fase (cambio visual), efectos de partículas
- **Audio:** Voces para mensajes, SFX para cada ataque
- **Pooling:** Object pooling para minions spawned
- **SerializeField:** Stats configurables por inspector
