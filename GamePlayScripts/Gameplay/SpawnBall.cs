using System.Collections.Generic;
using UnityEngine;

public class SpawnBall : MonoBehaviour
{
    public List<GameObject> BallList = new List<GameObject>();
    public GameObject ballCounter;
    public Transform startingPoint;

    public static GameObject ball;

    public static Vector2 moveDirection;
    public static float spin;
    public static float power;
   
    PlayerStats playerStats;
    BallCounter bc;

    GameObject currentBall;
    TrailRenderer trailRenderer;
    public int trailLayerOrder = 6;

    void Start()
    {
        bc = ballCounter.GetComponent<BallCounter>();
        playerStats = GameObject.FindGameObjectWithTag("GameController").GetComponent<PlayerStats>();
        ball = BallList[ChooseThisColor.chosenColorNumber];
        trailRenderer = ball.GetComponentInChildren<TrailRenderer>();
        trailRenderer.sortingOrder = trailLayerOrder;
        currentBall = ball;
    }

    public void ChangeChosenColor()
    {
        ball = BallList[ChooseThisColor.chosenColorNumber];
        if (currentBall != ball)
        {
            trailLayerOrder += 1;
            trailRenderer = ball.GetComponentInChildren<TrailRenderer>();
            trailRenderer.sortingOrder = trailLayerOrder;
            currentBall = ball;
        }
    }

    public void SpawnTheBall(Vector2 directionOfPointer, float torque, float force)
    {
        if (ball != null)
        {
            if (playerStats.currentNumberOfBalls > 0)
            {
                Vector2 ballDirection = -directionOfPointer;
                Quaternion rotation = Quaternion.LookRotation(Vector3.forward, ballDirection);
                Rigidbody2D rb = ball.GetComponent<Rigidbody2D>();

                moveDirection = ballDirection;
                spin = torque;
                power = force;
                
                rb.simulated =true;
                ball.transform.rotation = rotation;
                ball.transform.position = startingPoint.position;

                Instantiate(ball); 
                playerStats.UseBalls(1);
            }
        }
        bc.UpdateBallCount();
    }
}
