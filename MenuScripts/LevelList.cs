using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class LevelList : MonoBehaviour
{

    public GridLayoutGroup gridGroup;
    public Button levelButton;

    Text levelNumber;
    int numberOfLevel;

    [SerializeField] int numberOfButtons = 0;
    [SerializeField] int positionOfNextLevel= 0;

    List<int> listOfLevels;

    void Start()
    {
        listOfLevels = new List<int>();
        numberOfButtons = SceneManager.sceneCountInBuildSettings-5;
        StartCoroutine(LazyLevelLoad());
    }

    IEnumerator LazyLevelLoad()
    {
        Text[] texts = levelButton.GetComponentsInChildren<Text>();

        foreach (Text text in texts)
        {
            if (text.name == "LevelNumber")
            {
                levelNumber = text;
            }
        }

        for (int i = 0; i < numberOfButtons*2; i++)
        {
            numberOfLevel = i;
            levelNumber.text = (numberOfLevel + 1).ToString();
            levelButton.name = "Level" + (numberOfLevel + 1).ToString();
            Button button = (Button) Instantiate(levelButton, transform);
            if (i % 2 == 0)
            {
                yield return new WaitForSecondsRealtime(0.01f);
            }
            listOfLevels.Add(PlayerPrefs.GetInt(levelButton.name + "Lock", 0));
        }

        float whichColumn = (listOfLevels.LastIndexOf(1) + 1) / 3.0f;
        float rounded = whichColumn - Mathf.RoundToInt(whichColumn);
        Vector3 newPosition;
        float positionXForLevels = -462.5f - 125 * Mathf.FloorToInt(whichColumn - positionOfNextLevel);
        float timeSinceStart = 0;
        newPosition = new Vector3(transform.localPosition.x, positionXForLevels, transform.localPosition.z);
        while (true)
        {
            timeSinceStart += Time.deltaTime * 3;
            transform.localPosition = Vector3.Lerp(transform.localPosition, newPosition, timeSinceStart);

            if (timeSinceStart > 0.35f)
            {
                yield break;
            }

            yield return null;
        }
    }
}
