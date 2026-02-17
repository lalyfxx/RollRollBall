using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Vie")]
    public int health = 1;

    [Header("Effet mort")]
    public GameObject deathEffect;  // optionnel

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        if (deathEffect != null)
        {
            Instantiate(deathEffect, transform.position, Quaternion.identity);
        }
        Destroy(gameObject);
    }

    // ────────────────────────────────────────────────
    // CONTACT AVEC LE SLIME → GAME OVER
    // ────────────────────────────────────────────────
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") || 
            collision.gameObject.GetComponent<CharacterManager>() != null)
        {
            // Le slime touche l'ennemi → mort
            GameOver();
        }
    }

    private void GameOver()
    {
        Debug.LogError("GAME OVER - Le slime a touché un ennemi !");

        // Arrête le jeu (tout s'arrête physiquement)
        Time.timeScale = 0f;

        // Option : tu peux aussi arrêter la musique, afficher un écran, etc.
        // Pour l'instant on freeze juste la scène
    }
}