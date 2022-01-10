using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class RewardTimer : MonoBehaviour
{
    [SerializeField]TMP_Text _timerText;
    [SerializeField] Button _adButton;


    public bool adWasViewd { get; set; }
    int _dayDailyWasViewd;
    int _monthDailyWasViewd;
    int _yearDailyWasViewd;

    float timeToSubstract = 86400;
    TimeSpan interval;

    GameObject _getFreeGemsIndicator;

    public void DailyAdRewardHandler()
    {
        _dayDailyWasViewd = PlayerPrefs.GetInt("DayOfAdReward", 0);
        _monthDailyWasViewd = PlayerPrefs.GetInt("MonthOfAdReward", 0);
        _yearDailyWasViewd = PlayerPrefs.GetInt("YearOfAdReward", 0);

        if (!((DateTime.Now.Day == _dayDailyWasViewd) && (DateTime.Now.Month == _monthDailyWasViewd) && (DateTime.Now.Year == _yearDailyWasViewd)))
        {
            adWasViewd = false;
            _adButton.interactable = true;
            _timerText.text = "Free";
            if(_getFreeGemsIndicator != null)
                _getFreeGemsIndicator.SetActive(true);
        }else{
            if(_getFreeGemsIndicator != null)
                _getFreeGemsIndicator.SetActive(false);

            adWasViewd = true;
            _adButton.interactable = false;
        }
    }

        public void ViewDaily(){

            _adButton.interactable = false;

            _dayDailyWasViewd = DateTime.Now.Day;
            _monthDailyWasViewd = DateTime.Now.Month;
            _yearDailyWasViewd = DateTime.Now.Year;

            PlayerPrefs.SetInt("DayOfAdReward", _dayDailyWasViewd);
            PlayerPrefs.SetInt("MonthOfAdReward", _monthDailyWasViewd);
            PlayerPrefs.SetInt("YearOfAdReward", _yearDailyWasViewd);

            if(_getFreeGemsIndicator != null)
                _getFreeGemsIndicator.SetActive(false);
        }

    void FixedUpdate()
    {
        if (_adButton.interactable == false)
        {
            float hoursInminutes = DateTime.Now.Hour * 60;
            float minutsInMinutes = DateTime.Now.Minute;
            float totalMinutesOfDay = hoursInminutes + minutsInMinutes;
            timeToSubstract -= totalMinutesOfDay;
            interval = TimeSpan.FromMinutes(timeToSubstract);
            _timerText.text = string.Format("{0:00}:{1:00}", interval.Hours, interval.Minutes);
            timeToSubstract = 86400;
        }
    }
    private void ChangedActiveScene(Scene current, Scene next)
    {
        _getFreeGemsIndicator = GameObject.FindGameObjectWithTag("FreeGemsIndicator");
        if(_getFreeGemsIndicator != null){
            if(adWasViewd)
                _getFreeGemsIndicator.SetActive(false);
            else
                _getFreeGemsIndicator.SetActive(true);
        }
    }
    void Start()
    {
        _getFreeGemsIndicator = GameObject.FindGameObjectWithTag("FreeGemsIndicator");
        _getFreeGemsIndicator.SetActive(false);
        adWasViewd = false;
        SceneManager.activeSceneChanged += ChangedActiveScene;
        DailyAdRewardHandler();
    }

}
