using UnityEngine;

public class ShamsScript : MonoBehaviour
{
    public float speed = 1;
    public GameObject LoseButton;
    
    // --- NEW: متغيرات الصوت ---
    [Header("Audio Settings")]
    public AudioSource audioSource;
    public AudioClip footstepSound;
    // -------------------------

    Rigidbody2D rb;
    Animator animator;

    private float direction = 1f;
    private bool faceright = true;
    private bool isRunning = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        
        // تأكد من وجود AudioSource إذا نسيت إضافته يدوياً
        if (audioSource == null) audioSource = GetComponent<AudioSource>();
        
        Time.timeScale = 1f;
    }

    void Update()
    {
        if (DialogueManager.dialogueActive)
            return;

        if (Input.GetKeyDown(KeyCode.B)) ClickToStart();

        if (Input.GetKeyDown(KeyCode.Y) || Input.GetKeyDown(KeyCode.X))
        {
            ClickToStop();
        }

        if (animator != null)
        {
            animator.SetBool("ShamsIsWalking", isRunning);
            bool falling = rb.linearVelocity.y < -0.1f;
            animator.SetBool("IsFalling", falling);
        }

        if (isRunning)
        {
            if (direction > 0 && !faceright) Flip();
            else if (direction < 0 && faceright) Flip();
        }
    }

    private void FixedUpdate()
    {
        if (isRunning)
        {
            Move(direction);
        }
        else
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocityY);
        }
    }

    void Move(float dir)
    {
        float xVal = dir * speed * 100 * Time.fixedDeltaTime;
        rb.linearVelocity = new Vector2(xVal, rb.linearVelocityY);
    }

    // --- NEW: دالة تشغيل الصوت التي سنناديها من الأنميشن ---
public void PlayFootstep()
{
    if (isRunning && footstepSound != null && audioSource != null)
    {
        // 1.5 تعني سرعة مرة ونصف أسرع من الطبيعي
        audioSource.pitch = 1f; 
        audioSource.PlayOneShot(footstepSound);
    }
}
    // --------------------------------------------------

    public void ClickToStart()
    {
        isRunning = true;
    }

    public void ClickToStop()
    {
        isRunning = false;
    }

    void Flip()
    {
        faceright = !faceright;
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isRunning) return;

        foreach (ContactPoint2D contact in collision.contacts)
        {
            if ((direction > 0 && contact.point.x > transform.position.x) ||
                (direction < 0 && contact.point.x < transform.position.x))
            {
                direction *= -1f;
                break;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("DeadZone"))
        {
            Die();
        }

        if (other.gameObject.layer == LayerMask.NameToLayer("Trap"))
        {
            Die();
        }
    }

    void Die()
    {
        if (LoseButton != null)
            LoseButton.SetActive(true);

        isRunning = false;
        rb.linearVelocity = Vector2.zero;
        Time.timeScale = 0f;
    }
}