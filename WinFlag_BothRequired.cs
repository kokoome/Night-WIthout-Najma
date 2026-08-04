using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class WinFlag_BothRequired : MonoBehaviour
{
    public enum FlagOwner
    {
        Qamar,
        Shams
    }

    [Header("Flag Settings")]
    public FlagOwner flagOwner;

    [Header("Popup")]
    public GameObject winPopup;
    public float delayBeforeNextScene = 3f;

    // --- NEW: إعدادات الصوت ---
    [Header("Audio Settings")]
    public AudioSource audioSource;    // اسحب هنا الـ AudioSource
    public AudioClip reachFlagSound;  // صوت لمس أي علم (صوت بسيط)
    public AudioClip levelWinSound;   // صوت الفوز النهائي (موسيقى فوز)
    // -------------------------

    private static bool qamarReached = false;
    private static bool shamsReached = false;
    private static bool levelWon = false;

    void Awake()
    {
        // Reset when scene loads
        qamarReached = false;
        shamsReached = false;
        levelWon = false;

        // محاولة إيجاد الـ AudioSource تلقائياً إذا لم يتم سحبه
        if (audioSource == null) audioSource = GetComponent<AudioSource>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (levelWon) return;

        // عندما تصل قمر لعلمها
        if (flagOwner == FlagOwner.Qamar && other.CompareTag("Qamar") && !qamarReached)
        {
            qamarReached = true;
            PlayOneShotSound(reachFlagSound); // تشغيل صوت وصول قمر
            LockPlayer(other.gameObject);
            CheckWinCondition();
        }

        // عندما يصل شمس لعلمه
        if (flagOwner == FlagOwner.Shams && other.CompareTag("Shams") && !shamsReached)
        {
            shamsReached = true;
            PlayOneShotSound(reachFlagSound); // تشغيل صوت وصول شمس
            LockPlayer(other.gameObject);
            CheckWinCondition();
        }
    }

    void CheckWinCondition()
    {
        if (qamarReached && shamsReached && !levelWon)
        {
            levelWon = true;
            PlayOneShotSound(levelWinSound); // تشغيل صوت الفوز الكبير
            StartCoroutine(WinSequence());
        }
    }

    // دالة مساعدة لتشغيل الأصوات
    void PlayOneShotSound(AudioClip clip)
    {
        if (clip != null && audioSource != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }

    IEnumerator WinSequence()
    {
        if (winPopup != null)
            winPopup.SetActive(true);

        Time.timeScale = 0f;

        yield return new WaitForSecondsRealtime(delayBeforeNextScene);

        Time.timeScale = 1f;

        int index = SceneManager.GetActiveScene().buildIndex;
        if (index + 1 < SceneManager.sceneCountInBuildSettings)
            SceneManager.LoadScene(index + 1);
        else
            Debug.Log("No more scenes!");
    }

    void LockPlayer(GameObject player)
    {
        Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.bodyType = RigidbodyType2D.Static;
        }

        Animator anim = player.GetComponent<Animator>();
        if (anim != null)
            anim.enabled = false;

        MonoBehaviour movement = player.GetComponent<MonoBehaviour>();
        if (movement != null)
            movement.enabled = false;
    }
}