using UnityEngine;
using UnityEngine.SceneManagement;  
using System.Collections;

public class CheckpointManager : MonoBehaviour
{
    [Header("Death & Scene Settings")]
    public float fallDeathHeight = -10f;                    
    public string gameOverSceneName = "GameOver";           

    [Header("Respawn (optionnel)")]
    public bool useRespawnInsteadOfGameOver = false;        

    private CharacterManager player;

    void Start()
    {
        player = FindObjectOfType<CharacterManager>();

        if (player == null)
        {
            Debug.LogError("CheckpointManager : Aucun CharacterManager trouvé dans la scène !");
        }
    }

    void Update()
    {
        if (player == null) return;

        if (player.transform.position.y < fallDeathHeight)
        {
            TriggerGameOver();
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") || 
            collision.gameObject.GetComponent<CharacterManager>() != null)
        {
            TriggerGameOver();
        }
    }

    private void TriggerGameOver()
    {
        Debug.LogError("=== GAME OVER === Le joueur est tombé dans le vide !");

        Time.timeScale = 0f;

        StartCoroutine(LoadGameOverScene());
    }

    private IEnumerator LoadGameOverScene()
    {
        yield return new WaitForSecondsRealtime(1.2f);

        Time.timeScale = 1f;

        if (!string.IsNullOrEmpty(gameOverSceneName))
        {
            SceneManager.LoadScene(gameOverSceneName);
        }
        else
        {
            Debug.LogError("Aucun nom de scène Game Over défini dans CheckpointManager !");
        }
    }
}