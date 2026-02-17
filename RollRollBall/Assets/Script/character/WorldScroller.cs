using UnityEngine;

public class WorldScroller : MonoBehaviour
{
    [SerializeField] private float scrollSpeed = 6f;
    [SerializeField] private bool scrollingRightToLeft = true;  // habituellement true

    private float direction => scrollingRightToLeft ? -1f : 1f;

    void FixedUpdate()
    {
        transform.position += Vector3.right * direction * scrollSpeed * Time.fixedDeltaTime;
    }

    // Pour debug / tests rapides
    public static float GlobalScrollSpeed { get; private set; }

    void Update()
    {
        GlobalScrollSpeed = scrollSpeed;
    }
}