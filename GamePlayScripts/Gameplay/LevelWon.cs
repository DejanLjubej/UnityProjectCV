using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro;
using GooglePlayGames;
public class LevelWon : MonoBehaviour
{
    public Text textOfScore;
    public static bool levelWon = false;

    bool paintingShown = false;
    [Header("Point Text")]
    [SerializeField] TMP_Text basePoints;
    [SerializeField] TMP_Text bonusPoints;
    [SerializeField] TMP_Text totalPoints;

    [Header("Reward Ad Section")]
    [SerializeField] GameObject getGemsAdBtnObject;
    [SerializeField] TMP_Text bounsCoinAmount;

    [Header("BackgroundControll")]
    [SerializeField] SpriteRenderer backgroundImage;
    [SerializeField] Image buttonColorImage;

    bool isBackgroundDark;

    void Start()
    {
        getGemsAdBtnObject.SetActive(false);
    }

    public void HandleLevelWon()
    {
        string thisSceneName = SceneManager.GetActiveScene().name;
        HighScore hs = GameObject.FindObjectOfType<HighScore>();

        int scoreDifference = LevelScore.score - ScoreCounter.highScoreForThisLevel;

        if (ScoreCounter.highScoreForThisLevel < LevelScore.score)
        {
            textOfScore.text = "New High Score: " + LevelScore.score.ToString();
            getGemsAdBtnObject.SetActive(true);

            int scoreToPostOnLeaderboard = PlayerPrefs.GetInt("TotalScoreLeaderboard", 0) + scoreDifference;
            PlayerPrefs.SetInt("TotalScoreLeaderboard", scoreToPostOnLeaderboard);

            if (PlayGamesPlatform.Instance.IsAuthenticated())
            {
                Social.ReportScore(scoreToPostOnLeaderboard, "CgkIxuKq4sMBEAIQAQ", (success) => {
                    Debug.Log($"Reporting to leaderboard was a {success}. Reported score of {scoreToPostOnLeaderboard}");
                });
            }
        }
        else
        {
            textOfScore.text = "Score: " + LevelScore.score.ToString();
            scoreDifference = 0;
        }

        basePoints.text = $"{LevelScore.baseScore}";
        bonusPoints.text = $"{LevelScore.bonusScore}";
        totalPoints.text = $"{scoreDifference}";

        bounsCoinAmount.text = $"{LevelScore.score} Bonus";

        string levelNumberString = thisSceneName.Replace("Level", "").Trim();
        int forCorrectFormatting = 0;
        int.TryParse(levelNumberString, out forCorrectFormatting);
        int levelNumber = forCorrectFormatting + 1;
        hs.SetHighscore(levelNumber - 1, LevelScore.score);
        string levelLock = "Level" + levelNumber + "Lock";
        PlayerPrefs.SetInt(levelLock, 1);
        levelWon = true;
    }


    public void BacgroundColorController(){

        if (!isBackgroundDark)
        {
            backgroundImage.color = new Color(0.22f, 0.22f, 0.22f);
            buttonColorImage.color = Color.white;
            isBackgroundDark = true;
        }
        else
        {
            backgroundImage.color = Color.white;
            buttonColorImage.color = Color.black;
            isBackgroundDark = false;
        }
    
    }
    public void ShowHidePainting()
    {
        if (!paintingShown)
            this.gameObject.transform.localPosition = new Vector3(0, 2000, 0);
        else
            this.gameObject.transform.localPosition = new Vector3(0,0,0);
        paintingShown = !paintingShown;
    }

    public void DisableRewardAfterClick()
    {
        getGemsAdBtnObject.SetActive(false);
    }
    IEnumerator LoadNextScene()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
        operation.allowSceneActivation = false;

        while ((operation.progress / 0.9f) < 1f)
        {
            yield return null;
        }
        operation.allowSceneActivation = true;
    }
}
