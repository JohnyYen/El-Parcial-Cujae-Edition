---
title: Minion - Niveles y Comportamiento
assignee: carlos
---

## Historia de Usuario

Como jugador, quiero enfrentar diferentes tipos de minions con comportamientos variados para tener opciones tácticas durante el combate contra el boss.

## Descripción

Los Minions son enemigos secundarios spawnados por el boss. Tienen 3 niveles de dificultad con stats, movimientos y ataques diferenciados. Todos implementan la interfaz IMinion para estandarizar su comportamiento.

## Elementos Requeridos

### Sistema de Niveles

#### Minion Nivel 1 (Fácil)

- **Vida:** 30-50 HP
- **Daño:** 5-10 por ataque
- **Velocidad:** Lenta (0.5x - 0.7x)
- **Comportamiento:** Follow básico, ataque cuerpo a cuerpo simple
- **Tamaño:** Pequeño
- **Puntos de experiencia:** 10

#### Minion Nivel 2 (Medio)

- **Vida:** 60-100 HP
- **Daño:** 15-25 por ataque
- **Velocidad:** Normal (0.8x - 1.0x)
- **Comportamiento:** Combinación de seguimiento y ataques evasivos
- **Tamaño:** Mediano
- **Puntos de experiencia:** 25

#### Minion Nivel 3 (Difícil)

- **Vida:** 120-180 HP
- **Daño:** 30-45 por ataque
- **Velocidad:** Rápida (1.2x - 1.5x)
- **Comportamiento:** Agresivo, ataques rápidos, posible resistencia
- **Tamaño:** Grande
- **Puntos de experiencia:** 50

### Comportamiento por Nivel

#### Movimiento

```csharp
void IMinion.Mover(Vector2 direccion) {
  float velocidad = ObtenerVelocidadPorNivel();
  
  // Facil: Movimiento lineal directo
  if (tipo == TipoMinion.Facil) {
    transform.position += (Vector3)direccion * velocidad * Time.deltaTime;
  }
  
  // Medio: Movimiento con slight evasion
  if (tipo == TipoMinion.Medio) {
    Vector2 direccionEvasiva = CalcularEvasion(direccion);
    transform.position += (Vector3)direccionEvasiva * velocidad * Time.deltaTime;
  }
  
  // Dificil: Movimiento predictivo
  if (tipo == TipoMinion.Dificil) {
    Vector2 posicionPredicha = PredecirPosicionPlayer(direccion);
    transform.position += (Vector3)(posicionPredicha - transform.position).normalized * velocidad * Time.deltaTime;
  }
}
```

#### Ataque

```csharp
void IMinion.Atacar(ITargetable objetivo) {
  if (EstaEnRango(objetivo)) {
    // Facil: Ataque simple
    if (tipo == TipoMinion.Facil) {
      objetivo.RecibirDamage(ObtenerDamage());
    }
    
    // Medio: Ataque con combo corto
    if (tipo == TipoMinion.Medio) {
      EjecutarCombo(objetivo, 2 golpes);
    }
    
    // Dificil: Ataque con posible critico
    if (tipo == TipoMinion.Dificil) {
      bool esCritico = Random.value < 0.3f; // 30% chance
      float dano = esCritico ? ObtenerDamage() * 1.5f : ObtenerDamage();
      objetivo.RecibirDamage(dano);
      if (esCritico) MostrarFeedbackCritico();
    }
  }
}
```

### Vida y Muerte

```csharp
int IMinion.ObtenerVida() { return vidaActual; }
int IMinion.ObtenerVidaMaxima() { return vidaMaxima; }

void IMinion.RecibirDamage(float cantidad) {
  vidaActual -= cantidad;
  MostrarFeedbackDano(cantidad);
  
  if (tipo == TipoMinion.Dificil && vidaActual < vidaMaxima * 0.3f) {
    ActivarModoDesesperacion();
  }
  
  if (vidaActual <= 0) {
    Morir();
  }
}

void IMinion.Morir() {
  // Animation de muerte
  PlayAnimation("Death");
  
  // Notificar al sistema
  MinionManager.NotificarMuerte(this);
  
  // Drop de recursos (si aplica)
  SpawnDrop();
  
  // Destroy después de animación
  Destroy(gameObject, tiempoAnimacionMuerte);
}
```

## Comportamiento General

### Máquina de Estados del Minion

```
SPAWN → IDLE → (Player detectado) → PERSECUCION → ATAQUE
        → (Daño recibido) → HIT → PERSECUCION
        → (Vida <= 0) → MORTE
        
PERSECUCION → (Distancia < rango) → ATAQUE
PERSECUCION → (Player muy lejos) → IDLE
```

### IA de Movimiento

| Nivel | Estrategia | Agresividad |
|-------|------------|-------------|
| Facil | Follow directo | Baja |
| Medio | Follow con zigzag | Media |
| Dificil | Predictivo + flank | Alta |

### Interacción con Player

- **Daño al player:** Resta vida del player
- **Hit feedback:** Flash blanco en el minion
- **Death feedback:** Partículas + sonido
- **Aggro:** El minion más cercano al player es el target

## Requisitos Técnicos

- **Clases base:** `MinionBase.cs`, `MinionFacil.cs`, `MinionMedio.cs`, `MinionDificil.cs`
- **Implementa:** `IMinion`
- **Prefab:** Un prefab base con variantes por nivel
- **Animator:** Controller compartido con parámetros por nivel
- **Frame rate:** 60 FPS
- **Pooling:** Object pooling para spawn/despawn rápido
- **Audio:** SFX para spawn, ataque, hit, muerte (variantes por nivel)
- **Partículas:** Efectos diferenciados por nivel
- **Collider:** BoxCollider2D o CircleCollider2D según tamaño
- **Rigidbody:** Kinematic para movement controlado, Dynamic para physics
