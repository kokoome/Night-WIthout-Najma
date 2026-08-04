using UnityEngine;

public class BGscript : MonoBehaviour
{
    public Animator anim;

    private int currentState = -1;

    void Start()
    {
        if (anim == null)
            anim = GetComponent<Animator>();
    }

    void Update()
    {
                        if (DialogueManager.dialogueActive)
    return;
        if (Input.GetKeyDown(KeyCode.B)) SetTimeState(0);
        if (Input.GetKeyDown(KeyCode.Y)) SetTimeState(1);
        if (Input.GetKeyDown(KeyCode.X)) SetTimeState(2);
    }

    void SetTimeState(int newState)
    {
        if (currentState == newState) return;

        currentState = newState;
        anim.SetInteger("TimeState", currentState);
    }

    public void BGshams()  => SetTimeState(0);
    public void BGsunset() => SetTimeState(1);
    public void BGqamar()  => SetTimeState(2);
}