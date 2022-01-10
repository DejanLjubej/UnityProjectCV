using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SeamlessFirstTutorialManager : MonoBehaviour
{
    [SerializeField] GameObject tutorialStepOne;
    [SerializeField] GameObject tutorialStepTwo;
    [SerializeField] GameObject tutorialStepThree;
    [SerializeField] Image painterImage;


    BallContorller _ballCOntroller;
    PlayerStats _playerStats;

    bool canGoThroughTutorial = false;

      bool _hasSimulatedButtonDown = false;
    void Start()
    {
        _ballCOntroller = FindObjectOfType<BallContorller>();
        _playerStats = FindObjectOfType<PlayerStats>();

        if (!tutorialStepOne.activeSelf)
            tutorialStepOne.SetActive(true);

        if (tutorialStepTwo.activeSelf)
            tutorialStepTwo.SetActive(false);

        if (tutorialStepThree.activeSelf)
            tutorialStepThree.SetActive(false);
        StartCoroutine(WaitWithTutorial());
    }

    void Update()
    {
        if (canGoThroughTutorial)
        { 
            if (tutorialStepOne.activeSelf)
            {
                if (!_hasSimulatedButtonDown)
                {
                    _ballCOntroller.mousePos2D = new Vector2(1, -2.2f);
                    _ballCOntroller.MouseButtonDownActions();
                    StartCoroutine(SimulateDrag());
                    _hasSimulatedButtonDown = true;
                    _ballCOntroller._canShoot = false;
                }
                _ballCOntroller.MouseButtonHoldActions();

                  if(Input.GetMouseButton(0) && _ballCOntroller.hit){
                    _ballCOntroller._canShoot = true;
                    tutorialStepOne.SetActive(false);
                    tutorialStepTwo.SetActive(true);
                }
            }

            if(LevelScore.score >= LevelScore.requiredPointsForPublicUse)
            {
                tutorialStepTwo.SetActive(false);
                tutorialStepThree.SetActive(true);
            }

            if (_playerStats.currentNumberOfBallsLeft <= 0 || _playerStats.haveWon)
            {
                tutorialStepTwo.SetActive(false);
                tutorialStepThree.SetActive(false);
                SkipTutorial();
            }
        }
    }

IEnumerator SimulateDrag()
    {
        _ballCOntroller.mousePos2D = new Vector2(0, 0);
        float timer =0;
        while (true)
        {
            timer += Time.deltaTime/2;
            _ballCOntroller.mousePos2D = new Vector2(Mathf.Lerp(1,2.3f,timer), Mathf.Lerp(-2.2f, -6.9f, timer));

            if (timer > 0.995f)
            {
                timer = 0;
            }

            if(Input.GetMouseButton(0) && _ballCOntroller.hit)
            {
                yield break;
            }
            yield return null;
        }

    }
    IEnumerator WaitWithTutorial()
    {
        yield return new WaitForSeconds(2f);
        canGoThroughTutorial = true;
    }

    public void SkipTutorial()
    {
        this.gameObject.SetActive(false);
    }
}
