using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class WinFlag : MonoBehaviour
{
    public enum FlagOwner
    {
        Qamar,
        Shams
    }

    [Header("Flag Settings")]
    public FlagOwner flagOwner;

    [Header("Win Popup")]
    public GameObject winPopup;
    public float delayBeforeNextScene = 3f;

    // --- NEW: إعدادات الصوت ---
    [Header("Audio Settings")]
    public AudioSource audioSource;    // اسحب هنا أي AudioSource
    public AudioClip touchSound;      // صوت لمس قمر للعلم
    public AudioClip winSound;        // صوت الفوز النهائي الكبير
    // -------------------------

    private static bool qamarTouchedFirst = false;
    private static bool levelWon = false;

    // تصفير القيم عند بداية المرحلة (مهم جداً لأن المتغيرات static)
    private void Start()
    {
        qamarTouchedFirst = false;
        levelWon = false;
        if (audioSource == null) audioSource = GetComponent<AudioSource>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (levelWon) return;

        // الخطوة 1: قمر تلمس علمها
        if (!qamarTouchedFirst)
        {
            if (flagOwner == FlagOwner.Qamar && other.CompareTag("Qamar"))
            {
                qamarTouchedFirst = true;
                LockPlayer(other.gameObject);
                
                // تشغيل صوت لمس العلم (تأكيدي)
                PlaySound(touchSound);
                
                Debug.Log("Qamar locked. Waiting for Shams.");
            }
            return;
        }

        // الخطوة 2: شمس يلمس علمه (الفوز النهائي)
        if (flagOwner == FlagOwner.Shams && other.CompareTag("Shams"))
        {
            levelWon = true;
            
            // تشغيل صوت الفوز النهائي
            PlaySound(winSound);
            
            StartCoroutine(WinSequence());
        }
    }

    // دالة مساعدة لتشغيل الصوت
    void PlaySound(AudioClip clip)
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

        // نستخدم Realtime لأن الوقت متوقف
        yield return new WaitForSecondsRealtime(delayBeforeNextScene);

        Time.timeScale = 1f;
        LoadNextScene();
    }

    void LockPlayer(GameObject player)
    {
        MonoBehaviour movement = player.GetComponent<MonoBehaviour>();
        if (movement != null)
            movement.enabled = false;

        Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.bodyType = RigidbodyType2D.Static;
        }

        Animator anim = player.GetComponent<Animator>();
        if (anim != null)
            anim.enabled = false;
    }

    void LoadNextScene()
    {
        int index = SceneManager.GetActiveScene().buildIndex;
        // تأكد أن هناك مشهد تالي في Build Settings
        if (index + 1 < SceneManager.sceneCountInBuildSettings)
            SceneManager.LoadScene(index + 1);
        else
            Debug.Log("لا يوجد مشهد تالي!");
    }
}