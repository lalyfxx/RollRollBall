using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButtons : MonoBehaviour
{
    // ====================== MENU PRINCIPAL ======================
    
    public void PlayGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Game");        // Change "Game" si ta scène s'appelle autrement
    }

    public void QuitGame()
    {
        Debug.Log("Quitter le jeu...");
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    // ====================== GAME OVER ======================
    
    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Game");
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}