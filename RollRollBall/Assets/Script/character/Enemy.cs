using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Vie")]
    public int health = 1;

    [Header("Effet mort")]
    public GameObject deathEffect;  

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

    
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") || 
            collision.gameObject.GetComponent<CharacterManager>() != null)
        {
            
            GameOver();
        }
    }

    private void GameOver()
    {
        Debug.LogError("GAME OVER");

    
        Time.timeScale = 0f;

        
    }
}