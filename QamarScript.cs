using UnityEngine;

using UnityEngine.SceneManagement;



public class QamarScript : MonoBehaviour

{

    public float speed = 1;

    public GameObject LoseButton;



    // --- NEW: متغيرات الصوت ---

    [Header("Audio Settings")]

    public AudioSource audioSource;

    public AudioClip footstepSound;

    [Range(0.5f, 2.0f)] public float walkPitch = 1.3f; // يمكنك التحكم في سرعة الصوت من هنا

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



        // نضمن وجود AudioSource

        if (audioSource == null) audioSource = GetComponent<AudioSource>();



        Time.timeScale = 1f;

        if (LoseButton != null) LoseButton.SetActive(false);

    }



    void Update()

    {

        if (DialogueManager.dialogueActive)

            return;

       

        if (Input.GetKeyDown(KeyCode.X)) ClickToStart();



        if (Input.GetKeyDown(KeyCode.B) || Input.GetKeyDown(KeyCode.Y))

        {

            ClickToStop();

        }



        if (animator != null)

        {

            animator.SetBool("QamarIsWalking", isRunning);

            bool falling = rb.linearVelocity.y < -0.1f;

            animator.SetBool("isFalling", falling);

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

            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);

        }

    }



    // --- NEW: دالة تشغيل الصوت لقمر ---

    public void PlayFootstep()

    {

        if (isRunning && footstepSound != null && audioSource != null)

        {

            // نستخدم القيمة التي حددتها في walkPitch لتسريع الصوت

            audioSource.pitch = walkPitch + Random.Range(-0.1f, 0.1f);

            audioSource.PlayOneShot(footstepSound);

        }

    }



    void Move(float dir)

    {

        float xVal = dir * speed * 100 * Time.fixedDeltaTime;

        rb.linearVelocity = new Vector2(xVal, rb.linearVelocity.y);

    }



    public void ClickToStart() => isRunning = true;

    public void ClickToStop() => isRunning = false;



    public void RestartLevel()

    {

        Time.timeScale = 1f;

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

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