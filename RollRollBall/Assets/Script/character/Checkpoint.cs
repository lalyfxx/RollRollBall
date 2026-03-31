using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private CheckpointManager checkpointManager;

    void Start()
    {
        checkpointManager = FindObjectOfType<CheckpointManager>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") || other.GetComponent<CharacterManager>() != null)
        {
            if (checkpointManager != null)
            {
                checkpointManager.UpdateCheckpoint(transform.position);
            }

            Debug.Log("Checkpoint activé !");
        }
    }
}