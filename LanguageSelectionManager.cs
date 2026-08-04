using UnityEngine;
using UnityEngine.SceneManagement;

public class LanguageSelectionManager : MonoBehaviour
{
    private string selectedTextLang = "EN";
    private string selectedVoiceLang = "EN";

    public void SetTextLanguage(string lang)
    {
        selectedTextLang = lang;
        Debug.Log("Text language selected: " + lang);
    }

    public void SetVoiceLanguage(string lang)
    {
        selectedVoiceLang = lang;
        Debug.Log("Voice language selected: " + lang);
    }

    public void ConfirmSelection()
    {
        LocalizationManager.Instance.SetLanguage(selectedTextLang, selectedVoiceLang);
        LocalizationManager.Instance.SaveLanguage();

        SceneManager.UnloadSceneAsync("LanguageSelection");
        SceneManager.LoadScene("StarterMenu", LoadSceneMode.Additive);
    }
    public void SkipToNextScene() /// œ«·… «·”ﬂÌ» ··”Ì‰ «··Ì »⁄œÂ - Õ”» «· — Ì» »ÌﬂÊ‰ «·”Ì‰ ÂÊ Õﬁ «·StarterMenu
    {
        SceneManager.UnloadSceneAsync("LanguageSelection");
        SceneManager.LoadScene("StarterMenu", LoadSceneMode.Additive);
    }
}
