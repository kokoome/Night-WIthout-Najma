using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections;

public class RedButton : MonoBehaviour
{
    [Header("Settings")]
    public Tilemap[] barrierTilemaps; 
    public Color pressedColor = Color.green; 
    public Color normalColor = Color.white;  
    public float fadeSpeed = 2f; 

    [Header("Audio Settings")]
    public AudioSource audioSource; // اسحب مكون الـ Audio Source هنا
    public AudioClip pressSound;    // اسحب ملف الصوت هنا

    private SpriteRenderer buttonRenderer;
    private Coroutine fadeCoroutine; 

    void Start()
    {
        buttonRenderer = GetComponent<SpriteRenderer>();
        
        // محاولة جلب الـ AudioSource تلقائياً إذا نسينا سحبه في المفتش
        if (audioSource == null) audioSource = GetComponent<AudioSource>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // --- إضافة تشغيل الصوت ---
            if (audioSource != null && pressSound != null)
            {
                audioSource.PlayOneShot(pressSound);
            }
            // ------------------------

            if (buttonRenderer != null) buttonRenderer.color = pressedColor;

            if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);
            fadeCoroutine = StartCoroutine(FadeAllTilemaps(0f)); 
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (buttonRenderer != null) buttonRenderer.color = normalColor;

            if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);
            fadeCoroutine = StartCoroutine(FadeAllTilemaps(1f)); 
        }
    }

    IEnumerator FadeAllTilemaps(float targetAlpha)
    {
        if (targetAlpha > 0)
        {
            foreach (var tm in barrierTilemaps) tm.gameObject.SetActive(true);
        }

        bool allDone = false;
        while (!allDone)
        {
            allDone = true; 
            
            foreach (var tm in barrierTilemaps)
            {
                Color c = tm.color;
                if (!Mathf.Approximately(c.a, targetAlpha))
                {
                    c.a = Mathf.MoveTowards(c.a, targetAlpha, Time.deltaTime * fadeSpeed);
                    tm.color = c;
                    allDone = false; 
                }
            }
            yield return null;
        }

        foreach (var tm in barrierTilemaps)
        {
            if (tm.GetComponent<Collider2D>() != null)
            {
                tm.GetComponent<Collider2D>().enabled = (targetAlpha > 0.5f);
            }

            if (targetAlpha <= 0) tm.gameObject.SetActive(false);
        }
    }
}
