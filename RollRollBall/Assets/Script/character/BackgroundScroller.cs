using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class BackgroundScroller : MonoBehaviour
{
    [SerializeField] private float speedMultiplier = 0.3f;
    [SerializeField] private bool useGlobalSpeed = true;

    private Renderer rend;
    private Vector2 originalOffset;

    void Awake()
    {
        rend = GetComponent<Renderer>();
        originalOffset = rend.material.mainTextureOffset;
    }

    void Update()
    {
        float spd = useGlobalSpeed ? WorldScroller.GlobalScrollSpeed : 6f;
        float x = Mathf.Repeat(Time.time * spd * speedMultiplier, 1f);
        rend.material.mainTextureOffset = new Vector2(x, originalOffset.y);
    }
}