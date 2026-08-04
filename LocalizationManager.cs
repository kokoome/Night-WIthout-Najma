using UnityEngine;

public class LocalizationManager : MonoBehaviour
{
    public static LocalizationManager Instance;

    public string CurrentLanguage = "EN";
    public string CurrentVoiceLanguage = "EN";

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetLanguage(string textLang, string voiceLang)
    {
        CurrentLanguage = textLang;
        CurrentVoiceLanguage = voiceLang;
    }

    public void SaveLanguage()
    {
        PlayerPrefs.SetString("TextLang", CurrentLanguage);
        PlayerPrefs.SetString("VoiceLang", CurrentVoiceLanguage);
        PlayerPrefs.Save();
    }

    public void LoadLanguage()
    {
        CurrentLanguage = PlayerPrefs.GetString("TextLang", "EN");
        CurrentVoiceLanguage = PlayerPrefs.GetString("VoiceLang", "EN");
    }
}