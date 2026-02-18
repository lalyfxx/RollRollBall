using UnityEngine;

public class WorldKill : MonoBehaviour
{
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
        Debug.LogError("GAME OVER - Le slime a touché un ennemi !");

    
        Time.timeScale = 0f;

        
    }
}
