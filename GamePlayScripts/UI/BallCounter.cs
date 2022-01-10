using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
public class BallCounter : MonoBehaviour
{
    static PlayerStats playerStats;

    public Image ballImage;
    public GridLayoutGroup grid;
    public GameObject lastBallIndicator;
    List<Image> balls;

    void Start()
    {
        balls = new List<Image>();
        playerStats = GameObject.FindGameObjectWithTag("GameController").GetComponent<PlayerStats>();
        int numberToDisplay = playerStats.currentNumberOfBalls;

        InstantiateTheBalls(numberToDisplay);
    }

    public void UpdateBallCount()
    {
        if(playerStats.currentNumberOfBalls < balls.Count)
        {
            DestroyInstantiatedBalls(playerStats.currentNumberOfBalls);
        }
        else if(playerStats.currentNumberOfBalls > balls.Count)
        {
            InstantiateTheBalls(playerStats.currentNumberOfBalls - balls.Count);
        }
        LastBallIndicatorHandler();
    }

    void InstantiateTheBalls(int n)
    {
        if (n > 0)
        {
            Image img = (Image)Instantiate(ballImage, grid.transform) as Image;
            balls.Add(img);
            InstantiateTheBalls(n - 1);
        }
    }
    void DestroyInstantiatedBalls(int d)
    {
        if (d < balls.Count)
        {
            Destroy(balls[d].gameObject);
            balls.RemoveAt(d);
            DestroyInstantiatedBalls(d + 1);
        }
    }

    void LastBallIndicatorHandler()
    {
        if (playerStats.currentNumberOfBalls == 1)
        {
            lastBallIndicator.SetActive(true);
            if (SoundManager.Instance != null)
                SoundManager.Instance.PlayLastBallSound();
        }
        else
        {
            lastBallIndicator.SetActive(false);
        }
    }
}
