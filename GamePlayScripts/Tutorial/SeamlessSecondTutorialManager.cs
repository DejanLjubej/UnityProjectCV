using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SeamlessSecondTutorialManager : MonoBehaviour
{

    [SerializeField] GameObject outOfFrameTask;

    BallContorller _ballCOntroller;
    PlayerStats _playerStats;

    void Start()
    {
        _ballCOntroller = FindObjectOfType<BallContorller>();
        _playerStats = FindObjectOfType<PlayerStats>();

        outOfFrameTask.SetActive(true);
    }

    void Update()
    {
        if (outOfFrameTask.activeSelf)
        {
            if(_ballCOntroller.quitAimingHit || Input.GetMouseButtonUp(0))
            {
                outOfFrameTask.SetActive(false);
            }
        }

        if (_playerStats.currentNumberOfBallsLeft <= 0 || _playerStats.haveWon)
        {
            SkipTutorial();
        }
    }

    public void SkipTutorial()
    {
        this.gameObject.SetActive(false);
    }
}
