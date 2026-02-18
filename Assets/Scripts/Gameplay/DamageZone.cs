using UnityEngine;

/// <summary>
/// Script para zonas de daño de área creadas por ataques del boss.
/// Detecta cuando el jugador entra en la zona y aplica daño.
/// </summary>
public class DamageZone : MonoBehaviour
{
    [Header("Damage Settings")]
    [SerializeField] private float damage = 25f;
    [SerializeField] private bool damageOnEnter = true;
    [SerializeField] private bool damageOnStay = false;
    [SerializeField] private float damageTickInterval = 0.5f;

    [Header("Visual Settings")]
    [SerializeField] private bool enableVisuals = true;
    [SerializeField] private Color damageColor = Color.red;

    private bool hasHitPlayer = false;
    private float lastDamageTime = 0f;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        // Obtener componente spriterenderer para efectos visuales
        spriteRenderer = GetComponent<SpriteRenderer>();
        
        if (enableVisuals && spriteRenderer != null)
        {
            spriteRenderer.color = damageColor;
        }
    }

    /// <summary>
    /// Establece el daño de la zona dinámicamente desde el script de ataque.
    /// </summary>
    public void SetDamage(float newDamage)
    {
        damage = newDamage;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!damageOnEnter || !collision.CompareTag("Player"))
            return;

        if (!hasHitPlayer)
        {
            ApplyDamage(collision);
            hasHitPlayer = true;
        }
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if (!damageOnStay || !collision.CompareTag("Player"))
            return;

        // Aplicar daño en intervalos mientras el jugador está en la zona
        if (Time.time - lastDamageTime >= damageTickInterval)
        {
            ApplyDamage(collision);
            lastDamageTime = Time.time;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            hasHitPlayer = false;
        }
    }

    /// <summary>
    /// Aplica daño al objetivo que entró en la zona.
    /// </summary>
    private void ApplyDamage(Collider2D collision)
    {
        Player player = collision.GetComponent<Player>();
        if (player != null && player.player_behaviour != null)
        {
            player.player_behaviour.AddStress(damage);
            Debug.Log($"Zona de daño golpea al jugador por {damage} de estrés");
        }
    }
}
