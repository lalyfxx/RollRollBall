using UnityEngine;

public class ParallaxLayer : MonoBehaviour
{
    [SerializeField] private float parallaxFactor = 0.3f;     // 0 = immobile, 1 = suit le scroll exactement
    [SerializeField] private float repeatLength = 20f;        // largeur d'une "image" en unités Unity

    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;
    }

    void LateUpdate()
    {
        float speed = WorldScroller.GlobalScrollSpeed * parallaxFactor;

        transform.position += Vector3.right * -speed * Time.deltaTime;

        // Boucle infinie
        if (transform.position.x <= startPosition.x - repeatLength)
        {
            transform.position += Vector3.right * repeatLength;
        }
        else if (transform.position.x >= startPosition.x + repeatLength)
        {
            transform.position -= Vector3.right * repeatLength;
        }
    }
}