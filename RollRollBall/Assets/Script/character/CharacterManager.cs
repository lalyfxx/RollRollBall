using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    [Header("Mouvement & Saut")]
    public float jumpForce = 9f;
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask whatIsGround;

    [Header("Réduction Taille")]
    public float smallScale = 0.5f;           // Taille quand il est petit
    public KeyCode smallKey = KeyCode.S;      // Devenir petit
    public KeyCode normalKey = KeyCode.F;     // Redevenir normal

    [Header("Projectile")]
    public GameObject projectilePrefab;
    public Transform firePoint;
    public float projectileForce = 12f;
    public KeyCode shootKey = KeyCode.D;
    public float shootCooldown = 0.2f;

    [Header("Animation")]
    public Animator animator;

    private Rigidbody2D rb;
    private bool isGrounded;
    private bool isSmall = false;
    private Vector3 originalScale;
    private float lastShootTime;

    private static readonly int IsSmall = Animator.StringToHash("IsSmall");

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb == null) rb = gameObject.AddComponent<Rigidbody2D>();

        rb.gravityScale = 3f;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionX;

        originalScale = transform.localScale;

        if (animator == null)
            animator = GetComponent<Animator>();
    }

    void Update()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);

        // Saut
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.linearVelocity = new Vector2(0f, jumpForce);
        }

        // Devenir petit
        if (Input.GetKeyDown(smallKey) && !isSmall)
        {
            BecomeSmall();
        }

        // Redevenir normal
        if (Input.GetKeyDown(normalKey) && isSmall)
        {
            BecomeNormal();
        }

        // TIR → uniquement quand il n'est PAS petit
        if (Input.GetKeyDown(shootKey) && !isSmall && Time.time > lastShootTime + shootCooldown)
        {
            Shoot();
        }
    }

    private void BecomeSmall()
    {
        isSmall = true;
        transform.localScale = originalScale * smallScale;

        if (animator != null)
            animator.SetBool(IsSmall, true);

        Debug.Log("Slime → Mode PETIT (tir désactivé)");
    }

    private void BecomeNormal()
    {
        isSmall = false;
        transform.localScale = originalScale;

        if (animator != null)
            animator.SetBool(IsSmall, false);

        Debug.Log("Slime → Mode NORMAL (tir activé)");
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