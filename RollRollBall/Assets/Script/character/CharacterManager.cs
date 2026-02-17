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

    [Header("Division Slime")]
    public GameObject slimePrefab;
    public float splitScale = 0.5f;
    public float splitForce = 3f;
    public KeyCode splitKey = KeyCode.S;
    public KeyCode mergeKey = KeyCode.F;

    [Header("Limite Division")]
    public int maxDivisions = 4;

    [Header("PROJECTILE")]                    // ← ÇA c'est OK
    public GameObject projectilePrefab;       // ← ÇA c'est OK
    public Transform firePoint;
    public float projectileForce = 12f;
    public KeyCode shootKey = KeyCode.D;
    public float shootCooldown = 0.2f;

    private Rigidbody2D rb;
    private bool isGrounded;
    private int currentDivisionLevel = 0;
    private Vector3 baseScale;
    private float lastShootTime;              // ← NOUVEAU

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb == null) rb = gameObject.AddComponent<Rigidbody2D>();

        rb.gravityScale = 3f;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionX;

        baseScale = transform.localScale;

        if (SlimeManager.Instance != null)
        {
            SlimeManager.Instance.RegisterSlime(gameObject);
        }
    }

    void Update()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.linearVelocity = new Vector2(0f, jumpForce);
        }

        if (Input.GetKeyDown(splitKey) && slimePrefab != null && currentDivisionLevel < maxDivisions)
        {
            Split();
        }

        if (Input.GetKeyDown(mergeKey))
        {
            MergeToNormal();
        }

        // ───────────── TIR ─────────────
        if (Input.GetKeyDown(shootKey) && Time.time > lastShootTime + shootCooldown)
        {
            Shoot();
        }
    }

    private void Split()
    {
        currentDivisionLevel++;

        Vector3 pos = transform.position;
        Quaternion rot = transform.rotation;

        float scaleFactor = Mathf.Pow(splitScale, currentDivisionLevel);
        Vector3 newScale = baseScale * scaleFactor;

        // Slime GAUCHE
        GameObject left = Instantiate(slimePrefab, pos + Vector3.left * 0.4f, rot);
        left.transform.localScale = newScale;
        var leftRb = left.GetComponent<Rigidbody2D>();
        if (leftRb) leftRb.linearVelocity = new Vector2(-splitForce, 0f);

        var leftManager = left.GetComponent<CharacterManager>();
        if (leftManager)
        {
            leftManager.currentDivisionLevel = currentDivisionLevel;
            leftManager.baseScale = baseScale;
        }

        // Slime DROIT
        GameObject right = Instantiate(slimePrefab, pos + Vector3.right * 0.4f, rot);
        right.transform.localScale = newScale;
        var rightRb = right.GetComponent<Rigidbody2D>();
        if (rightRb) rightRb.linearVelocity = new Vector2(splitForce, 0f);

        var rightManager = right.GetComponent<CharacterManager>();
        if (rightManager)
        {
            rightManager.currentDivisionLevel = currentDivisionLevel;
            rightManager.baseScale = baseScale;
        }

        Destroy(gameObject);
    }

    private void MergeToNormal()
    {
        if (SlimeManager.Instance == null)
        {
            transform.localScale = baseScale;
            currentDivisionLevel = 0;
            return;
        }

        transform.localScale = baseScale;
        currentDivisionLevel = 0;
        SlimeManager.Instance.MergeAllSlimes(gameObject);
    }

    // ───────────── NOUVEAU : FONCTION TIR ─────────────
    private void Shoot()
    {
        if (projectilePrefab == null || firePoint == null) return;

        lastShootTime = Time.time;

        GameObject proj = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        Rigidbody2D projRb = proj.GetComponent<Rigidbody2D>();
        if (projRb)
        {
            projRb.linearVelocity = Vector2.right * projectileForce;
        }
    }

    void OnDrawGizmosSelected()
    {
        if (groundCheck == null) return;
        Gizmos.color = isGrounded ? Color.green : Color.red;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }

    void OnDestroy()
    {
        if (SlimeManager.Instance != null)
        {
            SlimeManager.Instance.UnregisterSlime(gameObject);
        }
    }
}