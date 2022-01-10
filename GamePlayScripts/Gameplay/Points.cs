using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Points : MonoBehaviour
{
    public static int hitsInARow = 0;

    int bonusPointNumber;

    TextMeshPro textMeshP;
    SpriteRenderer objectSpriteRenderer;
    SpriteRenderer boucerObjectSpriteRenderer;
    Collider2D obstaclesCollider;
    Animator obstacleAnimator;

    Color thisColor;
    Color ballColor;
    Color startColor;

    PaintExplosion _paintExplosion;
    BallMotor statsOfBallThatHit;
    ScoreCounter _scoreCounter;

    GameObject ballThatHit;

    public int pointsGainedByCollisionOnThisObject = 1;
    [Header("Buffs")]
    public bool isObstacleDeafault = true;
    public bool isObstacleBouncer = false;
    public bool isObstacleGiver = false;
    public bool isObstacleExplosion = false;
    public int bounceAddition = 3;

    [Header("Debuffs")]
    public bool isObstacleTaker = false;
    public bool isObstacleMover = false;
    public bool isObstacleBounceDimmer = false;
    public float moveDistance = 1;
    public int bounceRemoval = 3;
    public bool moveVertically = false;

    [Header("Buff Objects")]
    [SerializeField] GameObject bouncerObstacle;
    [SerializeField] GameObject giverObstacle;
    [SerializeField] GameObject explosionObstacle;
    [SerializeField] PhysicsMaterial2D bouncingMaterial;

    [Header("Debuff Objects")]
    [SerializeField] GameObject bounceTaker;
    public GameObject scorePopup;

    void OnCollisionEnter2D(Collision2D col)
    {
        thisColor =objectSpriteRenderer.color;
        if ("Ball" == col.gameObject.tag || "ExplosionBall" == col.gameObject.tag)
        {
            ballThatHit = col.gameObject;
            if("Ball" == col.gameObject.tag)
            {
                statsOfBallThatHit = ballThatHit.GetComponent<BallMotor>();
                TrigeerAnEffect();
                ChangeObstacleToDefault();
            }


            ballColor = ballThatHit.GetComponent<SpriteRenderer>().color;

            if (thisColor != ballColor)
            {

                textMeshP = scorePopup.GetComponent<TextMeshPro>();
                textMeshP.color = ballColor;
                textMeshP.fontSize = 36;
                textMeshP.text = pointsGainedByCollisionOnThisObject.ToString();

                startColor = thisColor;

                Instantiate(scorePopup, transform.position, Quaternion.identity);

                if (hitsInARow > 0)
                {
                    bonusPointNumber = hitsInARow * (int)(pointsGainedByCollisionOnThisObject/2);

                    textMeshP.fontSize = 50;
                    textMeshP.color = startColor;
                    textMeshP.text = "+"+bonusPointNumber.ToString();

                    StartCoroutine(WaitABitWithExtraPoints());

                    if("Ball" == col.gameObject.tag)
                    DetermineTheObstacleType();

                    if(SoundManager.Instance != null)
                        SoundManager.Instance.PlayCombo();
                }
                else
                {
                    if(SoundManager.Instance != null)
                        SoundManager.Instance.PlayBasic();
                }

                boucerObjectSpriteRenderer.color = ballColor;

                LevelScore.score += pointsGainedByCollisionOnThisObject + bonusPointNumber;
                ScoreCounter.UpdateBounceCount();
                _scoreCounter.UpdateProgressBar();

                objectSpriteRenderer.color = ballColor;
                hitsInARow += 1;
            }
            else
            {
                hitsInARow = 0;
                if(SoundManager.Instance != null)
                SoundManager.Instance.PlayMiss();
            }

        }
    }

    void TrigeerAnEffect()
    {
        if (isObstacleExplosion)
        {
            TriggerTheExplosionIfLegible();
        }else if (isObstacleGiver)
        {
            AddBouncesIfLegible();
        }else if (isObstacleTaker)
        {
            RemoveBouncesIfLegible();
        }
    }

    void TriggerTheExplosionIfLegible()
    {
        if (null != _paintExplosion)
            _paintExplosion.TriggerExplosion();
    }

    void AddBouncesIfLegible()
    {
        if (statsOfBallThatHit != null)
            statsOfBallThatHit.AddBallBounces(bounceAddition);
    }

    void RemoveBouncesIfLegible()
    {
        if (statsOfBallThatHit != null)
            statsOfBallThatHit.RemoveBallBounce(bounceRemoval);
    }

    void DetermineTheObstacleType()
    {
        switch (hitsInARow)
        {
            case 1: ChangeObstacleToBouncer(); 
                break;
            case 2: ChangeObstacleToGiver();
                break;
            default: ChangeObstacleToExplosion();
                break;
        }
    }


    void ChangeObstacleToDefault()
    {
        objectSpriteRenderer.enabled = true;
        isObstacleDeafault = true;
        isObstacleBouncer = false;
        isObstacleGiver = false;
        isObstacleExplosion = false;
        isObstacleMover = false;
        isObstacleTaker = false;

        bouncerObstacle.SetActive(false);
        giverObstacle.SetActive(false);
        explosionObstacle.SetActive(false);
        bounceTaker.SetActive(false);

        if(null != obstaclesCollider.sharedMaterial )
        {
            obstaclesCollider.sharedMaterial = null;
        }
    }

    void ChangeObstacleToBouncer()
    {
        ChangeObstacleToDefault();
        boucerObjectSpriteRenderer.color = ballColor;
        obstacleAnimator.SetTrigger("BiggerBounce");
        bouncerObstacle.SetActive(true);
        obstaclesCollider.sharedMaterial = bouncingMaterial;
        objectSpriteRenderer.enabled = false;
        isObstacleDeafault = false;
        isObstacleBouncer = true;
    }

    void ChangeObstacleToGiver()
    {
        ChangeObstacleToDefault();
        obstacleAnimator.SetTrigger("BonusBounce");
        giverObstacle.SetActive(true);

        isObstacleDeafault = false;
        isObstacleGiver = true;
    }

    void ChangeObstacleToExplosion()
    {
        ChangeObstacleToDefault();
        obstacleAnimator.SetTrigger("Explosion");
        explosionObstacle.SetActive(true);

        isObstacleDeafault = false;
        isObstacleExplosion = true;
    }

    void ChangeObstacleToMover()
    {
        ChangeObstacleToDefault();

        isObstacleDeafault = false;
        isObstacleMover = true;
        StartCoroutine(MoveObject());
    }

    IEnumerator MoveObject()
    {
        float timeOfMove = 0;


        Vector3 startPosition;
        Vector3 positionOne;
        Vector3 positionTwo;
        startPosition = this.transform.localPosition;

        if (!moveVertically)
        {
            positionOne = new Vector3(startPosition.x - moveDistance, startPosition.y, startPosition.z);
            positionTwo = new Vector3(startPosition.x + moveDistance, startPosition.y, startPosition.z);
        }
        else
        {
            positionOne = new Vector3(startPosition.x, startPosition.y - moveDistance, startPosition.z);
            positionTwo = new Vector3(startPosition.x, startPosition.y + moveDistance, startPosition.z);
        }

        Vector3 positionOfStart = startPosition;
        Vector3 endPosition = positionOne;

        bool isStartMove = true;
        bool isLeft = false;

        while (true)
        {
            timeOfMove += Time.deltaTime/2;


            if (isStartMove)
            {
                this.gameObject.transform.position = Vector3.Lerp(positionOfStart, endPosition, timeOfMove *2);
                if (timeOfMove > 1)
                {
                    isStartMove = false;
                    isLeft = true;
                    timeOfMove = 0;
                }
            }else if (isLeft)
            {
                this.gameObject.transform.position = Vector3.Lerp(positionOne, positionTwo, timeOfMove);
                if (timeOfMove > 1)
                {
                    isLeft = false;
                    timeOfMove = 0;
                }
            }
            else if(!isLeft)
            {
                this.gameObject.transform.position = Vector3.Lerp(positionTwo, positionOne, timeOfMove);
                if (timeOfMove > 1)
                {
                    isLeft = true;
                    timeOfMove = 0;
                }
            }

            if (!isObstacleMover)
            {
                timeOfMove = 0;
                yield return new WaitUntil(() =>
                {
                    timeOfMove += Time.deltaTime;
                    this.gameObject.transform.position = Vector3.Lerp(this.transform.position, startPosition, timeOfMove);
                    if (timeOfMove > 1)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                });
                yield break;
            }
            yield return null;
        }
    }

    void ChangeObstacleToTaker()
    {
        ChangeObstacleToDefault();
        obstacleAnimator.SetTrigger("Spikes");
        bounceTaker.SetActive(true);

        isObstacleDeafault = false;
        isObstacleTaker = true;
    }
    void InitialObstacleTypeHandler()
    {
        if (isObstacleDeafault)
        {
            ChangeObstacleToDefault();
        }else if (isObstacleBouncer)
        {
            ChangeObstacleToBouncer();
        }else if (isObstacleGiver)
        {
            ChangeObstacleToGiver();
        }else if (isObstacleExplosion)
        {
            ChangeObstacleToExplosion();
        }else if (isObstacleMover)
        {
            ChangeObstacleToMover();
        }else if (isObstacleTaker)
        {
            ChangeObstacleToTaker();
        }
    }

    IEnumerator WaitABitWithExtraPoints()
    {
        yield return new WaitForSeconds(0.25f);
       
        Instantiate(scorePopup, transform.position, Quaternion.identity);
    }
   
    void Start()
    {
        StartCoroutine(DoThingABitLater());
    }

    IEnumerator DoThingABitLater()
    {
        yield return new WaitForSeconds(0.3f);

        obstaclesCollider = gameObject.GetComponent<Collider2D>();
        objectSpriteRenderer = this.gameObject.GetComponent<SpriteRenderer>();
        obstacleAnimator = gameObject.GetComponent<Animator>();
        boucerObjectSpriteRenderer = bouncerObstacle.GetComponent<SpriteRenderer>();
        if (explosionObstacle != null)
            _paintExplosion = GetComponentInChildren<PaintExplosion>(true);

        thisColor = objectSpriteRenderer.color;
        ballColor = thisColor;

        _scoreCounter = FindObjectOfType<ScoreCounter>();

        InitialObstacleTypeHandler();
    }
}
