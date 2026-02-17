using UnityEngine;

public class Projectile : MonoBehaviour
{
    [Header("Vitesse")]
    public float speed = 12f;
    
    [Header("Vie du projectile")]
    public float lifetime = 3f;        // disparaît après 3 secondes
    
    [Header("Dommages")]
    public int damage = 1;

    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb == null) rb = gameObject.AddComponent<Rigidbody2D>();
        
        // Pas de gravité, va tout droit
        rb.gravityScale = 0f;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    void Start()
    {
        // Auto-destruction après lifetime
        Destroy(gameObject, lifetime);
    }

    void FixedUpdate()
    {
        // Va tout droit vers la droite (dans le sens du runner)
        rb.linearVelocity = Vector2.right * speed;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Détruit les ennemis
        Enemy enemy = other.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
            Destroy(gameObject);  // Le projectile disparaît après impact
        }
    }

    void OnBecameInvisible()
    {
        Destroy(gameObject);  // Optimisation : si hors écran
    }
}