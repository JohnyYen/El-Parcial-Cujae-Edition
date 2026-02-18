# Sistema de Minions - Guía de Implementación

## Descripción General

El sistema de minions está implementado siguiendo el patrón de arquitectura del proyecto, utilizando ScriptableObjects para definir datos y comportamiento, separados de los componentes MonoBehaviour que manejan la lógica física en Unity.

## 🎯 Dos Modos de Uso

### Modo 1: Básico (Compatible con Boss actual) ✅ RECOMENDADO PARA MVP
El Boss usa `Instantiate(minionPrefab, ...)` directamente. Los minions son **autocontenidos** y funcionan automáticamente al ser instanciados.

**Uso:** Boss fight básico con un tipo de minion.

### Modo 2: Avanzado (Con MinionSpawner) 🔧 OPCIONAL
Usar el componente `MinionSpawner` para gestión avanzada con múltiples tipos, object pooling y control de cantidad.

**Uso:** Escenas complejas, múltiples enemigos, optimización avanzada.

> **Nota:** El Boss actual usa **Modo 1**. MinionSpawner está implementado pero es opcional.

---

## Arquitectura del Sistema

### 1. Estructura de Archivos

```
Assets/Scripts/Gameplay/
├── Interfaces/
│   └── IMinion.cs                    # Interfaz base para minions
├── ScriptableObject/
│   ├── Interfaces/
│   │   └── MinionSO.cs               # Clase abstracta base (ScriptableObject)
│   ├── BasicMinion.cs                # Implementación del minion básico
│   ├── FastMinion.cs                 # Implementación del minion rápido
│   ├── TankMinion.cs                 # Implementación del minion tanque
│   └── RangedMinion.cs               # Implementación del minion a distancia
├── MinionBehaviour.cs                # Componente MonoBehaviour
├── MinionProjectile.cs               # Proyectil para minions a distancia
└── MinionSpawner.cs                  # Sistema de spawn con object pooling
```

### 2. Componentes del Sistema

#### IMinion (Interfaz)
Define el contrato que todos los minions deben cumplir:
- Propiedades: Type, Health, MaxHealth, IsAlive, EnfoqueReward
- Métodos: TakeDamage, Attack, MoveTowardsPlayer, Patrol, Die, Initialize
- Eventos: OnMinionHit, OnMinionDeath, OnMinionAttack, OnMinionSpawned

#### MinionSO (ScriptableObject Abstracto)
Implementa IMinion y define:
- Stats configurables (vida, velocidad, daño, rangos)
- Lógica base de daño y muerte
- Métodos protegidos para invocar eventos desde clases hijas
- Estado interno del minion

#### Implementaciones Concretas
Tres tipos de minions, cada uno con características únicas:

**BasicMinion:**
- Vida: 50 HP
- Velocidad: 2.0 (lenta)
- Comportamiento: Movimiento lineal directo
- Enfoque reward: 10
- **Usado por Boss:** Fase 1 y 2

**MediumMinion:**
- Vida: 70 HP
- Velocidad: 3.5 (media)
- Comportamiento: Equilibrado, más agresivo
- Enfoque reward: 20
- **Usado por Boss:** Fase 2 y 3

**HardMinion:**
- Vida: 120 HP
- Velocidad: 1.8 (lenta)
- Comportamiento: Resistente (25% reducción de daño)
- Enfoque reward: 35
- **Usado por Boss:** No (reservado para uso especial)

## Configuración en Unity

### 1. Crear Assets de Minion

1. En Unity, click derecho en la carpeta `Assets/Scripts/Gameplay/ScriptableObject/Objects`
2. Seleccionar `Create > Minions > [Tipo de Minion]`
3. Nombrar el asset (ej: "Basic Minion 01")
4. Configurar stats en el Inspector si es necesario

### 2. Crear Prefabs de Minion

1. Crear un GameObject vacío en la escena
2. Agregar componente `MinionBehaviour`
3. Agregar componente `Rigidbody2D`:
   - Body Type: Dynamic
   - Gravity Scale: 0 (para movimiento 2D top-down) o 1 (para platformer)
   - Constraints: Freeze Rotation Z
4. Agregar componente `Collider2D` (Box o Circle según el sprite)
5. Agregar `SpriteRenderer` con el sprite del minion
6. (Opcional) Agregar `Animator` con animaciones del minion
7. Asignar el MinionSO correspondiente en el Inspector
8. Asignar layer "Enemy" al GameObject
9. Guardar como prefab en `Assets/Prefabs/Minions/`

### 3. Configurar MinionBehaviour

