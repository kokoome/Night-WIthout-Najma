using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class NPCTransition : MonoBehaviour
{
    [Header("Settings")]
    public int nextSceneIndex = 5;  
    public CanvasGroup fadeImage;   
    public float fadeSpeed = 5f;    // زدنا السرعة الافتراضية هنا ليكون الـ Fade خاطف

    private bool isTransitioning = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isTransitioning)
        {
            // نوقف حركة اللاعب فوراً (اختياري لضمان عدم استمراره بالمشي)
            var playerRb = other.GetComponent<Rigidbody2D>();
            if (playerRb != null) playerRb.linearVelocity = Vector2.zero;

            StartCoroutine(StartFadeAndLoad());
        }
    }

    IEnumerator StartFadeAndLoad()
    {
        isTransitioning = true;

        if (fadeImage != null)
        {
            fadeImage.gameObject.SetActive(true);
            
            // حلقة سريعة جداً لتسويد الشاشة
            while (fadeImage.alpha < 1)
            {
                fadeImage.alpha += Time.deltaTime * fadeSpeed;
                yield return null; 
            }
        }

        // حذفنا الانتظار الطويل (0.5) وضعنا انتظاراً بسيطاً جداً للراحة البصرية
        yield return new WaitForSeconds(0.1f);

        SceneManager.LoadScene(nextSceneIndex);
    }
}
