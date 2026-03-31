using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    [Header("Respawn Settings")]
    public Vector3 lastCheckpointPosition = Vector3.zero;
    public float fallDeathHeight = -10f;        

    private CharacterManager player;

    void Start()
    {
        player = FindObjectOfType<CharacterManager>();

        if (player == null)
        {
            Debug.LogError("CheckpointManager : Aucun CharacterManager trouvé !");
            return;
        }

        lastCheckpointPosition = player.transform.position;
    }

    void Update()
    {
        if (player != null && player.transform.position.y < fallDeathHeight)
        {
            RespawnAtCheckpoint();
        }
    }

    public void UpdateCheckpoint(Vector3 newPosition)
    {
        lastCheckpointPosition = newPosition;
        Debug.Log("Checkpoint mis à jour ! Nouvelle position : " + newPosition);
    }

    private void RespawnAtCheckpoint()
    {
        if (player == null) return;

        player.transform.position = lastCheckpointPosition;

        Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
        }

        Debug.Log("Le joueur est tombé dans le vide → Respawn au checkpoint");
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") || 
            collision.gameObject.GetComponent<CharacterManager>() != null)
        {
            RespawnAtCheckpoint();
        }
    }
}