using UnityEngine;

public class WorldScroller : MonoBehaviour
{
    [SerializeField] private float scrollSpeed = 6f;

    public static float GlobalScrollSpeed { get; private set; }

    void FixedUpdate()
    {
        transform.position += Vector3.right * -scrollSpeed * Time.fixedDeltaTime;
    }

    void Update()
    {
        GlobalScrollSpeed = scrollSpeed;
    }
}