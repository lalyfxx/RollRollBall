using UnityEngine;
using System.Collections;

public class CharacterManager : MonoBehaviour
{
    [Header("Mouvement & Saut")]
    public float jumpForce = 9f;
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask whatIsGround;

    [Header("Réduction Taille")]
    public float smallScale = 0.5f;
    public KeyCode smallKey = KeyCode.Q;
    public KeyCode normalKey = KeyCode.D;
    public float sizeChangeCooldown = 0.6f;

    [Header("Projectile")]
    public GameObject projectilePrefab;
    public Transform firePoint;
    public float projectileForce = 12f;
    public KeyCode shootKey = KeyCode.S;
    public float shootCooldown = 0.2f;

    [Header("Animation")]
    public Animator animator;

    private Rigidbody2D rb;
    private bool isGrounded;
    private bool isSmall = false;
    private Vector3 originalScale;
    private float lastShootTime;
    private float lastSizeChangeTime;

    private static readonly int IsSmall = Animator.StringToHash("IsSmall");

    private SpriteRenderer spriteRenderer;
    private Coroutine flashCoroutine;   // ← Important pour arrêter l'ancien clignotement

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb == null) rb = gameObject.AddComponent<Rigidbody2D>();

        rb.gravityScale = 3f;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionX;

        originalScale = transform.localScale;

        if (animator == null)
            animator = GetComponent<Animator>();

        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    void Update()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);

        // Saut
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.linearVelocity = new Vector2(0f, jumpForce);
        }

        // Changement de taille avec cooldown
        if (Time.time > lastSizeChangeTime + sizeChangeCooldown)
        {
            if (Input.GetKeyDown(smallKey) && !isSmall)
            {
                BecomeSmall();
            }

            if (Input.GetKeyDown(normalKey) && isSmall)
            {
                BecomeNormal();
            }
        }

        // Tentative de tir
        if (Input.GetKeyDown(shootKey))
        {
            if (!isSmall)
            {
                if (Time.time > lastShootTime + shootCooldown)
                    Shoot();
            }
            else
            {
                FlashRedFeedback();     // Version améliorée
            }
        }
    }

    private void BecomeSmall()
    {
        isSmall = true;
        transform.localScale = originalScale * smallScale;
        lastSizeChangeTime = Time.time;

        if (animator != null)
            animator.SetBool(IsSmall, true);

        Debug.Log("Slime → Mode PETIT");
    }

    private void BecomeNormal()
    {
        isSmall = false;
        transform.localScale = originalScale;
        lastSizeChangeTime = Time.time;

        if (animator != null)
            animator.SetBool(IsSmall, false);

        Debug.Log("Slime → Mode NORMAL");
    }

    private void Shoot()
    {
        if (projectilePrefab == null || firePoint == null) return;

        lastShootTime = Time.time;

        GameObject proj = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        Rigidbody2D projRb = proj.GetComponent<Rigidbody2D>();
        if (projRb != null)
        {
            projRb.linearVelocity = Vector2.right * projectileForce;
        }
    }

    // ====================== FEEDBACK CLIGNOTEMENT ======================
    private void FlashRedFeedback()
    {
        // Arrête le clignotement précédent s'il est encore en cours
        if (flashCoroutine != null)
            StopCoroutine(flashCoroutine);

        flashCoroutine = StartCoroutine(FlashRedRoutine());
    }

    private IEnumerator FlashRedRoutine()
    {
        if (spriteRenderer == null) yield break;

        Color originalColor = spriteRenderer.color;
        float duration = 0.35f;
        float timer = 0f;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            // Clignotement plus net et contrôlé
            float alpha = Mathf.PingPong(timer * 15f, 1f);
            spriteRenderer.color = Color.Lerp(originalColor, new Color(1f, 0.3f, 0.3f), alpha);
            yield return null;
        }

        // Retour garanti à la couleur d'origine
        spriteRenderer.color = originalColor;
    }

    // ====================== COLLISIONS ======================
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            GameOver();
        }
    }

    private void GameOver()
    {
        Debug.LogError("=== GAME OVER === Le slime a touché un ennemi !");
        Time.timeScale = 0f;
    }

    void OnDrawGizmosSelected()
    {
        if (groundCheck == null) return;
        Gizmos.color = isGrounded ? Color.green : Color.red;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }
}