using UnityEngine;

public class Shms3 : MonoBehaviour
{
    [Header("Movement Settings")]
    public float speed = 1f;
    private float direction = 1f;
    private bool isRunning = false;
    private bool faceright = true;

    [Header("UI References")]
    public GameObject LoseButton; 

    Rigidbody2D rb;
    Animator animator;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        Time.timeScale = 1f;
    }

    void Update()
    {
        if (DialogueManager.dialogueActive) return;

        if (Input.GetKeyDown(KeyCode.B)) ClickToStart();
        if (Input.GetKeyDown(KeyCode.Y) || Input.GetKeyDown(KeyCode.X)) ClickToStop();

        if (animator != null) animator.SetBool("ShamsIsWalking", isRunning);

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
            float xVal = direction * speed * 100 * Time.fixedDeltaTime;
            rb.linearVelocity = new Vector2(xVal, rb.linearVelocity.y);
        }
        else
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
        }
    }

    public void ClickToStart() => isRunning = true;
    public void ClickToStop() => isRunning = false;

    void Flip()
    {
        faceright = !faceright;
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }

    // --- الطريقة الأسهل باستخدام التاق ---
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isRunning) return;

        // "Wall" هنا هي التاق الذي ستضعه على الجدران أو الزر الأحمر
        if (collision.gameObject.CompareTag("Wall"))
        {
            direction *= -1f; // اعكس الاتجاه فوراً
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("DeadZone") || other.gameObject.layer == LayerMask.NameToLayer("Trap"))
        {
            Die();
        }
    }

    void Die()
    {
        if (LoseButton != null) LoseButton.SetActive(true);
        isRunning = false;
        rb.linearVelocity = Vector2.zero;
        Time.timeScale = 0f;
    }
}