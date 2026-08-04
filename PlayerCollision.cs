using UnityEngine;

public class PlayerCollision : MonoBehaviour {
    GameManager gm;

    void Start() { 
        gm = FindFirstObjectByType<GameManager>(); 
    }

    void OnTriggerEnter2D(Collider2D other) {
        // 1. لمس العلم
        if (other.CompareTag("Goal1") && gameObject.CompareTag("Player1")) gm.UpdateStatus(1, true);
        if (other.CompareTag("Goal2") && gameObject.CompareTag("Player2")) gm.UpdateStatus(2, true);

        // 2. لمس الزر (فتح الجسر + تأثير الضغط)
        if (other.CompareTag("BridgeButton")) 
        {
            // إرسال (true) لفتح الجسر
            if (FindFirstObjectByType<BridgeMover>() != null)
                FindFirstObjectByType<BridgeMover>().ActivateBridge(true);

            // تغيير اللون للأخضر
            if (other.GetComponent<SpriteRenderer>() != null) {
                other.GetComponent<SpriteRenderer>().color = Color.green;
            }

            // إيحاء الضغط: تصغير حجم الزر قليلاً (كأنه نزل للأرض)
            other.transform.localScale = new Vector3(0.85f, 0.85f, 1f);
        }

        // 3. السقوط في الهاوية
        if (other.CompareTag("DeadZone")) gm.LoseGame();
    }

    void OnTriggerExit2D(Collider2D other) {
        // الخروج من منطقة العلم
        if (other.CompareTag("Goal1")) gm.UpdateStatus(1, false);
        if (other.CompareTag("Goal2")) gm.UpdateStatus(2, false);

        // 4. ترك الزر (إغلاق الجسر + إرجاع الشكل)
        if (other.CompareTag("BridgeButton")) 
        {
            // إرسال (false) ليعود الجسر لمكانه
            if (FindFirstObjectByType<BridgeMover>() != null)
                FindFirstObjectByType<BridgeMover>().ActivateBridge(false);

            // إعادة اللون للأبيض
            if (other.GetComponent<SpriteRenderer>() != null) {
                other.GetComponent<SpriteRenderer>().color = Color.white;
            }

            // إعادة الحجم الطبيعي للزر
            other.transform.localScale = new Vector3(1f, 1f, 1f);
        }
    }
}