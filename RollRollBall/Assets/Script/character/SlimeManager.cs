using UnityEngine;
using System.Collections.Generic;

public class SlimeManager : MonoBehaviour
{
    public static SlimeManager Instance { get; private set; }

    private List<GameObject> activeSlimes = new List<GameObject>();

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void RegisterSlime(GameObject slime)
    {
        if (slime != null && !activeSlimes.Contains(slime))
        {
            activeSlimes.Add(slime);
        }
    }

    public void UnregisterSlime(GameObject slime)
    {
        activeSlimes.Remove(slime);
    }

    public void MergeAllSlimes(GameObject survivor)
{
    if (survivor == null) return;

    // Détruire tous les slimes SAUF le survivor
    foreach (var slime in activeSlimes.ToArray())
    {
        if (slime != null && slime != survivor)
        {
            Destroy(slime);
        }
    }

    // Nettoyer la liste et ne garder que le survivor
    activeSlimes.Clear();
    activeSlimes.Add(survivor);

    Debug.Log("Fusion terminée → un seul slime restant à taille normale");
}
}