using System.Collections;
using UnityEngine;

public class LevelScore : MonoBehaviour
{

    public static int score;
    public static int bonusScore;
    public static int baseScore;
    public static int requiredPointsForPublicUse;

    public int pointsNeededToFinishLevel;

    Collider2D endCollider;
    SpriteRenderer endSpriteRenderer;
    Animator endpointAnimator;

    public GameObject endPoint;
    public GameObject obsticles;

    bool wasAnimationActivated = false;

    void Start()
    {
        requiredPointsForPublicUse = pointsNeededToFinishLevel;
        endpointAnimator = endPoint.GetComponent<Animator>();
        endCollider = endPoint.GetComponent<Collider2D>();
        endCollider.enabled = false;
    }

    void Update()
    {
        if(score >= pointsNeededToFinishLevel && !wasAnimationActivated)
        {
            if(SoundManager.Instance !=null)
                SoundManager.Instance.PlayEnoughPointsSound();
            wasAnimationActivated = true;
            endpointAnimator.SetTrigger("ActivateEndpoint");
            endCollider.enabled = true;
        }
    }

}
