using UnityEngine;
using UnityEngine.UI;

public class BlackScreenFade : MonoBehaviour
{
    public float fadeSpeed = 1f; // speed of fade
    private Image img;
    private CanvasGroup cg;

    private void Awake()
    {
        img = GetComponent<Image>();

        // add CanvasGroup if it doesn't exist
        cg = GetComponent<CanvasGroup>();
        if (cg == null)
            cg = gameObject.AddComponent<CanvasGroup>();

        // start fully black
        Color c = img.color;
        c.a = 1f;
        img.color = c;

        cg.blocksRaycasts = true; // block input while fading
    }

    private void Update()
    {
        FadeOut();
    }

    private void FadeOut()
    {
        Color c = img.color;
        c.a -= fadeSpeed * Time.deltaTime;
        img.color = c;

        if (c.a <= 0f)
        {
            c.a = 0f;
            img.color = c;

            Destroy(gameObject); // 🔥 يمحيه بالكامل من المشهد
        }
    }
}