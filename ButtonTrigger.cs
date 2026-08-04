using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections;

public class ButtonTrigger : MonoBehaviour
{
    public Tilemap barrierTilemap; 
    public Color pressedColor = Color.green; 
    public Color normalColor = Color.white;  
    public float fadeSpeed = 2f; 

    // --- إضافات الصوت (عند الدخول فقط) ---
    public AudioSource audioSource; 
    public AudioClip pressSound;    
    // ------------------------------------

    private SpriteRenderer buttonRenderer;
    private Coroutine fadeCoroutine; 

    void Start()
    {
        buttonRenderer = GetComponent<SpriteRenderer>();
        
        // محاولة جلب الـ AudioSource تلقائياً إذا نسينا سحبه
        if (audioSource == null) audioSource = GetComponent<AudioSource>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // تشغيل الصوت هنا فقط
            if (audioSource != null && pressSound != null)
            {
                audioSource.PlayOneShot(pressSound);
            }

            if (buttonRenderer != null) buttonRenderer.color = pressedColor;

            if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);
            fadeCoroutine = StartCoroutine(FadeTilemap(0f)); 
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // هنا لا يوجد صوت، فقط نغير اللون ونعيد الحاجز
            if (buttonRenderer != null) buttonRenderer.color = normalColor;

            if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);
            fadeCoroutine = StartCoroutine(FadeTilemap(1f)); 
        }
    }

    IEnumerator FadeTilemap(float targetAlpha)
    {
        if (targetAlpha > 0) barrierTilemap.gameObject.SetActive(true);

        Color c = barrierTilemap.color;
        
        while (!Mathf.Approximately(c.a, targetAlpha))
        {
            c.a = Mathf.MoveTowards(c.a, targetAlpha, Time.deltaTime * fadeSpeed);
            barrierTilemap.color = c;
            yield return null;
        }

        if (barrierTilemap.GetComponent<Collider2D>() != null)
        {
            barrierTilemap.GetComponent<Collider2D>().enabled = (targetAlpha > 0.5f);
        }

        if (targetAlpha <= 0) barrierTilemap.gameObject.SetActive(false);
    }
}