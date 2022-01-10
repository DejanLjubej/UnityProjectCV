using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionBallParticleMotor : MonoBehaviour
{
    PlayerStats _playerStats;

    Vector2 _direction;
    float _force;

    bool _lastbounce = false;

    [SerializeField] float _ballDivider = 2;
    [SerializeField] int _numberOfBounces = 5;
    [SerializeField] ParticleSystem particleSplash;
    [SerializeField] ParticleSystem bonusSplash;
    [SerializeField] Material thirdMaterial;
    [SerializeField] Texture thirdTexture;

    TrailRenderer trailRenderer;
    BallContorller _ballController;
    void Start()
    {
        _ballController = FindObjectOfType<BallContorller>();
        trailRenderer = gameObject.GetComponentInChildren<TrailRenderer>();
        trailRenderer.widthMultiplier = 1;
        trailRenderer.sharedMaterials = _ballController.ballTrailMaterials;
    }

    public void widenTheTrail()
    {
        StartCoroutine(WideningTheTrail());
    }

    IEnumerator WideningTheTrail()
    {
        float maxWidth = Random.Range(4, 8);
        yield return new WaitForEndOfFrame();
        float timer = 0;
        while (true)
        {

            timer += Time.deltaTime;
            trailRenderer.widthMultiplier = Mathf.Lerp(1f, maxWidth, timer);
            if (timer > 1f)
            {
                yield break;
            }
            yield return null;
        }
    }
    void OnCollisionEnter2D(Collision2D col)
    {
        if (particleSplash != null)
        {
            particleSplash.Play();
        }

        if (col.gameObject.tag != "Obstacle")
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


        this.transform.localScale = this.transform.localScale / _ballDivider;
        _numberOfBounces -= 1;
        if (_numberOfBounces <= 0)
        {
            if (_lastbounce)
            {
                this.gameObject.GetComponent<Rigidbody2D>().simulated = false;
            }
            _lastbounce = true;
        }

    }

}