En el Inspector del prefab:

```
MinionBehaviour:
├── Minion Data: [Drag & drop el MinionSO - BasicMinion o MediumMinion]
├── Player Transform: [Auto-asignado en tiempo de ejecución]
├── Sprite Renderer: [Auto-asignado si está en el mismo GameObject]
├── Hit Color: Red
├── Hit Flash Duration: 0.1
├── Attack Point: [Transform hijo para posición de ataque]
├── Attack Radius: 1.0-1.5 ⚠️ AJUSTAR según escala del juego
├── Player Layer: Player ⚠️ CRÍTICO - debe coincidir con layer del jugador
├── Death Particles: [Prefab de partículas opcional]
└── Hit Particles: [Prefab de partículas opcional]
```

> **⚠️ IMPORTANTE:**
> - **Attack Radius:** El valor de 0.5 es muy pequeño para la mayoría de juegos. Empieza con 1.0-1.5 y ajusta según necesites.
> - **Player Layer:** Debe estar configurado correctamente o los ataques no funcionarán. Selecciona el layer donde está tu GameObject del jugador.

> **Nota:** Boss solo usa Basic y Medium. Hard es opcional para otros contextos.

---

## 🎮 Uso en Boss Fight (Modo Básico)

El Boss actual usa `Instantiate()` directo. Configuración simple:

1. **Crear prefab** con MinionBehaviour + MinionSO
2. **Asignar al Boss** en el campo `Minion Prefab`
3. **Configurar spawn points** (hijos de un Transform contenedor)
4. **Listo** - Los minions funcionan automáticamente

```csharp
// Lo que hace el Boss internamente:
Instantiate(minionPrefab, spawnPoint.position, Quaternion.identity);

// El minion se inicializa automáticamente en Start()
```

Ver [boss-minion-integration.md](boss-minion-integration.md) para más detalles.

---

## 🔧 MinionSpawner (Avanzado - OPCIONAL)

El componente `MinionSpawner` proporciona funcionalidades avanzadas:
- Spawn de múltiples tipos de minions
- Object pooling para optimización
- Control de cantidad máxima
- Spawn points estratégicos
- Oleadas configurables
Uso Básico (Sin MinionSpawner)

El Boss usa instanciación directa:

```csharp
// En Boss.cs (ya implementado)
Instantiate(minionPrefab, spawnPos, Quaternion.identity);
```

Los minions funcionan automáticamente - no requiere código adicional.

### Uso Avanzado (Con MinionSpawner - OPCIONAL)

Si decides usar MinionSpawner para gestión avanzada:

#### 
**Nota:** El Boss NO requiere MinionSpawner para funcionar. Esta sección es para uso avanzado opcional.

### 4. Configurar MinionSpawner (Si decides usarlo)

1. Crear un GameObject vacío llamado "MinionSpawner"
2. Agregar componente `MinionSpawner`
3. Configurar en el Inspector:

```
MinionSpawner:
├── Minion Prefabs:
│   ├── Basic Minion Prefab
│   ├── Fast Minion Prefab
│   ├── Tank Minion Prefab
│   └── Ranged Minion Prefab
├── Spawn Points: [Array de Transforms donde pueden aparecer minions]
├── Spawn Delay: 2
├── Max Active Minions: 5
├── Player Transform: [Auto-asignado]
├── Use Object Pooling: ☑
└── Pool Size Per Type: 10
```

### 5. Crear Spawn Points

1. Crear GameObjects vacíos en posiciones estratégicas
2. Nombrarlos "SpawnPoint_01", "SpawnPoint_02", etc.
3. Arrastrarlos al array "Spawn Points" del MinionSpawner
4. Visualización: Los spawn points se muestran como esferas verdes en Scene view

## Uso del Sistema

### Spawning Básico

```csharp
// Obtener referencia al spawner
MinionSpawner spawner = GetComponent<MinionSpawner>();

// Spawnear un minion de tipo específico
spawner.SpawnMinion(MinionType.Basic);

// Spawnear en posición específica
Vector3 spawnPos = new V (Si usas MinionSpawner)

**IMPORTANTE:** El Boss actual NO usa MinionSpawner. Este código es para referencia si decides extender la funcionalidad:

```csharp
// EJEMPLO - No es el código actual del Boss
public class BossCustom : MonoBehaviour
{
    [SerializeField] private MinionSpawner minionSpawner;
    
    private void OnPhase1Started()
    {
        // Fase 1: Minions básicos
        minionSpawner.SpawnMultiple(MinionType.Basic, 2);
    }
    
