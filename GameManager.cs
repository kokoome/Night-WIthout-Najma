using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    public GameObject winPanel;  // اسحبي لوحة الفوز هنا
    public GameObject losePanel; // اسحبي لوحة الخسارة هنا
    public bool p1In = false, p2In = false;

    void Start() { 
        winPanel.SetActive(false); 
        losePanel.SetActive(false); 
        Time.timeScale = 1f; 
    }

    public void UpdateStatus(int playerID, bool status) {
        // التعديل الجديد: نحدث الحالة فقط إذا دخل اللاعب (true)
        // ولا نلغيها إذا خرج (false)
        if (status == true) 
        {
            if (playerID == 1) p1In = true;
            if (playerID == 2) p2In = true;
        }

        // التحقق من فوز الطرفين
        if (p1In && p2In) { 
            winPanel.SetActive(true); 
            Time.timeScale = 0f; 
        }
    }

    public void LoseGame() { losePanel.SetActive(true); Time.timeScale = 0f; }
    public void RestartGame() { Time.timeScale = 1f; SceneManager.LoadScene(SceneManager.GetActiveScene().name); }
    public void NextLevel() { Time.timeScale = 1f; SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); }
}