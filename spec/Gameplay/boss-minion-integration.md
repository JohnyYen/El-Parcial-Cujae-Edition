# Integración Boss - Sistema de Minions (Versión Simple)

## Descripción

El Boss usa `Instantiate(minionPrefab, ...)` para spawnear minions. El sistema de Minions es **autocontenido** - cada prefab tiene todo lo necesario para funcionar independientemente.

---

## Configuración en Unity

### Paso 1: Crear Prefab de Minion

1. Crear GameObject vacío: "MinionBasic"
2. Agregar componentes:
   - `MinionBehaviour`
   - `Rigidbody2D` (Dynamic, Gravity Scale: 0, Freeze Rotation Z)
   - `BoxCollider2D` o `CircleCollider2D`
   - `SpriteRenderer`
   - (Opcional) `Animator`

3. **Configurar MinionBehaviour** en el Inspector:
   ```
   MinionBehaviour:
   ├── Minion Data: BasicMinion (ScriptableObject)
   ├── Player Transform: [Dejar vacío - se auto-detecta]
   ├── Sprite Renderer: [Asignar]
   ├── Attack Point: [Transform hijo o self]
   ├── Attack Radius: 0.5
   └── Player Layer: Player
   ```

4. Asignar **Layer: Enemy** al GameObject

5. Guardar como prefab en `Assets/Prefabs/Minions/BasicMinion.prefab`

6. **Repetir** para cada tipo de minion

---

### Paso 2: Crear ScriptableObject Assets

1. En `Assets/Scripts/Gameplay/ScriptableObject/Objects/`
2. Click derecho → `Create > Minions > Basic Minion`
3. Nombrar: "BasicMinionData"
4. Los valores por defecto ya están configurados correctamente
5. Repetir para Fast, Tank y Ranged

---

### Paso 3: Configurar Boss

1. Crear GameObject con hijos para spawn points:
   ```
   SpawnPointsContainer
   ├── SpawnPoint_01
   ├── SpawnPoint_02
   ├── SpawnPoint_03
   └── SpawnPoint_04
   ```

2. En el componente **Boss**:
   ```
   Boss:
   ├── Boss Behaviour: ElParcialBoss
   ├── Attack Interval: 3
   ├── Minion Spawn Interval: 5
   ├── Minion Prefab: BasicMinion prefab  [Arrastrar]
   └── Spawn Points: SpawnPointsContainer  [Arrastrar el padre]
   ```

---

## Comportamiento Automático

Una vez configurado, todo funciona automáticamente:

```
Boss spawns Minion
    ↓
Instantiate(minionPrefab, position, identity)
    ↓
MinionBehaviour.Start() ejecuta
    ↓
minionData.Initialize() 
    ↓
Busca Player automáticamente
    ↓
Máquina de estados comienza
    ↓
Minion funciona completamente
```

---

## Limitación Actual

El Boss solo puede spawnear **un tipo de minion** (el asignado en `minionPrefab`).

**Soluciones opcionales** (sin modificar Boss.cs):

### Opción A: Array de Prefabs (Requiere cambio mínimo en Boss)
Cambiar `minionPrefab` a `minionPrefabs[]` y seleccionar según fase.

### Opción B: Prefab Inteligente (Compatible sin cambios)
Crear un prefab "MinionDynamic" con un script selector:

```csharp
public class MinionPhaseSelector : MonoBehaviour
{
    [SerializeField] private MinionSO[] minionDataByPhase; // [0]=Basic, [1]=Fast, [2]=Tank
    
    void Start()
    {
        Boss boss = FindObjectOfType<Boss>();
        int phase = boss.BossBehaviour.CurrentPhase;
        
        MinionBehaviour minion = GetComponent<MinionBehaviour>();
        minion.SetMinionData(minionDataByPhase[phase - 1]);
    }
}
```

Pero esto requiere agregar `SetMinionData()` a MinionBehaviour.

---

## Recomendación

**Para MVP/GameJam**: Usar un solo tipo de minion por simplicidad.

**Para versión completa**: Usar MinionSpawner (sistema completo implementado en `MinionSpawner.cs`) que maneja:
- Múltiples tipos
- Object pooling
- Límites de minions
- Spawn points estratégicos

---

## Verificación

1. ✅ Crear prefab con MinionBehaviour
2. ✅ Asignar MinionSO al prefab
3. ✅ Configurar Boss con el prefab
4. ✅ Crear spawn points
5. ✅ Play: Los minions deben spawnear y perseguir al jugador automáticamente
