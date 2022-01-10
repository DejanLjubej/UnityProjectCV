using UnityEngine;
using System.Collections;
public class BallMotor : MonoBehaviour
{
    PlayerStats _playerStats;

    Vector2 _direction;
    float _force;
    bool _lastbounce = false;

    [SerializeField] float _ballDivider = 2;
    [SerializeField] int _numberOfBounces = 5;
    [SerializeField] ParticleSystem particleSplash;
    [SerializeField] ParticleSystem bonusSplash;
    [SerializeField] ParticleSystem continuousParticles;

    [SerializeField] GameObject[] splash;
    TrailRenderer _trailRenderer;
    BallContorller _ballController;

    void Start()
    {
        _ballController = FindObjectOfType<BallContorller>();
        _trailRenderer = gameObject.GetComponentInChildren<TrailRenderer>();
        _trailRenderer.widthMultiplier = 1;

        _force = SpawnBall.power;
        _direction = SpawnBall.moveDirection;
        this.GetComponent<Rigidbody2D>().AddForce(_direction * _force);

        _playerStats = GameObject.FindGameObjectWithTag("GameController").GetComponent<PlayerStats>();
        _trailRenderer.sharedMaterials = _ballController.ballTrailMaterials;

        _numberOfBounces = PlayerPrefs.GetInt("NumberOfBounces", 4);
        float ballSize = PlayerPrefs.GetFloat("BallSize", 5);
        this.transform.localScale = new Vector3(ballSize, ballSize, ballSize);
    }

    void OnCollisionEnter2D(Collision2D col)
    {

        this.transform.localScale = this.transform.localScale / _ballDivider;
        ControllSplashesAndSounds(col);
        StartCoroutine(HandleLastBallLater());
    }

    public void ControllSplashesAndSounds(Collision2D collider)
    {
        if (particleSplash != null)
        {
            particleSplash.Play();
        }

        if (collider.gameObject.tag != "Obstacle")
        {
            Points.hitsInARow = 0;
            if (SoundManager.Instance != null)
                SoundManager.Instance.PlayMiss();
        }
        else if (Points.hitsInARow > 0)
        {
            if (bonusSplash != null)
                bonusSplash.Play();
        }
        else
        {
            Points.hitsInARow = 0;
        }
            RemoveBallBounce();
    }

    IEnumerator HandleLastBallLater()
    {
        yield return new WaitForEndOfFrame();
        LastBallHandler();
            continuousParticles.startSize = 0.8f+0.4f*Points.hitsInARow;
        bonusSplash.startSize =  Points.hitsInARow ;
    }
    
    public void LastBallHandler()
    {
        if (_numberOfBounces <= 0)
        {
            BallContorller.canYouStartAiming = true;
            continuousParticles.Stop();
            this.gameObject.GetComponent<Rigidbody2D>().simulated = false;
            _playerStats.BallStopped();
            if (_playerStats.currentNumberOfBalls <= 0)
            {
                _playerStats.GameOver();
            }
        }
    }
    public void widenTheTrail(int ballIndex)
    {
        float trailWidthAdjustmentForBallPlace = ballIndex;
        StartCoroutine(WideningTheTrail(trailWidthAdjustmentForBallPlace));
    }

    IEnumerator WideningTheTrail(float trailAdjustment)
    {
        if(trailAdjustment>5)
        trailAdjustment = 5;

        float lowerRange =27-trailAdjustment*5;
        float higherRange = 37-trailAdjustment*7;
        yield return new WaitUntil(() => !this.gameObject.GetComponent<Rigidbody2D>().simulated);
        float maxWidth = Random.Range(lowerRange,higherRange);
        float timer = 0;
        while (true)
        {

            timer += Time.deltaTime;
            _trailRenderer.widthMultiplier = Mathf.Lerp(1f, maxWidth, timer);
            if (timer > 2f)
            {
                yield break;
            }
            yield return null;
        }
    }

    void IncreaseSize(int increaseMultiplier)
    {
        for (int i = 0; i < increaseMultiplier; i++)
        {
            this.transform.localScale = this.transform.localScale * 1.3f;
        }
    }
    void DecreaseSize(int decreseWeight)
    {
        for (int i = 0; i < decreseWeight; i++)
        {
            this.transform.localScale = this.transform.localScale / _ballDivider;
        }
    }
    public void RemoveBallBounce()
    {
        _numberOfBounces--;
    }
    public void AddBallBounces(int multiplier)
    {
        _numberOfBounces+=multiplier;
        IncreaseSize(multiplier);
    }

    public void RemoveBallBounce(int weight)
    {
        _numberOfBounces -= weight;
        DecreaseSize(weight);

    }
    public void AddForceAndTorque(Vector2 directionOfPointer, float torque, float force)
    {
        this.GetComponent<Rigidbody2D>().AddForce(directionOfPointer * force);
        this.GetComponent<Rigidbody2D>().AddTorque(torque, ForceMode2D.Impulse);
    }
}
