using UnityEngine;

public class BridgeButton : MonoBehaviour
{
    [Header("Bridge Reference")]
    public Transform bridge;

    [Header("Button Visuals")]
    public Color pressedColor = Color.green; 
    private Color normalColor;               
    private SpriteRenderer buttonRenderer;   

    [Header("Movement Settings")]
    public float moveDistance = -3f;
    public float moveSpeed = 3f;

    [Header("Audio Settings")]
    public AudioSource audioSource; // اسحب مكون الـ Audio Source هنا
    public AudioClip pressSound;    // اسحب ملف الصوت هنا

    private Vector3 startPosition;
    private Vector3 targetPosition;
    private bool isPressed = false;

    void Start()
    {
        buttonRenderer = GetComponent<SpriteRenderer>();
        if (buttonRenderer != null)
        {
            normalColor = buttonRenderer.color; 
        }

        // محاولة جلب الـ AudioSource تلقائياً إذا كان على نفس الجسم
        if (audioSource == null) audioSource = GetComponent<AudioSource>();

        startPosition = bridge.position;
        targetPosition = startPosition + new Vector3(moveDistance, 0f, 0f);
    }

    void Update()
    {
        if (bridge == null) return;

        Vector3 destination = isPressed ? targetPosition : startPosition;

        bridge.position = Vector3.MoveTowards(
            bridge.position,
            destination,
            moveSpeed * Time.deltaTime
        );
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Shams"))
        {
            // --- تشغيل الصوت عند الضغط ---
            if (audioSource != null && pressSound != null && !isPressed)
            {
                audioSource.PlayOneShot(pressSound);
            }
            // ----------------------------

            isPressed = true;
            if (buttonRenderer != null) buttonRenderer.color = pressedColor;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Shams"))
        {
            isPressed = false;
            if (buttonRenderer != null) buttonRenderer.color = normalColor;
        }
    }
}
