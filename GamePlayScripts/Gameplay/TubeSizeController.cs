using UnityEngine;

public class TubeSizeController : MonoBehaviour
{
    public PlayerStats playerStats;
    public GameObject TubePartToScale;
    int numberOfBallsAtBeginning;
    int numberForBallNumberCheck;
    float startingTubeLength;
    float subtractorOfTubeSize;
    float ySize;
    float startingXPositionOfTubeToScale;

    // Start is called before the first frame update
    void Start()
    {
        playerStats.GetComponent<PlayerStats>();
        numberOfBallsAtBeginning =playerStats.currentNumberOfBalls;
        startingTubeLength = TubePartToScale.transform.localScale.x;
        subtractorOfTubeSize = (startingTubeLength - startingTubeLength * 0.15f)/numberOfBallsAtBeginning;
        ySize = TubePartToScale.transform.localScale.y;
        numberForBallNumberCheck = playerStats.currentNumberOfBalls;
        startingXPositionOfTubeToScale = TubePartToScale.transform.localPosition.x;
    }

    // Update is called once per frame
    void Update()
    {
        if (numberForBallNumberCheck != playerStats.currentNumberOfBalls)
        {
            float currentSubtractorOfTubeSize = subtractorOfTubeSize * (numberOfBallsAtBeginning - playerStats.currentNumberOfBalls);
            float xPositionOfTubeToScale = startingXPositionOfTubeToScale + currentSubtractorOfTubeSize*1.5f;
            TubePartToScale.transform.localScale = new Vector2(startingTubeLength - currentSubtractorOfTubeSize, ySize);
            TubePartToScale.transform.localPosition = new Vector2(xPositionOfTubeToScale, 0);
            numberForBallNumberCheck = playerStats.currentNumberOfBalls;
        }
    }
}
