using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
public class ScoreCounter : MonoBehaviour
{
    [SerializeField] TMP_Text _levelText;
    [SerializeField] Image mainProgressBar;
    [SerializeField] Image highScoreProgressBar;

    public static Text textOfScore;
    static LevelScore pointsNeeded;

    static string levelName;
    public static int highScoreForThisLevel;

    void Start()
    {
        pointsNeeded = FindObjectOfType<LevelScore>();
        
        string sceneFullName= SceneManager.GetActiveScene().name;
        levelName = sceneFullName + "HS";
        _levelText.text = sceneFullName.Replace("Level","Level ");
        textOfScore = this.GetComponent<Text>();
        highScoreForThisLevel = PlayerPrefs.GetInt(levelName,0);
        UpdateBounceCount();
        UpdateProgressBar();
    }

    public static void UpdateBounceCount()
    {
        textOfScore.text = $"SCORE: {LevelScore.score} / {pointsNeeded.pointsNeededToFinishLevel}      HS: {highScoreForThisLevel}";
    }

    public void UpdateProgressBar()
    {
        mainProgressBar.fillAmount = 0;
        highScoreProgressBar.fillAmount = 0;

        float mainProgressBarFillAmount = LevelScore.score * 1f / pointsNeeded.pointsNeededToFinishLevel;
        mainProgressBar.fillAmount = mainProgressBarFillAmount;

        if (highScoreForThisLevel != 0)
        {
            float highScoreProgressBarFillAmount = LevelScore.score * 1f / highScoreForThisLevel;
            highScoreProgressBar.fillAmount = highScoreProgressBarFillAmount;
        }
    }
}
