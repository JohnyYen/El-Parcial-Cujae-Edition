# Integración Boss - Sistema de Minions

## Descripción

El Boss spawns minions usando `Instantiate()` con **dos prefabs**: uno para Basic y otro para Medium. El sistema es simple y directo.

## Tipos de Minions

| Tipo | Vida | Velocidad | Daño | Usado por Boss |
|------|------|-----------|------|----------------|
| **Basic** | 50 HP | 2.0 | 10 | ✅ Sí (Fase 1 y 2) |
| **Medium** | 70 HP | 3.5 | 20 | ✅ Sí (Fase 2 y 3) |
| **Hard** | 120 HP | 1.8 | 30 | ❌ No (reservado para otros usos) |

---

## Configuración en Unity

### Paso 1: Crear Prefabs de Minions

#### Prefab Basic Minion

1. Crear GameObject: "BasicMinion"
2. Agregar componentes:
   - `MinionBehaviour`
   - `Rigidbody2D` (Dynamic, Gravity Scale: 0, Freeze Rotation Z)
   - `BoxCollider2D` o `CircleCollider2D`
   - `SpriteRenderer`
   - (Opcional) `Animator`

3. **Configurar MinionBehaviour** ⚠️ IMPORTANTE:
   ```
   MinionBehaviour:
   ├── Minion Data: BasicMinion (ScriptableObject)
   ├── Player Transform: [Dejar vacío - auto-detecta]
   ├── Sprite Renderer: [Asignar]
   ├── Attack Point: [Transform hijo o self]
   ├── Attack Radius: 1.0-1.5 (⚠️ 0.5 es muy corto, ajustar según tu juego)
   └── Player Layer: Player (⚠️ CRÍTICO - debe coincidir con el layer del jugador)
   ```
   
   **Notas importantes:**
   - **Attack Radius:** Empieza con 1.0-1.5 y ajusta según tu escala de juego. Si es muy pequeño, los minions no detectarán al jugador.
   - **Player Layer:** DEBE estar configurado correctamente o los ataques no funcionarán. Asegúrate de que el GameObject del jugador esté en el layer "Player".

4. Layer: **Enemy**
5. Guardar como: `Assets/Prefabs/Minions/BasicMinion.prefab`

#### Prefab Medium Minion

Repetir el proceso anterior, pero:
- Nombre: "MediumMinion"
- Minion Data: **MediumMinion** (ScriptableObject)
- Guardar como: `Assets/Prefabs/Minions/MediumMinion.prefab`

---

### Paso 2: Crear ScriptableObject Assets

1. En `Assets/Scripts/Gameplay/ScriptableObject/Objects/`
2. Click derecho → `Create > Minions > Basic Minion`
3. Nombrar: "BasicMinionData"
4. Repetir para Medium: `Create > Minions > Medium Minion`

---

### Paso 3: Configurar Boss

1. **Crear spawn points**:
   ```
   SpawnPointsContainer (GameObject)
   ├── SpawnPoint_01
   ├── SpawnPoint_02
   ├── SpawnPoint_03
   └── SpawnPoint_04
   ```

2. **Configurar componente Boss**:
   ```
   Boss:
   ├── Boss Behaviour: ElParcialBoss
   ├── Attack Interval: 3
   ├── Minion Spawn Interval: 5
   ├── Basic Minion Prefab: BasicMinion [Arrastrar]
   ├── Medium Minion Prefab: MediumMinion [Arrastrar]
   └── Spawn Points: SpawnPointsContainer [Arrastrar]
   ```

---

## Comportamiento por Fase

### Fase 1 - "Tranquilo, era fácil"
- **100% Basic Minions**
- Propósito: Introducir mecánica

### Fase 2 - "Esto no lo dimos"
- **50% Basic, 50% Medium** (aleatorio)
- Propósito: Aumentar dificultad gradualmente

### Fase 3 - "El Integrador"
- **100% Medium Minions**
- Propósito: Máxima presión

---

## Flujo de Ejecución

```
Boss.Update()
    ↓
Tiempo de spawn?
    ↓
SpawnMinion()
    ├─ Detecta fase actual
    ├─ Selecciona tipo (Basic o Medium)
    ├─ Selecciona prefab correspondiente
    └─ Instantiate(prefab, spawnPoint)
    ↓
MinionBehaviour.Start()
    ├─ minionData.Initialize()
    ├─ Busca Player automáticamente
    └─ Inicia máquina de estados
    ↓
✅ Minion funciona completamente
```

---

## Verificación

1. ✅ Crear 2 prefabs (BasicMinion y MediumMinion)
2. ✅ Crear 2 ScriptableObjects (BasicMinionData y MediumMinionData)
3. ✅ Asignar ambos prefabs al Boss
4. ✅ Configurar spawn points
5. ✅ **Configurar Player Layer** en el prefab del minion
6. ✅ **Ajustar Attack Radius** (1.0-1.5 recomendado)
7. ✅ Verificar que el jugador tenga tag "Player" y esté en el layer correcto
8. ✅ Play y verificar:
   - Fase 1: Solo minions básicos
   - Fase 2: Mezcla de basic y medium
   - Fase 3: Solo minions medium
   - Los minions persiguen al jugador
   - Los minions atacan cuando están cerca

---

## ⚠️ Problemas Comunes y Soluciones

### Los minions no atacan
**Causa:** Player Layer no configurado o Attack Radius muy pequeño
**Solución:**
1. Verificar que el campo "Player Layer" en MinionBehaviour tenga seleccionado el layer del jugador
2. Aumentar Attack Radius a 1.0-1.5
3. Verificar en Scene view que el círculo rojo (gizmo de attack range) sea visible cuando seleccionas el minion

### Los minions no detectan al jugador
**Causa:** GameObject del jugador no tiene el tag o layer correcto
**Solución:**
1. Seleccionar el GameObject del jugador
2. En Inspector, verificar:
   - Tag: "Player"
   - Layer: "Player" (o el que hayas configurado)
3. Verificar Detection Range en el ScriptableObject del minion (default: 8.0)

### Los ataques no hacen daño
**Causa:** Collider del jugador no está en el layer correcto
**Solución:**
1. Verificar que el jugador tenga un Collider2D
2. Verificar que el layer mask en MinionBehaviour incluya el layer del jugador
3. Usar Debug.Log en MinionBehaviour.HandleAttack() para verificar detección

---

## Notas Importantes

- **Hard Minions** no son spawneados por el Boss. Se reservan para otros usos (ej: eventos especiales, otras áreas).
- El Boss alterna aleatoriamente entre Basic y Medium en Fase 2 para variedad.
- Cada prefab debe tener su propio MinionSO asignado en el Inspector.
- Los minions se auto-inicializan, no requieren configuración adicional en runtime.
