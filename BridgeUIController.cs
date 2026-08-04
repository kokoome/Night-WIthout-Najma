using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections;

public class BridgeController : MonoBehaviour
{
    public Tilemap ShamsBridge;    // جسر شمس (Tilemap)
    public Tilemap QamarBridge;    // جسر قمر (Tilemap)
    
    [Range(0.5f, 5f)]
    public float fadeSpeed = 2f;
    
    private Coroutine shamsFade;
    private Coroutine qamarFade;
    
    void Update()
    {
        // كيبورد
        if (Input.GetKeyDown(KeyCode.B))
        {
            Debug.Log("Keyboard B: تفعيل ShamsButton");
            ShamsButton();
        }
        
        if (Input.GetKeyDown(KeyCode.X))
        {
            Debug.Log("Keyboard X: تفعيل QamarButton");
            QamarButton();
        }
        
        if (Input.GetKeyDown(KeyCode.Y))
        {
            Debug.Log("Keyboard Y: تفعيل StopButton");
            StopButton();
        }
    }
    
    public void ShamsButton()
    {
        Debug.Log("ShamsButton - إخفاء شمس، إظهار قمر");
        
        if (shamsFade != null) StopCoroutine(shamsFade);
        if (qamarFade != null) StopCoroutine(qamarFade);
        
        shamsFade = StartCoroutine(FadeTilemap(ShamsBridge, false));
        qamarFade = StartCoroutine(FadeTilemap(QamarBridge, true));
    }
    
    public void QamarButton()
    {
        Debug.Log("QamarButton - إخفاء قمر، إظهار شمس");
        
        if (shamsFade != null) StopCoroutine(shamsFade);
        if (qamarFade != null) StopCoroutine(qamarFade);
        
        shamsFade = StartCoroutine(FadeTilemap(ShamsBridge, true));
        qamarFade = StartCoroutine(FadeTilemap(QamarBridge, false));
    }
    
    public void StopButton()
    {
        Debug.Log("StopButton - إظهار الجسرين");
        
        if (shamsFade != null) StopCoroutine(shamsFade);
        if (qamarFade != null) StopCoroutine(qamarFade);
        
        shamsFade = StartCoroutine(FadeTilemap(ShamsBridge, true));
        qamarFade = StartCoroutine(FadeTilemap(QamarBridge, true));
    }
    
    IEnumerator FadeTilemap(Tilemap tilemap, bool show)
    {
        if (tilemap == null) yield break;
        
        // نجيب الـ TilemapRenderer
        TilemapRenderer renderer = tilemap.GetComponent<TilemapRenderer>();
        
        if (renderer == null)
        {
            Debug.LogWarning(tilemap.name + " ما فيه TilemapRenderer!");
            
            // إذا ما فيه Renderer، نستخدم enabled
            tilemap.gameObject.SetActive(show);
            yield break;
        }
        
        Debug.Log(tilemap.name + " بدأ التلاشي " + (show ? "دخول" : "خروج"));
        
        // للأسف Tilemap ما يدعم الشفافية بسهولة،所以我们 بنستخدم طريقتين:
        
        // الطريقة 1: نغير لون الـ Tilemap (إذا كان يستخدم Sprite مع شفافية)
        float targetAlpha = show ? 1f : 0f;
        float startAlpha = 1f; // افتراضي
        
        // نحاول نغير لون التايلماب
        Color startColor = tilemap.color;
        startAlpha = startColor.a;
        
        float elapsedTime = 0f;
        float fadeDuration = 1f / fadeSpeed;
        
        // نتأكد إن في فرق
        if (Mathf.Approximately(startAlpha, targetAlpha))
        {
            Debug.Log(tilemap.name + " بالفعل في القيمة المطلوبة");
            yield break;
        }
        
        // نتأكد أن الجسر active
        if (!tilemap.gameObject.activeSelf)
            tilemap.gameObject.SetActive(true);
        
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / fadeDuration;
            float smoothT = Mathf.SmoothStep(0, 1, t);
            
            // غير لون التايلماب ككل
            Color newColor = tilemap.color;
            newColor.a = Mathf.Lerp(startAlpha, targetAlpha, smoothT);
            tilemap.color = newColor;
            
            yield return null;
        }
        
        // القيمة النهائية
        Color finalColor = tilemap.color;
        finalColor.a = targetAlpha;
        tilemap.color = finalColor;
        
        // إذا وصلنا للصفر، نخفي الجسر
        if (!show)
        {
            tilemap.gameObject.SetActive(false);
        }
        
        Debug.Log(tilemap.name + " اكتمل التلاشي إلى " + targetAlpha);
    }
}