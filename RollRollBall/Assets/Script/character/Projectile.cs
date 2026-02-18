using UnityEngine;

public class Projectile : MonoBehaviour
{
    [Header("Vitesse")]
    public float speed = 12f;
    
    [Header("Vie du projectile")]
    public float lifetime = 3f;        
    
    [Header("Dommages")]
    public int damage = 1;

    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb == null) rb = gameObject.AddComponent<Rigidbody2D>();
        
        
        rb.gravityScale = 0f;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    void Start()
    {
        
        Destroy(gameObject, lifetime);
    }

    void FixedUpdate()
    {
        
        rb.linearVelocity = Vector2.right * speed;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        
        Enemy enemy = other.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
            Destroy(gameObject); 
        }
    }

    void OnBecameInvisible()
    {
        Destroy(gameObject); 
    }
}