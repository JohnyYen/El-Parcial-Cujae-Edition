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

#### Minion Tipo 1 - Basic (Fácil)

- **Vida:** 50 HP
- **Daño:** 10 por ataque
- **Velocidad:** 2.0 unidades/seg (lenta)
- **Rango de ataque:** 2.0 unidades
- **Rango de detección:** 8.0 unidades
- **Comportamiento:** Follow directo, ataque cuerpo a cuerpo simple
- **Enfoque al morir:** 10 puntos
- **Usado por Boss:** ✅ Sí (Fase 1 y 2)

#### Minion Tipo 2 - Medium (Medio)

- **Vida:** 70 HP
- **Daño:** 20 por ataque
- **Velocidad:** 3.5 unidades/seg (rápida)
- **Rango de ataque:** 2.0 unidades
- **Rango de detección:** 8.0 unidades
- **Comportamiento:** Agresivo, persecución rápida
- **Enfoque al morir:** 20 puntos
- **Usado por Boss:** ✅ Sí (Fase 2 y 3)

#### Minion Tipo 3 - Hard (Difícil)

- **Vida:** 120 HP
- **Daño:** 30 por ataque
- **Velocidad:** 1.8 unidades/seg (lenta, pero resistente)
- **Rango de ataque:** 2.0 unidades
- **Rango de detección:** 8.0 unidades
- **Comportamiento:** Tanque con 25% de reducción de daño
- **Enfoque al morir:** 35 puntos
- **Usado por Boss:** ❌ No (reservado para uso especial)

### Comportamiento por Nivel

#### Movimiento

Todos los minions usan movimiento directo hacia el jugador:

```csharp
// En MinionBehaviour.cs
private void MoveTowardsPlayer()
{
    if (playerTransform == null) return;

    // Movimiento estándar hacia el jugador
    Vector2 direction = (playerTransform.position - transform.position).normalized;
    rb.linearVelocity = direction * minionData.MoveSpeed;
    
    // Flip sprite según dirección
    FlipSprite(direction.x);
}
```

La diferencia entre tipos está en la velocidad:
- **Basic:** Lento (2.0) - más fácil de esquivar
- **Medium:** Rápido (3.5) - presión constante
- **Hard:** Lento (1.8) pero resistente - difícil de eliminar

#### Ataque

```csharp
Todos los minions usan ataque de contacto:

```csharp
// En MinionBehaviour.cs - HandleAttack()
private void HandleAttack()
{
    if (attackPoint == null)
        attackPoint = transform;

    // Detectar jugador en rango de ataque
    Collider2D[] hits = Physics2D.OverlapCircleAll(
        attackPoint.position, 
        attackRadius, 
        playerLayer
    );
    
    foreach (Collider2D hit in hits)
    {
        Player player = hit.GetComponent<Player>();
        if (player != null && player.player_behaviour != null)
        {
            // Aplicar daño según el tipo de minion
            player.player_behaviour.AddStress(minionData.AttackDamage);
        }
    }
#### Sistema de Daño
// En MinionBehaviour.cs - HandleDeath()
private void HandleDeath()
{
    ChangeState(MinionState.Death);
    
    // Otorgar enfoque al jugador
    Player player = playerTransform?.GetComponent<Player>();
    if (player != null && player.player_behaviour != null)
    {
        player.player_behaviour.AddEnfoque(minionData.EnfoqueReward);
    }

    // Partículas de muerte
    if (deathParticles != null)
    {
        Instantiate(deathParticles, transform.position, Quaternion.identity);
    }

    // Destruir después de animación
    Destroy(gameObject, 1f
    // Aplica reducción de daño del 25%
    float reducedDamage = amount * (1f - damageReduction); // 0.75x
    currentHealth -= reducedDamage;
    InvokeOnMinionHit(reducedDamage);

    if (currentHealth <= 0)
    {
        currentHealth = 0;
        Die();
    }
}
```

#### Muerte

}
```

La diferencia entre tipos está en el daño:
- **Basic:** 10 de daño
- **Medium:** 20 de daño
- **Hard:** 30 de daño
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
  Tipo | Velocidad | Estrategia | Usado por Boss |
|------|-----------|------------|----------------|
| Basic | 2.0 (lenta) | Follow directo | ✅ Sí |
| Medium | 3.5 (rápida) | Follow agresivo | ✅ Sí |
| Hard | 1.8 (lenta) | Follow resistente (25% reducción daño) | ❌ No

void IMinion.Morir() {
  // Animation de muerte
  PlayAnimation("Death");
  
  // Notificar al sistema
  MinionManager.NotificarMuerte(this);
  
  // Drop de recursos (si aplica)
  SpawnDrop();
### Arquitectura Implementada

- **Interface:** `IMinion.cs`
- **Base abstracta:** `MinionSO.cs` (ScriptableObject)
- **Implementaciones:** `BasicMinion.cs`, `MediumMinion.cs`, `HardMinion.cs`
- **MonoBehaviour:** `MinionBehaviour.cs` (controla física y estados)
- **Enum:** `MinionType` (Basic, Medium, Hard)

### Configuración de Prefabs

- **Rigidbody2D:** Dynamic, Gravity Scale: 0, Freeze Rotation Z
- **Collider2D:** BoxCollider2D o CircleCollider2D
- **Layer:** Enemy
- **Tag:** No requerido

### Configuración Crítica

⚠️ **MinionBehaviour Inspector:**
- **Attack Radius:** 1.0-1.5 (NO usar 0.5, es muy pequeño)
- **Player Layer:** Debe coincidir con el layer del jugador

### Opcionales

- **Animator:** Controller con parámetros IsMoving, IsAttacking, IsDead
- **Audio:** SFX para spawn, ataque, hit, muerte
- **Partículas:** Efectos de muerte y hit
- **Pooling:** MinionSpawner incluye object pooling (opcional)

## Integración con Boss

Ver [boss-minion-integration.md](boss-minion-integration.md) para detalles completos de implementación.
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
