using UnityEngine;
using UnityEngine.Analytics;

public class SetTheTimer : MonoBehaviour
{
    NoAdsHandler _noAdsHandler;
    public void TimerConnection()
    {
        if (Application.installMode == ApplicationInstallMode.Store)
            Analytics.CustomEvent("noAdsButton");
        if (GemManager.gemAmount >= 10)
        {
            if (SoundManager.Instance != null)
                SoundManager.Instance.PlayCombo();
            _noAdsHandler.SetNoAdsTimer();
        }
        else
        {
            if (SoundManager.Instance != null)
                SoundManager.Instance.PlayMiss();
        }
    }

    void Start()
    {
        DisableIfPurchased();
        _noAdsHandler = FindObjectOfType<NoAdsHandler>();
    }

    public void DisableIfPurchased()
    {
        if (PlayerPrefs.GetString("ShowAds", "true") == "false")
            this.gameObject.SetActive(false);
    }
}