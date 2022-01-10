using UnityEngine;
using UnityEngine.Advertisements;
using System.Collections;
public class LevelWinPoint : MonoBehaviour
{
    public int NumberOfTimesBeforeAd = 5;

    public static int numberOfTimesWon = 0;

    private int _adjustmentForBallNumber;
    bool wasHit = false;

    PlayerStats playerStats;
    Animator endpointAnimator;

    [SerializeField] GameObject signature;
    void OnCollisionEnter2D(Collision2D col)
    {
        if (!wasHit)
        {
            StartCoroutine(WidenTheTrail());

            if (SoundManager.Instance != null)
                SoundManager.Instance.PlayVictorySound();
            wasHit = true;

            endpointAnimator.SetTrigger("HideEndpoint");
            signature.SetActive(true);
            LevelScore.baseScore = LevelScore.score;
            LevelScore.bonusScore = _adjustmentForBallNumber * playerStats.currentNumberOfBalls;
            int _scoreBeforeComparingWithMaxScore = LevelScore.score + (LevelScore.bonusScore);
            LevelScore.score = _scoreBeforeComparingWithMaxScore;
            playerStats.GameWon();
            ShowInterstitialOnWin();

        }
    }


    IEnumerator WidenTheTrail()
    {
        int ballIndex = 0;
        yield return new WaitForEndOfFrame();
        GameObject[] playersBalls = GameObject.FindGameObjectsWithTag("Ball");
        BallMotor[] ballMotorInstances = new BallMotor[playersBalls.Length];

        for(int i =0; i<playersBalls.Length; i++){
            ballMotorInstances[i]= playersBalls[i].GetComponent<BallMotor>();
        }

        ExplosionBallParticleMotor[] explosionBallMotor = FindObjectsOfType<ExplosionBallParticleMotor>();
 
        foreach (var item in ballMotorInstances)
        {
            ballIndex++;
            if(ballIndex == ballMotorInstances.Length)
                yield return new WaitForSecondsRealtime(1f);
                       yield return new WaitForSecondsRealtime(0.1f);
            ballMotorInstances[ballMotorInstances.Length-ballIndex].widenTheTrail(ballIndex);
        }
        foreach (var item in explosionBallMotor)
        {
            item.widenTheTrail();
            yield return new WaitForSecondsRealtime(0.1f);
        }

    }
    void Start()
    {
        signature.SetActive(false);
        playerStats = GameObject.FindGameObjectWithTag("GameController").GetComponent<PlayerStats>();
        _adjustmentForBallNumber = LevelScore.requiredPointsForPublicUse/3;
        endpointAnimator = gameObject.GetComponent<Animator>();
    }

    public void ShowInterstitialOnWin()
    {
        if (numberOfTimesWon >= NumberOfTimesBeforeAd)
        {
            if (NoAdsHandler.noAdsTimer <= 0)
            {
                Advertisement.Show("InterstitialOnWin");
                numberOfTimesWon = 0;
            }
        }
        else
        {
            int numberToAdd = Random.Range(1, 3);
            numberOfTimesWon += numberToAdd;
        }
    }
}
