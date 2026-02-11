using UnityEngine;

public class ParallaxLayer : MonoBehaviour
{
    [Header("Réglages")]
    [Tooltip("0 = bouge avec la caméra (fond très lointain), 1 = bouge normalement, >1 = bouge plus vite (premier plan)")]
    public float parallaxFactor = 0.3f;     // plus petit = plus loin

    [Tooltip("Vitesse de scroll de base si tu ne veux PAS suivre la caméra")]
    public float autoScrollSpeed = 0f;      // 0 = suit seulement la caméra

    [Header("Setup infini (ne pas toucher sauf si tu changes la taille)")]
    public bool infiniteHorizontal = true;

    private Transform cam;
    private Vector3 startPos;
    private float length;                   // largeur d'UNE image (en unités Unity)
    private float extraLength = 0.1f;       // petite marge pour éviter les micro-fissures

    void Start()
    {
        cam = Camera.main.transform;
        startPos = transform.position;

        // On prend la largeur du sprite (attention : doit être en unités Unity, pas pixels)
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            length = sr.bounds.size.x;
        }
        else
        {
            // Si plusieurs sprites enfants, on prend la largeur du parent ou on fixe manuellement
            length = 20f; // ← À TOI DE MESURER ton image dans l'éditeur
        }
    }

    void LateUpdate()   // LateUpdate → après le mouvement du joueur/camera
    {
        // 1. Mouvement parallax classique
        float distance = cam.position.x * parallaxFactor;
        float temp = cam.position.x * (1 - parallaxFactor);

        Vector3 targetPos = startPos + Vector3.right * distance;

        if (autoScrollSpeed != 0)
        {
            targetPos += Vector3.right * (Time.time * autoScrollSpeed);
        }

        transform.position = new Vector3(targetPos.x, startPos.y, startPos.z);

        // 2. Boucle infinie (seulement si infiniteHorizontal = true)
        if (infiniteHorizontal)
        {
            if (cam.position.x > transform.position.x + length)
            {
                transform.position += Vector3.right * (length + extraLength);
            }
            else if (cam.position.x < transform.position.x - length)
            {
                transform.position -= Vector3.right * (length + extraLength);
            }
        }
    }
}