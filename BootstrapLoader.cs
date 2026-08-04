using UnityEngine;
using UnityEngine.SceneManagement;

public class BootstrapLoader : MonoBehaviour
{
    void Start()
    {
        SceneManager.LoadScene("MasterUI", LoadSceneMode.Additive);

        if (PlayerPrefs.HasKey("TextLang"))
            SceneManager.LoadScene("IntroScene", LoadSceneMode.Additive);
        else
            SceneManager.LoadScene("LanguageSelection", LoadSceneMode.Additive);
    }
}