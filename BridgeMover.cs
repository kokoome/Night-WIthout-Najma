using UnityEngine;

public class BridgeMover : MonoBehaviour {
    public Vector3 targetPos; // اسحبي الجسر لمكان النهاية وانقلي الأرقام هنا
    private Vector3 startPos; // سيحفظ مكان البداية تلقائياً
    public float speed = 2f;
    private bool isOpen = false;

    void Start() {
        // يحفظ مكان الجسر أول ما تشغلين اللعبة
        startPos = transform.position; 
    }

    // تأكدي أن بين القوسين مكتوب (bool status)
    public void ActivateBridge(bool status) {
        isOpen = status; 
    }

    void Update() {
        // تحديد الوجهة: إذا isOpen صح يروح للنهاية، وإذا خطأ يرجع للبداية
        Vector3 destination = isOpen ? targetPos : startPos;
        
        // تحريك الجسر تدريجياً
        transform.position = Vector3.MoveTowards(transform.position, destination, speed * Time.deltaTime);
    }
}