    private void OnPhase2Started()
    {
        // Fase 2: Minions más difíciles
        minionSpawner.SpawnWave(
            basicCount: 1,
            fastCount: 2,
            tankCount: 0,
            rangedCount: 1
        );
    }
    
    private void OnPhase3Started()
    {
        // Fase 3: Mezcla completa
        minionSpawner.SpawnWave(
            basicCount: 2,
            fastCount: 2,
            tankCount: 1,
            rangedCount: 2
        );
    }
}
```

**El Boss actual (`Boss.cs`) usa únicamente instanciación directa.**

### Gestión de Minions Activos (MinionSpawner)
    
    private void OnPhase3Started()
    {
        // Fase 3: Mezcla completa
        minionSpawner.SpawnWave(
            basicCount: 2,
            fastCount: 2,
            tankCount: 1,
            rangedCount: 2
        );
    }
}
```

### Gestión de Minions Activos

```csharp
// Verificar si se pueden spawnear más minions
if (spawner.CanSpawn)
{
    spawner.SpawnMinion(MinionType.Basic);
}

// Obtener cantidad de minions activos
int activeCount = spawner.ActiveMinionCount;

// Obtener minions de un tipo específico
List<GameObject> tanks = spawner.GetMinionsByType(MinionType.Tank);

// Limpiar todos los minions
spawner.ClearAllMinions();
```

### Daño a Minions

Los minions pueden recibir daño desde balas u otros sistemas:

```csharp
public class Bullet : MonoBehaviour
{
    [SerializeField] private float damage = 10f;
    
    void OnTriggerEnter2D(Collider2D other)
    {
        MinionBehaviour minion = other.GetComponent<MinionBehaviour>();
        if (minion != null)
        {
            minion.TakeDamage(damage);
            Destroy(gameObject); // Destruir la bala
        }
    }
}
```

### Suscripción a Eventos

```csharp
void Start()
{
    MinionBehaviour minion = GetComponent<MinionBehaviour>();
    
    if (minion != null && minion.MinionData != null)
    {
        minion.MinionData.OnMinionHit += OnMinionWasHit;
        minion.MinionData.OnMinionDeath += OnMinionDied;
        minion.MinionData.OnMinionAttack += OnMinionAttacked;
    }
}

void OnMinionWasHit(float damage)
{
    Debug.Log($"Minion received {damage} damage!");
}

void OnMinionDied()
{
    Debug.Log("Minion died!");
    // El enfoque se otorga automáticamente al jugador
}

void OnMinionAttacked()
{
    Debug.Log("Minion attacked!");
}
```

## Máquina de Estados

Los minions siguen esta máquina de estados:

```
IDLE → (Player detectado) → CHASE → (En rango) → ATTACK
  ↓                           ↑
PATROL → (Player detectado) ──┘

CHASE → (Player muy lejos) → IDLE/PATROL
ATTACK → (Player sale de rango) → CHASE

Cualquier estado → (Recibe daño) → HIT → (Regresa al estado anterior)
Cualquier estado → (Vida <= 0) → DEATH
```

## Configuración de Layers

Asegurarse de configurar las layers en Unity:

1. **Player Layer:**
   - Nombre: "Player"
   - Layer Index: 6 (ejemplo)

2. **Enemy Layer:**
   - Nombre: "Enemy"
   - Layer Index: 7 (ejemplo)

3. **Configurar Collision Matrix:**
   - Edit > Project Settings > Physics 2D
   - Enemy puede colisionar con Player
   - Enemy no colisiona con Enemy (opcional)

## Proyectiles del Ranged Minion

### Configuración del Prefab de Proyectil

1. Crear GameObject con sprite del proyectil
2. Agregar componente `Rigidbody2D`:
   - Body Type: Dynamic
   - Gravity Scale: 0
3. Agregar `Collider2D` (Circle o Box)
   - Is Trigger: ☑
4. Agregar componente `MinionProjectile`
5. Configurar en Inspector:
   - Damage: 15
   - Speed: 8
   - Lifetime: 5
   - Player Layer: Player
   - Obstacle Layer: Ground (opcional)
6. Guardar como prefab
7. Asignar en el RangedMinion ScriptableObject

## Debugging
 (Solo con MinionSpawner)

Si usas MinionSpawner, el sistema incluye object pooling automático:
- Activado por defecto
- Pool size configurable por tipo
- Reduce garbage collection
- Mejora rendimiento en escenas con muchos minions

```csharp
// Solo relevante si usas MinionSpawner
minionSpawner.useObjectPooling = false; // Para desactivar
```

