using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    [Header("Mouvement")]
    public float horizontalSpeed = 6f;

    [Header("Saut & Sol")]
    public float jumpForce = 9f;
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask whatIsGround;

    [Header("Réduction de Taille")]
    public float splitScale = 0.5f;           // Réduction par niveau
    public int maxDivisions = 4;
    public KeyCode reduceKey = KeyCode.S;     // Réduire
    public KeyCode resetKey = KeyCode.F;      // Retour normal

    [Header("Projectile")]
    public GameObject projectilePrefab;
    public Transform firePoint;
    public float projectileForce = 12f;
    public KeyCode shootKey = KeyCode.D;
    public float shootCooldown = 0.2f;

    private Rigidbody2D rb;
    private bool isGrounded;
    private int currentDivisionLevel = 0;
    private Vector3 baseScale;
    private float lastShootTime;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb == null) rb = gameObject.AddComponent<Rigidbody2D>();

        rb.gravityScale = 3f;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionX;

        baseScale = transform.localScale;   // Sauvegarde taille originale (ex: 0.18)
    }

    void Update()
    {
        // Détection sol
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);

        // Saut
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.linearVelocity = new Vector2(0f, jumpForce);
        }

        // Réduire la taille
        if (Input.GetKeyDown(reduceKey) && currentDivisionLevel < maxDivisions)
        {
            ReduceSize();
        }

        // Retour taille normale
        if (Input.GetKeyDown(resetKey))
        {
            ResetToOriginalSize();
        }

        // Tirer
        if (Input.GetKeyDown(shootKey) && Time.time > lastShootTime + shootCooldown)
        {
            Shoot();
        }
    }

    private void ReduceSize()
    {
        currentDivisionLevel++;
        float scaleFactor = Mathf.Pow(splitScale, currentDivisionLevel);
        transform.localScale = baseScale * scaleFactor;

        Debug.Log($"Slime réduit → Niveau {currentDivisionLevel} | Taille = {transform.localScale.x:F2}");
    }

    private void ResetToOriginalSize()
    {
        transform.localScale = baseScale;
        currentDivisionLevel = 0;
        Debug.Log("Slime retourné à sa taille originale");
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

    // ====================== MORT DU JOUEUR ======================
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
        Time.timeScale = 0f;        // Arrête tout le jeu
    }

    void OnDrawGizmosSelected()
    {
        if (groundCheck == null) return;
        Gizmos.color = isGrounded ? Color.green : Color.red;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }
}