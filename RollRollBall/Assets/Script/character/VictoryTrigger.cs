using UnityEngine;
using UnityEngine.SceneManagement;

public class VictoryTrigger : MonoBehaviour
{
    [Header("Victoire")]
    public string victorySceneName = "Victory";    

    [Header("Optionnel")]
    public bool disablePlayerMovementOnWin = true;  

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") || other.GetComponent<CharacterManager>() != null)
        {
            TriggerVictory();
        }
    }

    private void TriggerVictory()
    {
        Debug.Log(" VICTOIRE ! Le joueur a atteint la fin !");

        if (disablePlayerMovementOnWin)
        {
            CharacterManager player = FindObjectOfType<CharacterManager>();
            if (player != null)
            {
                Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    rb.linearVelocity = Vector2.zero; 
                }
            }
        }

        // Charge la scène Victory
        SceneManager.LoadScene(victorySceneName);
    }
}