**Nota:** El Boss usa instanciación directa (`Instantiate()`), no pooling."[Type] minion spawned!"`
- `"[Type] minion attacked player for X damage!"`
- `"[Type] minion shot projectile at player!"`
- `"Minion received X damage!"`

## Optimización

### Object Pooling

El sistema incluye object pooling automático:
- Activado por defecto
- Pool size configurable por tipo
- Reduce garbage collection
- Mejora rendimiento en escenas con muchos minions

Para desactivar:
```csharp
minionSpawner.useObjectPooling = false;
```

## Próximos Pasos

1. **Animaciones**: Crear Animator Controllers para cada tipo
2. **Audio**: Agregar SFX para spawn, ataque, hit, muerte
3. **VFX**: Crear partículas para efectos visuales
4. **AI Avanzada**: Implementar comportamientos más complejos
5. **Balanceo**: Ajustar stats según pruebas de gameplay

## Troubleshooting

### ⚠️ Problema Más Común: Los Ataques No Funcionan

**Síntoma:** El minion persigue al jugador y se detiene, pero no lo ataca.

**Causas y Soluciones:**

1. **Player Layer no configurado** (causa #1 más común)
   - En el prefab del minion, Inspector → MinionBehaviour → Player Layer
   - Debe estar seleccionado el layer donde está el jugador (normalmente "Player")
   - Si está en "Nothing" o vacío, NO funcionará

2. **Attack Radius muy pequeño** (causa #2 más común)
   - Attack Radius: 0.5 es muy pequeño
   - Aumentar a 1.0-1.5 para empezar
   - Verificar en Scene view: selecciona el minion spawneado, verás un círculo rojo (attack range) - debe ser visible y suficientemente grande

3. **Jugador en layer incorrecto**
   - Seleccionar GameObject del jugador
   - Inspector → Layer → debe estar en "Player"
   - Verificar que tenga tag "Player"

4. **Jugador sin Collider2D**
   - El jugador necesita un Collider2D para ser detectado por OverlapCircleAll
   - Agregar BoxCollider2D o CircleCollider2D al jugador

**Cómo verificar en tiempo real:**
```csharp
// En MinionBehaviour.HandleAttack(), añade estos Debug.Log temporalmente:
Debug.Log($"Attack radius: {attackRadius}, PlayerLayer: {playerLayer.value}");
Collider2D[] hits = Physics2D.OverlapCircleAll(attackPoint.position, attackRadius, playerLayer);
Debug.Log($"Colliders found: {hits.Length}");
```

---

### Otros Problemas

**Los minions no se mueven:**
- Verificar que tienen Rigidbody2D (Dynamic, Gravity Scale: 0)
- Verificar que MinionSO está asignado
- Verificar que el jugador tiene tag "Player"
- Verificar que Detection Range es suficiente (default: 8.0)

**Los minions no detectan al jugador:**
- Verificar Detection Range en el MinionSO (debe ser mayor que la distancia)
- Verificar que el jugador está en la escena activa
- Verificar en Scene view: selecciona el minion, verás un círculo amarillo (detection range)

**Los minions atraviesan paredes:**
- Cambiar Rigidbody2D de Kinematic a Dynamic
- Agregar colliders a las paredes
- Configurar Collision Matrix en Project Settings → Physics 2D
- MinionSO: `Assets/Scripts/Gameplay/ScriptableObject/Interfaces/MinionSO.cs`
- MinionBehaviour: `Assets/Scripts/Gameplay/MinionBehaviour.cs`
- Integración con Boss: [boss-minion-integration.md](boss-minion-integration.md)
- Documentación de diseño: [minion.md](minion.md)
- Ejemplo de patrón: Ver PlayerSO/Player.cs

---

## Resumen Rápido
2 prefabs: BasicMinion y MediumMinion
2. ✅ Asignar MinionSO respectivos
3. ✅ Configurar ambos en Boss (2 slots)
4. ✅ Funciona automáticamente según fasfab
3. ✅ Configurar en Boss
4. ✅ Funciona automáticamente

### Para gestión avanzada (Opcional):
1. 🔧 Usar MinionSpawner
2. 🔧 Configurar múltiples prefabs
3. 🔧 Implementar object pooling
4. 🔧 Controlar oleadas y límites
## Referencias

- IMinion: `Assets/Scripts/Gameplay/Interfaces/IMinion.cs`
- Documentación completa: `spec/Gameplay/minion.md`
- Ejemplo de uso: Ver PlayerSO/Player.cs para patrón similar
