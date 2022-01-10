using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SeamlessBuyColorTutorial : MonoBehaviour
{

    [SerializeField] GameObject firstTask;
    [SerializeField] GameObject secondTask;
    [SerializeField] GameObject thirdTask;
    [SerializeField] GameObject forthTask;
    [SerializeField] GameObject fifthTask;

    [SerializeField] Image painter;

    [SerializeField] PauseMenuButton pauseMenuScript;

    ChooseThisColor _chooseThisColor;

    IsColorLocked isColorRedLocked;

    bool canGoThroughTutorial = false;
    void OnEnable()
    {
        string hasSeenTutorialBoolInString = PlayerPrefs.GetString("BuyColorTutorialSeen", "false");

        if(hasSeenTutorialBoolInString == "false"){
            StartCoroutine(NoAming());

            if (!painter.gameObject.activeSelf)
                firstTask.SetActive(true);
            if (secondTask.activeSelf)
                secondTask.SetActive(false);
            if (thirdTask.activeSelf)
                thirdTask.SetActive(false);
            if (forthTask.activeSelf)
                forthTask.SetActive(false);
            if (fifthTask.activeSelf)
                fifthTask.SetActive(false);
            StartCoroutine(WaitWithTutorial());
        }else{
            this.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        if (canGoThroughTutorial)
        {
            if (Input.GetMouseButtonUp(0))
            {
                if (firstTask.activeSelf)
                {
                    FirstDeactivateAndOpenPauseSecondActivate();
                }else if (secondTask.activeSelf)
                {
                    SeconddDeactivateBuyRedColorThirdActivate();
                }else if (thirdTask.activeSelf)
                {
                    ThirdDeactivateResumeForthActivate();
                }else if (forthTask.activeSelf)
                {
                    ForthAndPainterDeactivate();
                }
                else if (fifthTask.activeSelf)
                {
                    FifthAndPainterDeactivate();
                }
                else
                {
                    SkipTutorial();
                }
            }
        }
    }

    public void SkipTutorial()
    {
        SeconddDeactivateBuyRedColorThirdActivate();
        FifthAndPainterDeactivate();
        StartCoroutine(CanAim());
    }
    public void FirstDeactivateAndOpenPauseSecondActivate()
    {
        firstTask.SetActive(false);
        secondTask.SetActive(true);
        pauseMenuScript.PauseMenu();
        if (SoundManager.Instance != null)
            SoundManager.Instance.PlayButtonClick();
    }

    public void SecondDeactivateThirdActivate()
    {
        secondTask.SetActive(false);
        thirdTask.SetActive(true);
        if (SoundManager.Instance != null)
            SoundManager.Instance.PlayButtonClick();
    }

    public void SeconddDeactivateBuyRedColorThirdActivate()
    {
        if (PlayerPrefs.GetInt("PlayerCurrentMoney", 0) < 201)
        {
            PlayerPrefs.SetInt("PlayerCurrentMoney", 200);
        }

        GameObject colorRed = GameObject.FindGameObjectWithTag("RedColorTutorial");
        isColorRedLocked = colorRed.GetComponent<IsColorLocked>();
        secondTask.SetActive(false);
        isColorRedLocked.UnlockColor();
        thirdTask.SetActive(true);
        _chooseThisColor = GameObject.Find("Color1(Clone)").GetComponent<ChooseThisColor>();
        if (SoundManager.Instance != null)
            SoundManager.Instance.PlayButtonClick();
    }
    public void ThirdDeactivateResumeForthActivate()
    {
        thirdTask.SetActive(false);
        forthTask.SetActive(true);
        if (SoundManager.Instance != null)
            SoundManager.Instance.PlayButtonClick();
    }

    public void ForthAndPainterDeactivate()
    {
        forthTask.SetActive(false);
        pauseMenuScript.Resume();
        fifthTask.SetActive(true);
        if (SoundManager.Instance != null)
            SoundManager.Instance.PlayButtonClick();
    }
    public void FifthAndPainterDeactivate()
    {
        GameObject colorToPick = GameObject.FindGameObjectWithTag("ColorToPickRed");
        _chooseThisColor = colorToPick.GetComponent<ChooseThisColor>();
        if (_chooseThisColor != null)
            _chooseThisColor.UseThisColor();
        fifthTask.SetActive(false);
        painter.gameObject.SetActive(false);
        StartCoroutine(CanAim());
        if (SoundManager.Instance != null)
            SoundManager.Instance.PlayButtonClick();
    }
    IEnumerator WaitWithTutorial()
    {
        yield return new WaitForSeconds(2.6f);
        canGoThroughTutorial = true;
    }

    IEnumerator NoAming()
    {
        yield return new WaitForSeconds(1.72f);
        BallContorller.canYouStartAiming = false;
        yield return new WaitForEndOfFrame();
        if (BallContorller.canYouStartAiming)
            BallContorller.canYouStartAiming = false;
        yield return new WaitForEndOfFrame();
        if (BallContorller.canYouStartAiming)
            BallContorller.canYouStartAiming = false;
    }

    IEnumerator CanAim()
    {
        pauseMenuScript.Resume();
        yield return new WaitForSecondsRealtime(0.1f);
        pauseMenuScript.Resume();
        BallContorller.canYouStartAiming = true;
        this.gameObject.SetActive(false);
    }
}
