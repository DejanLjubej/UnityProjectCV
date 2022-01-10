using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Analytics;
using UnityEngine.Advertisements;

public class PlayerStats : MonoBehaviour
{
    [SerializeField] int numberOfBalls=3;

    public int currentNumberOfBalls { get; private set; }
    public int currentNumberOfBallsLeft { get; private set; }
    public bool haveWon
    {
        get;  set;
    }

    public bool haveLost
    {
        get;  set;
    }

    public static string currentLevelName;
    public static int currentLevelNumber;

    public int extraBalls;

    int _thisSceneHighScore;
    int _moneyAfterHS;
    int _moneyFinal;
    int _moneyBefore;


    //UI
    public GameObject gameOverUI;
    public GameObject gameWonUI;
    public GameObject gamePausedUI;
    public GameObject popupPanel;

    public GameObject[] miscGameObjectsToDeactivate;
    public GameObject[] miscGameObjectsToMove; 

    OnLossUnityInterstitial lossInterstitial;

    [SerializeField] LevelWon lw;

    Animator victoryAnimator;
    Animator defeatAnimator;

    Dictionary<string, object> mainParametersTosendWithAnalytics;

    void Awake()
    {
        haveWon = false;
        haveLost = false;
        popupPanel.SetActive(true);
        currentLevelName = SceneManager.GetActiveScene().name;
        int.TryParse(currentLevelName.Replace("Level", ""), out currentLevelNumber);
        _thisSceneHighScore = PlayerPrefs.GetInt(currentLevelName + "HS", 0);

        lossInterstitial = GetComponent<OnLossUnityInterstitial>();

        currentNumberOfBalls = numberOfBalls;

        Time.timeScale = 1;
        Points.hitsInARow = 0;
        if (lw == null)
        {
            lw = FindObjectOfType<LevelWon>();
        }
        currentNumberOfBallsLeft = currentNumberOfBalls;


        StartCoroutine(ActivatePanels());
        victoryAnimator = gameWonUI.GetComponent<Animator>();
        defeatAnimator = gameOverUI.GetComponent<Animator>();

        mainParametersTosendWithAnalytics = new Dictionary<string, object>() { 
            {"timeSinceStart", Time.unscaledTime}
        };
        if(Application.installMode == ApplicationInstallMode.Store)
            AnalyticsEvent.LevelStart(currentLevelName, mainParametersTosendWithAnalytics);
            

    }
    
    IEnumerator ActivatePanels()
    {
        yield return new WaitForSeconds(0.5f);
        gamePausedUI.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        gameWonUI.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        gameOverUI.SetActive(true);
    }
    public void UseBalls(int ballsUsed)
    {
        currentNumberOfBalls -= ballsUsed;
    }
    public void BallStopped()
    {
        currentNumberOfBallsLeft -= 1;
    }


    public void MoveMiscObjects()
    {
        foreach (var item in miscGameObjectsToMove)
        {
            if(item!=null)
                item.transform.position += new Vector3(2000, 0);
        }
    }
    public void DeactivateMiscObjects()
    {
        foreach (var item in miscGameObjectsToDeactivate)
        {
            if(item != null)
            item.SetActive(false);
        }
    }

    public void ActivateMiscObjects()
    {
        foreach (var item in miscGameObjectsToDeactivate)
        {
            if (item != null)
                item.SetActive(true);
        }
    }
    public void GameOver() 
    {
        if (!haveWon)
        {
            haveLost = true;
            BallContorller.canYouStartAiming = false;
            if(SoundManager.Instance !=null)
                SoundManager.Instance.PlayDefeatSound();
            
            StartCoroutine(SpeedupandStopTime(false));
    
            lossInterstitial.ShowInterstitialOnLoss();
            if (Application.installMode == ApplicationInstallMode.Store)
                AnalyticsEvent.LevelFail(currentLevelName, mainParametersTosendWithAnalytics);
        }
    }


    public void GameWon()
    {
        if (!haveLost)
        {
            
            haveWon = true;
            BallContorller.canYouStartAiming = false;
            StartCoroutine(SpeedupandStopTime(true));
            

           
            lw.HandleLevelWon();
            _moneyBefore = PlayerPrefs.GetInt("PlayerCurrentMoney", 0);

            if (_thisSceneHighScore <= LevelScore.score)
            {
                _moneyAfterHS = LevelScore.score - _thisSceneHighScore;
                _moneyFinal = _moneyBefore + _moneyAfterHS;
            }
            else
            {
                _moneyFinal = _moneyBefore;
            }

            PlayerPrefs.SetInt("PlayerCurrentMoney", _moneyFinal);
            mainParametersTosendWithAnalytics.Add("BallsLeft", currentNumberOfBalls);
            if (Application.installMode == ApplicationInstallMode.Store)
            {
                AnalyticsEvent.LevelComplete(currentLevelName, mainParametersTosendWithAnalytics);
                Analytics.CustomEvent(currentLevelName+" Completed");
            }
        }
    }
    public void addBalls()
    {
        currentNumberOfBalls += extraBalls;
        currentNumberOfBallsLeft += 1;
    }

    public void GotGemsAnimationEffect(){
        
        victoryAnimator.SetTrigger("GotThreeGemsTrigger");
        SoundManager.Instance.PlayGotThreeGems();
    }

    IEnumerator SpeedupandStopTime(bool didWinCallThis)
    {
        MoveMiscObjects();
        DeactivateMiscObjects();
        FindObjectOfType<BallContorller>().ThingsToDoOnRelease();
        if (didWinCallThis)
        {
            Time.timeScale = 4;
            yield return new WaitForSecondsRealtime(1.5f);
           
            Time.timeScale = 1;
                gameWonUI.transform.localPosition = new Vector3(0, 0, 0);
            victoryAnimator.SetTrigger("ShowFirstAnimation");
            yield return new WaitForSeconds(2f);
            victoryAnimator.SetTrigger("ShowSecondAnimation");
        }
        else
        {
            Time.timeScale = 2;
            yield return new WaitForSeconds(1f);
            Time.timeScale = 1;
            if(!haveWon)
            gameOverUI.transform.localPosition = new Vector3(0, 0, 0);
            defeatAnimator.SetTrigger("StartFirstAnimation");

            yield return new WaitForSeconds(2f);
            defeatAnimator.SetTrigger("StartSecondAnimation");
        }
    }

    void OnApplicationQuit()
    {
        if (Application.installMode == ApplicationInstallMode.Store)
            Analytics.CustomEvent("quitWithing"+currentLevelName, mainParametersTosendWithAnalytics);
    }

    void OnApplicationPause(bool wasPaused)
    {
        if (Application.installMode == ApplicationInstallMode.Store)
        {
            if (wasPaused)
            {

                Analytics.CustomEvent("gameWasMinimized");
            }
            else
            {
                Analytics.CustomEvent("gameWasMaximized");
            }
        }
    }
    public void ShowWin()
    {
        StartCoroutine(SpeedupandStopTime(true));
    }
}
