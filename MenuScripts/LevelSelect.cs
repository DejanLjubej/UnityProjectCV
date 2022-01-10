using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Analytics;
using System.Collections.Generic;

public class LevelSelect : MonoBehaviour
{
    public Text scoreText;
    public Image lockImage;
    public Image colorOfVictory;

    public static bool loadingLevel = false;

    Dictionary<string, object> parametersToSendToAnalytics;

    string levelText;

    LoadingScreen _loadingScreen;

    void Start()
    {
        levelText = this.gameObject.name.Replace("(Clone)", "").Trim();

        string keyForHighScorePrefs = levelText + "HS";

        string temporaryTextString = levelText.Replace("Level","");
        int temporaryTextNumber=0;
        int.TryParse(temporaryTextString, out temporaryTextNumber);

        scoreText.text = PlayerPrefs.GetInt(keyForHighScorePrefs, 0).ToString();
        if(levelText == "Level1")
        {
            PlayerPrefs.SetInt(levelText+"Lock", 1);
        }
        if(PlayerPrefs.GetInt(levelText+"Lock") == 1)
        {
            lockImage.gameObject.SetActive(false);
            HandleColorOfVictory();
        }
        parametersToSendToAnalytics = new Dictionary<string, object>();
        parametersToSendToAnalytics.Add("LevelNumer", levelText);
        _loadingScreen = FindObjectOfType<LoadingScreen>();
    }
    public void PlayLevel()
    {
        if (PlayerPrefs.GetInt(levelText+"Lock", 0) ==1)
        {
            if(SoundManager.Instance != null)
            SoundManager.Instance.PlayColorPick();
            StartCoroutine(LoadThisLevel());
            _loadingScreen.CLoseTheCurtain();
        }
        if (Application.installMode == ApplicationInstallMode.Store)
            Analytics.CustomEvent("levelSelected", parametersToSendToAnalytics);
    }

    IEnumerator LoadThisLevel()
    {
        yield return new WaitForSeconds(1.14f);
            SceneManager.LoadScene(levelText);
        if(!SceneManager.GetSceneByName(levelText).IsValid())
            SceneManager.LoadScene("FinalScene");

    }
    static IEnumerator LevelByNameAsync(string name)
    {
        if(loadingLevel == false)
        {
            loadingLevel = true;
            AsyncOperation operation = SceneManager.LoadSceneAsync(name);
            operation.allowSceneActivation = false;

            while ((operation.progress / 0.9f) < 1f)
            {
                yield return null;
            }
            loadingLevel = false;
            operation.allowSceneActivation = true;
        }
    }
    void HandleColorOfVictory()
    {
        if(PlayerPrefs.GetInt(levelText+"HS", 0) > 0)
        {
            colorOfVictory.gameObject.SetActive(false);
        }
        else
        {
            colorOfVictory.gameObject.SetActive(true);
        }
    }
}
