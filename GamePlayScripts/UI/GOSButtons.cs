using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;


public class GOSButtons : MonoBehaviour
{
    public LoadingScreen loadingScreen;

    public BallCounter bc;
    PlayerStats playerStats;

    int sceneIndex;
    string levelLock;
    string levelName;

    Dictionary<string, object> analyticsDataToSend;

    public void RetryButton()
    {
        if(SoundManager.Instance!= null)
            SoundManager.Instance.PlayButtonClick();
        StartCoroutine(ReloadThisScene());
        if(loadingScreen != null)
            loadingScreen.CLoseTheCurtain();
        if(Application.installMode == ApplicationInstallMode.Store)
            Analytics.CustomEvent("retryWasClicked", analyticsDataToSend);
    }
    IEnumerator ReloadThisScene()
    {
        Time.timeScale = 1;
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
    }
    public void LoadStore(){
        if(SoundManager.Instance!=null)
            SoundManager.Instance.PlayButtonClick();
        StartCoroutine(LoadNextScene(3));
    }
    public void NextLevelButton()
    {
        if (SoundManager.Instance != null)
            SoundManager.Instance.PlayButtonClick();
        sceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        int levelNumber = sceneIndex - 4;
        StartCoroutine(LoadNextScene(sceneIndex));
        levelLock = "Level" + levelNumber + "Lock";
        PlayerPrefs.SetInt(levelLock, 1);
        if (Application.installMode == ApplicationInstallMode.Store)
            Analytics.CustomEvent("nextLevelButtonWasClicked", analyticsDataToSend);
    }

    IEnumerator LoadNextScene(int sceneIndexx)
    {
        loadingScreen.CLoseTheCurtain();
        yield return new WaitForSecondsRealtime(1.15f);
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndexx, LoadSceneMode.Single);
        operation.allowSceneActivation = false;

        while ((operation.progress / 0.8f) < 1f)
        {
            yield return null;
        }
        operation.allowSceneActivation = true;
        yield return new WaitUntil(() => operation.isDone);
    }

    public void MenuButton()
    {
        if (SoundManager.Instance != null)
            SoundManager.Instance.PlayButtonClick();
        StartCoroutine(LoadNextScene(1));

        if (Application.installMode == ApplicationInstallMode.Store)
            AnalyticsEvent.LevelQuit(levelName, analyticsDataToSend);
    }

    public void GetMoreBalls()
    {
        if(SoundManager.Instance != null)
            SoundManager.Instance.PlayButtonClick();
        playerStats.gameOverUI.transform.localPosition = new Vector3(2000, 0, 0);
        playerStats.gamePausedUI.transform.localPosition = new Vector3(2000, 0, 0);
        Time.timeScale = 1;
        playerStats.addBalls();
        bc.UpdateBallCount();
    }
 
    void Start()
    {
        loadingScreen = FindObjectOfType<LoadingScreen>();
        playerStats = FindObjectOfType<PlayerStats>();
        levelName=SceneManager.GetActiveScene().name;
        analyticsDataToSend = new Dictionary<string, object>()
        {
            {"LevelName", levelName},
            {"Time", AnalyticsSessionInfo.sessionElapsedTime}
        };
    }
}
