using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Analytics;

public class CrossPromoManager : MonoBehaviour
{
    private static CrossPromoManager _instance;

    string _sceneName = "Menu";
    Dictionary<string, object> mainParametersTosendWithAnalytics;

    public static CrossPromoManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<CrossPromoManager>();

                DontDestroyOnLoad(_instance.gameObject);
            }
            return _instance;
        }
    }

    public void ShowCrossPromoPopupAutomatic()
    {
        gameObject.transform.localPosition = new Vector2(0, 0);
    }

    public void ShowCrossPromoPopup()
    {
        gameObject.transform.localPosition = new Vector2(0, 0);
        if(Application.installMode == ApplicationInstallMode.Store)
            Analytics.CustomEvent("Cross Promo Opened");
    }

    public void HideCrossPromoPopup()
    {
        gameObject.transform.localPosition = new Vector2(0, 1000);
        if(Application.installMode == ApplicationInstallMode.Store)
            Analytics.CustomEvent("Cross Promo Closed");

    }

    public void OpenPromotedSubjectGooglePlay(string pathtoopen)
    {
        Application.OpenURL(pathtoopen);
        if(Application.installMode == ApplicationInstallMode.Store)
        {
            string nameTodisplayInAnalytics = pathtoopen.Replace("https://play.google.com/store/apps/details?id=com.", "");
            Analytics.CustomEvent("Promotion Clicked name: " + nameTodisplayInAnalytics);
        }
    }

    private void ChangedActiveScene(Scene current, Scene next)
    {
        _sceneName = next.name;

        if (next.name == "FinalScene")
            ShowCrossPromoPopupAutomatic();
    }
    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            if (this != _instance)
            {
                Destroy(this.gameObject);
            }
        }
        SceneManager.activeSceneChanged += ChangedActiveScene;
    }
}
