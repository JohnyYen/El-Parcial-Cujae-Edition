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

    [Header("Expansion Settings")]
    private float initialRadius = 1f;
    private float finalRadius = 5f;
    private float expansionDuration = 0.8f;
    private float delayBeforeExpansion = 0.3f; // Tiempo que permanece pequeña antes de expandirse
    private bool isExpanding = false;
    private bool isWaitingToExpand = false;
    private float expansionStartTime = 0f;
    private float waitStartTime = 0f;

    private bool hasHitPlayer = false;
    private float lastDamageTime = 0f;
    private SpriteRenderer spriteRenderer;
    private CircleCollider2D circleCollider;

    void Start()
    {
        // Obtener o crear componentes necesarios
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
        }
        
        circleCollider = GetComponent<CircleCollider2D>();
        if (circleCollider == null)
        {
            circleCollider = gameObject.AddComponent<CircleCollider2D>();
        }
        
        // Configurar el collider como trigger y establecer radio base
        circleCollider.isTrigger = true;
        circleCollider.radius = 0.5f;
        circleCollider.offset = Vector2.zero;
        
        // Crear sprite circular programáticamente
        if (enableVisuals && spriteRenderer != null)
        {
            spriteRenderer.sprite = CreateCircleSprite(512); // Tamaño de textura 512x512 para mejor calidad
            spriteRenderer.color = damageColor;
        }
        
        Debug.Log($"DamageZone inicializado - Collider radius: {circleCollider.radius}, es trigger: {circleCollider.isTrigger}");
    }

    void Update()
    {
        if (isWaitingToExpand)
        {
            // Verificar si ha pasado el tiempo de espera
            if (Time.time - waitStartTime >= delayBeforeExpansion)
            {
                isWaitingToExpand = false;
                isExpanding = true;
                expansionStartTime = Time.time;
                Debug.Log("DamageZone: Iniciando expansión ahora");
            }
        }
        else if (isExpanding)
        {
            UpdateExpansion();
        }
    }

    /// <summary>
    /// Establece el daño de la zona dinámicamente desde el script de ataque.
    /// </summary>
    public void SetDamage(float newDamage)
    {
        damage = newDamage;
    }

    /// <summary>
    /// Configura la expansión del área de daño.
    /// </summary>
    public void SetExpansion(float startRadius, float endRadius, float duration, float delayBeforeExpanding = 0.3f)
    {
        initialRadius = startRadius;
        finalRadius = endRadius;
        expansionDuration = duration;
        delayBeforeExpansion = delayBeforeExpanding;
        
        // Iniciar el período de espera antes de expandir
        isWaitingToExpand = true;
        isExpanding = false;
        waitStartTime = Time.time;
        
        // Establecer tamaño inicial
        SetSize(initialRadius);
        
        Debug.Log($"DamageZone: Manteniendo tamaño inicial {initialRadius} por {delayBeforeExpansion}s antes de expandir a {finalRadius}");
    }

    /// <summary>
    /// Actualiza la expansión progresiva del área.
    /// </summary>
    private void UpdateExpansion()
    {
        float elapsed = Time.time - expansionStartTime;
        float progress = Mathf.Clamp01(elapsed / expansionDuration);
        
        // Interpolación suave del radio
        float currentRadius = Mathf.Lerp(initialRadius, finalRadius, progress);
        SetSize(currentRadius);
        
        // Terminar la expansión cuando se complete
        if (progress >= 1f)
        {
            isExpanding = false;
            Debug.Log($"DamageZone: Expansión completada. Tamaño final: {finalRadius}");
        }
    }

    /// <summary>
    /// Establece el tamaño de la zona (collider y sprite).
    /// </summary>
    private void SetSize(float radius)
    {
        // Mantener el collider en radio 0.5 (tamaño base del sprite generado)
        // El transform.localScale se encargará de escalarlo correctamente
        if (circleCollider != null)
        {
            circleCollider.radius = 0.5f;
        }
        
        // Actualizar solo la escala del transform
        // El sprite generado tiene un radio de 0.5 unidades, así que necesitamos escalar por el diámetro
        float scale = radius * 2f;
        transform.localScale = new Vector3(scale, scale, 1f);
        
        // Debug para verificar el tamaño real
        float effectiveRadius = circleCollider != null ? circleCollider.radius * scale : 0f;
        Debug.Log($"DamageZone SetSize - Radio deseado: {radius}, Escala: {scale}, Radio efectivo del collider: {effectiveRadius}");
    }

    /// <summary>
    /// Crea un sprite circular programáticamente.
    /// </summary>
    /// <param name="textureSize">Tamaño de la textura en píxeles (debe ser potencia de 2)</param>
    /// <returns>Sprite circular generado</returns>
    private Sprite CreateCircleSprite(int textureSize)
    {
        // Crear textura con formato RGBA32 para mejor calidad
        Texture2D texture = new Texture2D(textureSize, textureSize, TextureFormat.RGBA32, false);
        
        // Configurar filtrado para que sea nítido (sin interpolación)
        texture.filterMode = FilterMode.Bilinear; // Suave pero no borroso
        texture.wrapMode = TextureWrapMode.Clamp;
        
        // Centro del círculo
        float centerX = textureSize / 2f;
        float centerY = textureSize / 2f;
        float radius = textureSize / 2f;
        
        // Generar píxeles del círculo con anti-aliasing suave
        Color[] pixels = new Color[textureSize * textureSize];
        for (int y = 0; y < textureSize; y++)
        {
            for (int x = 0; x < textureSize; x++)
            {
                // Calcular distancia desde el centro
                float dx = x - centerX;
                float dy = y - centerY;
                float distance = Mathf.Sqrt(dx * dx + dy * dy);
                
                // Crear un gradiente suave en el borde para anti-aliasing
                float alpha = 1f - Mathf.Clamp01((distance - radius + 2f) / 2f);
                
                // Asignar color blanco con alpha variable
                pixels[y * textureSize + x] = new Color(1f, 1f, 1f, alpha);
            }
        }
        
        // Aplicar píxeles a la textura
        texture.SetPixels(pixels);
        texture.Apply(false, false); // No generar mipmaps, no hacer readonly
        
        // Crear sprite desde la textura
        // El sprite tendrá un radio de 0.5 unidades en el mundo (diámetro de 1 unidad)
        Sprite sprite = Sprite.Create(
            texture,
            new Rect(0, 0, textureSize, textureSize),
            new Vector2(0.5f, 0.5f), // Pivot en el centro
            textureSize // pixelsPerUnit = textureSize para que el sprite tenga 1 unidad de diámetro
        );
        
        return sprite;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!damageOnEnter || !collision.CompareTag("Player"))
            return;

        if (!hasHitPlayer)
        {
            // Debug para verificar distancia real
            float distance = Vector2.Distance(transform.position, collision.transform.position);
            float currentScale = transform.localScale.x;
            float effectiveRadius = circleCollider != null ? circleCollider.radius * currentScale : 0f;
            Debug.Log($"Colisión detectada - Distancia al jugador: {distance}, Radio efectivo: {effectiveRadius}, Escala: {currentScale}");
            
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
