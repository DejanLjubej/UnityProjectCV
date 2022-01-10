using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.Analytics;
using System.Collections.Generic;
using System.Collections;

public class MenuButtons : MonoBehaviour
{
    [SerializeField] GameObject settingsPanel;
    IDictionary<string, object> analyticsDictionary;

    LoadingScreen _loadingScreen;
    CrossPromoManager _crossPromotionManager;
    void Start()
    {
        _loadingScreen = FindObjectOfType<LoadingScreen>();
        _crossPromotionManager = FindObjectOfType<CrossPromoManager>();

        analyticsDictionary = new Dictionary<string, object>() { 
            { "numberOfSessions", AnalyticsSessionInfo.sessionCount }, 
            {"sceneName", SceneManager.GetActiveScene().name },
            {"playtimeAt", Time.unscaledTime}
        };
        if (Application.installMode == ApplicationInstallMode.Store)
            Analytics.CustomEvent("gameStarted", analyticsDictionary);
    }

    public void PlayButton()
    {
        if (SoundManager.Instance != null)
            SoundManager.Instance.PlayButtonClick();
        StartCoroutine(LoadScene("LevelSelect"));
        if (Application.installMode == ApplicationInstallMode.Store)
            Analytics.CustomEvent("playButton", analyticsDictionary);
    }
    public void UpgradesButton()
    {
        if (SoundManager.Instance != null)
            SoundManager.Instance.PlayButtonClick();
        StartCoroutine(LoadScene("UpgradeSelect"));
        if (Application.installMode == ApplicationInstallMode.Store)
            Analytics.CustomEvent("upgradeButton", analyticsDictionary);
    }
    public void ExitButton()
    {
        if (SoundManager.Instance != null)
            SoundManager.Instance.PlayButtonClick();
        Application.Quit();
        if (Application.installMode == ApplicationInstallMode.Store)
            Analytics.CustomEvent("quitButton", analyticsDictionary);
    }
    public void MenuButton()
    {
        if (SoundManager.Instance != null)
            SoundManager.Instance.PlayButtonClick();
        StartCoroutine(LoadScene("Menu"));
        if (Application.installMode == ApplicationInstallMode.Store)
            Analytics.CustomEvent("menuButton", analyticsDictionary);
    }
    IEnumerator LoadScene(string sceneName)
    {
        if (_loadingScreen != null)
            _loadingScreen.CLoseTheCurtain();
        yield return new WaitForSecondsRealtime(1.15f);
        SceneManager.LoadScene(sceneName);

    }

    public void OpenSettingsButton()
    {
        if (SoundManager.Instance != null)
            SoundManager.Instance.PlayButtonClick();
        if (settingsPanel.activeSelf == false)
        {
           settingsPanel.SetActive(true);
        }
        if (Application.installMode == ApplicationInstallMode.Store)
            Analytics.CustomEvent("openSettingsButton");

    }

    public void CloseSettingsButton()
    {
        if (SoundManager.Instance != null)
            SoundManager.Instance.PlayButtonClick();
        if (settingsPanel.activeSelf)
            settingsPanel.SetActive(false);
        if (Application.installMode == ApplicationInstallMode.Store)
            Analytics.CustomEvent("closeSettingsButton");

    }

    public void OpenCrosPromotion()
    {
        _crossPromotionManager.ShowCrossPromoPopup();
    }

    public void ReserPrefs()
    {
        PlayerPrefs.DeleteAll();
    }

    void OnApplicationQuit()
    {
        if (Application.installMode == ApplicationInstallMode.Store)
            Analytics.CustomEvent("gameClosedWithoutButton");
    }

    void OnApplicationPause()
    {
        if (Application.installMode == ApplicationInstallMode.Store)
            Analytics.CustomEvent("gameWasMinimized");
    }
}
