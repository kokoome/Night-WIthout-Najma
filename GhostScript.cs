using UnityEngine;

using UnityEngine.SceneManagement; // مهم جداً لإعادة المرحلة



public class GhostController : MonoBehaviour

{

    [Header("Movement Settings")]

    public float speed = 1f;

    public float fadeSpeed = 2f;



    [Header("Game Over UI")]

    public GameObject gameOverCanvas; // اسحب الـ Canvas أو صورة الزر هنا



    Rigidbody2D rb;

    SpriteRenderer sr;



    private float direction = 1f;

    private bool faceright = true;

    private bool isRunning = false;



    private bool isFadingOut = false;

    private bool isFadingIn = false;



    private void Awake()

    {

        rb = GetComponent<Rigidbody2D>();

        sr = GetComponent<SpriteRenderer>();

       

        // تأكد أن واجهة الخسارة مخفية في البداية

        if (gameOverCanvas != null) gameOverCanvas.SetActive(false);

    }



    private void Update()

    {

        if (Input.GetKeyDown(KeyCode.B)) ClickToStart();

        if (Input.GetKeyDown(KeyCode.Y) || Input.GetKeyDown(KeyCode.X)) ClickToStop();



        if (isRunning)

        {

            if (direction > 0 && !faceright) Flip();

            else if (direction < 0 && faceright) Flip();

        }



        HandleFade();

    }



    private void FixedUpdate()

    {

        if (isRunning)

        {

            Move(direction);

        }

        else

        {

            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);

        }

    }



    void Move(float dir)

    {

        float xVal = dir * speed * 100 * Time.fixedDeltaTime;

        rb.linearVelocity = new Vector2(xVal, rb.linearVelocity.y);

    }



    public void ClickToStart() { isRunning = true; StartFadeIn(); }

    public void ClickToStop() { isRunning = false; StartFadeIn(); }

    public void HideGhost() { isRunning = false; StartFadeOut(); }



    void Flip()

    {

        faceright = !faceright;

        Vector3 localScale = transform.localScale;

        localScale.x *= -1;

        transform.localScale = localScale;

    }



    private void HandleFade()

    {

        if (isFadingOut)

        {

            Color c = sr.color;

            c.a -= fadeSpeed * Time.deltaTime;

            if (c.a <= 0f) { c.a = 0f; isFadingOut = false; }

            sr.color = c;

        }



        if (isFadingIn)

        {

            Color c = sr.color;

            c.a += fadeSpeed * Time.deltaTime;

            if (c.a >= 1f) { c.a = 1f; isFadingIn = false; }

            sr.color = c;

        }

    }



    private void StartFadeOut() { isFadingOut = true; isFadingIn = false; }

    private void StartFadeIn() { isFadingIn = true; isFadingOut = false; sr.enabled = true; }



    // --- الجزء الجديد الخاص بالخسارة ---

    private void OnCollisionEnter2D(Collision2D collision)

    {

        // 1. منطق الارتداد الأصلي (إذا لمس جدار)

        foreach (ContactPoint2D contact in collision.contacts)

        {

            if (Mathf.Abs(contact.normal.x) > 0.8f)

            {

                direction *= -1f;

                break;

            }

        }



        // 2. منطق الخسارة (إذا لمس اللاعب أو القفص)

        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Cage"))

        {

            GameOver();

        }

    }



    void GameOver()

    {

        isRunning = false; // توقف الشبح

        rb.linearVelocity = Vector2.zero;

       

        if (gameOverCanvas != null)

        {

            gameOverCanvas.SetActive(true); // إظهار زر الإعادة

        }

       

        // اختيارياً: إبطاء الوقت لإعطاء شعور بالخسارة

        Time.timeScale = 0f;

    }



    // دالة يتم استدعاؤها عند الضغط على زر الإعادة في الـ UI

    public void RestartLevel()

    {

        Time.timeScale = 1f; // إعادة الوقت لطبيعته

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    }

}