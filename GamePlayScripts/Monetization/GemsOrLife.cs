using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Analytics;
using System.Collections.Generic;

public class GemsOrLife : MonoBehaviour
{
    [SerializeField] UnityAdImplementation adImplementation;

    [SerializeField] GameObject optionToWatchAdOrSpendGems;
    [SerializeField] int gemsToSpend;

    GemManager _gemManager;

    Dictionary<string, object> analyticsDataToSend;

    string buttonClicked;
    bool watchAd;


    public void WatchAd()
    {
        if (SoundManager.Instance != null)
            SoundManager.Instance.PlayCombo();
        watchAd = true;
        CallTheCorrectFunctionForReward();
        if (Application.installMode == ApplicationInstallMode.Store)
            Analytics.CustomEvent("watchedAdForReward", analyticsDataToSend);
    }

    public void SpendGems()
    {
        if (Application.installMode == ApplicationInstallMode.Store)
            Analytics.CustomEvent("triedToSpendGemsForReward", analyticsDataToSend);
        if(GemManager.gemAmount>= gemsToSpend)
        {
            if (Application.installMode == ApplicationInstallMode.Store)
                Analytics.CustomEvent("spentGemsForReward", analyticsDataToSend);
            if (SoundManager.Instance != null)
                SoundManager.Instance.PlayCombo();
            watchAd = false;
            GemManager.gemAmount -= gemsToSpend;
            _gemManager.UpdateGemAmountText();
            CallTheCorrectFunctionForReward();
        }
    }

    public void AdButton(string action)
    {
        if(SoundManager.Instance!=null)
            SoundManager.Instance.PlayCombo();
        optionToWatchAdOrSpendGems.SetActive(true);
        buttonClicked = action;
        if (Application.installMode == ApplicationInstallMode.Store)
            Analytics.CustomEvent("wentForReward", analyticsDataToSend);
    }

    void CallTheCorrectFunctionForReward()
    {
        switch (buttonClicked)
        {
            case "ExtraBAllOnPause": adImplementation.HandleExtraBallOnPause(watchAd); break;
            case "ExtraBAllOnGameLost": adImplementation.HandleExtraBallOnLoss(watchAd); break;
            case "SkipLevel": adImplementation.SkipLevelOnLoss(watchAd); break;
            case "BoostScore": adImplementation.BoostScoreOnWin(watchAd); break;
            default: break;
        }
        optionToWatchAdOrSpendGems.SetActive(false);
        watchAd = false;
    }

    public void ClosePopUp()
    {
        optionToWatchAdOrSpendGems.SetActive(false);
    }
    void Start()
    {
        watchAd = false;
        _gemManager = FindObjectOfType<GemManager>();
        optionToWatchAdOrSpendGems.SetActive(false);

        analyticsDataToSend = new Dictionary<string, object>() { 
            { "LevelName", SceneManager.GetActiveScene().name }
        };
    }
}
