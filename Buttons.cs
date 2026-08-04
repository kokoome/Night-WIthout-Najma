using UnityEngine;
using UnityEngine.SceneManagement;

public class Buttons : MonoBehaviour
{
    // 🔹 Call this on your "Play" button
    public void LoadScene(string sceneName)
    {
        // Make sure the scene is added in Build Settings
        SceneManager.LoadScene(sceneName);
    }

    // 🔹 Call this on your "Exit" button
    public void QuitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false; // Stop in editor
        #else
            Application.Quit(); // Quit the game in build
        #endif
    }
}