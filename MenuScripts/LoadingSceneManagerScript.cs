using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Analytics;
using GooglePlayGames;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI;
using UnityEngine.Advertisements;

public class LoadingSceneManagerScript : MonoBehaviour
{
    bool _hasAccepted = false;
    bool _gotDailyReward = true;


    int _dailyRewardAmount = 0;
    int _day = 0;
    int _month = 0;
    int _year = 0;

    Animator termsAndPolicyAnimator;

    [Header("Links")]
    [SerializeField] string _termsUri = "";
    [SerializeField] string _privacyUri = "";

    [Header("GameObjects")]
    [SerializeField] GameObject termsAndPrivacyBox;

    [SerializeField] GameObject rewardBox;
    [SerializeField] GameObject[] covers;

 private string gameId = "4061363";


    void LogInToPlayServices()
    {
        PlayGamesPlatform.Instance.Authenticate(GooglePlayGames.BasicApi.SignInInteractivity.NoPrompt, (result) =>
        {
        });
    }

    /// <summary>
    /// Button Actions - Accept, go to terms, go to privacy policy
    /// </summary>
    public void AcceptAndPlay()
    {
        if (Application.installMode == ApplicationInstallMode.Store)
            Analytics.CustomEvent("privacyAccepted");
        _hasAccepted = true;
        PlayerPrefs.SetInt("HasAcceptedTermsAndPrivacyPolica", 1);
        if(termsAndPolicyAnimator != null)
        {
            termsAndPolicyAnimator.SetTrigger("HideTermsAndPrivacy");
        }
    }

    public void GoToTermsAndAgreement()
    {
        Application.OpenURL(_termsUri);
    }

    public void GoToPrivacyPolicy()
    {
        Application.OpenURL(_privacyUri);
    }

   void DailyRewardHandler()
    {
        _dailyRewardAmount = PlayerPrefs.GetInt("DailyRewardAmount", 1);
        _day = PlayerPrefs.GetInt("DayOfReward", 0);
        _month = PlayerPrefs.GetInt("MonthOfReward", 0);
        _year = PlayerPrefs.GetInt("YearOfReward", 0);
        
        if(!((DateTime.Now.Day == _day) && (DateTime.Now.Month == _month) && (DateTime.Now.Year == _year)))
        {
            if (Application.installMode == ApplicationInstallMode.Store)
                Analytics.CustomEvent("RewardGained", new Dictionary<string, object>() { { "DailyRewardAmount",_dailyRewardAmount} });

            _day = DateTime.Now.Day;
            _month = DateTime.Now.Month;
            _year = DateTime.Now.Year;
            PlayerPrefs.SetInt("DayOfReward", _day);
            PlayerPrefs.SetInt("MonthOfReward", _month);
            PlayerPrefs.SetInt("YearOfReward", _year);

            int gemAmount= PlayerPrefs.GetInt("GemAmount", 0);
            gemAmount += _dailyRewardAmount;
            PlayerPrefs.SetInt("GemAmount", gemAmount);

            if(_dailyRewardAmount > 0)
            {
            }
                rewardBox.SetActive(true);

                covers[_dailyRewardAmount-1].
                    GetComponent<Animator>().
                        SetTrigger("ShowReward");

            if (_dailyRewardAmount < 7)
                _dailyRewardAmount += 1;
            else
                _dailyRewardAmount = 1;

            PlayerPrefs.SetInt("DailyRewardAmount", _dailyRewardAmount);
        }
    }
    IEnumerator LoadMenu()
    {
        if (!_hasAccepted)
        {
            yield return new WaitForSeconds(1f);
            termsAndPrivacyBox.SetActive(true);
        }

        yield return new WaitUntil(()=>_hasAccepted);
        
        yield return new WaitForSeconds(2f);
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Menu");
        Resources.UnloadUnusedAssets();
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }

    void Start()
    {
        if (PlayerPrefs.GetInt("HasAcceptedTermsAndPrivacyPolica", 0) == 1)
        {
            _hasAccepted = true;
        }
        else
        {
            termsAndPolicyAnimator = termsAndPrivacyBox.GetComponent<Animator>();
        }
        StartCoroutine(LoadMenu());
        DailyRewardHandler();
        LogInToPlayServices(); 
        Advertisement.Initialize(gameId);
    }
}
