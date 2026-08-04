using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; 
using System.Collections; 

public class LevelChanger : MonoBehaviour
{
    [Header("Settings")]
    public int nextSceneIndex = 4;
    public CanvasGroup fadeImage; 
    public float fadeSpeed = 1f;

    private bool isTransitioning = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        // إذا لمس اللاعب الجسم ولم نبدأ الانتقال بعد
        if (other.CompareTag("Player") && !isTransitioning)
        {
            // --- الإضافة هنا: تغيير لون الجسم الحالي للأخضر لحظة اللمس ---
            SpriteRenderer mySprite = GetComponent<SpriteRenderer>();
            if (mySprite != null)
            {
                mySprite.color = Color.green;
            }
            // -------------------------------------------------------

            StartCoroutine(FadeAndExit());
        }
    }

    IEnumerator FadeAndExit()
    {
        isTransitioning = true;

        // ابدأ بجعل الصورة السوداء تظهر تدريجياً
        if (fadeImage != null)
        {
            while (fadeImage.alpha < 1)
            {
                fadeImage.alpha += Time.deltaTime * fadeSpeed;
                yield return null;
            }
        }

        // انتظر ثانية بسيطة ثم انتقل للسين رقم 4
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene(nextSceneIndex);
    }
}
