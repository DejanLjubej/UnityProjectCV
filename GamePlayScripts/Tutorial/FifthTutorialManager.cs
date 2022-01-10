using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FifthTutorialManager : MonoBehaviour
{
    [SerializeField] GameObject firstTask;
    [SerializeField] GameObject secondTask;
    [SerializeField] GameObject thirdTask;
    [SerializeField] GameObject forthTask;
    [SerializeField] GameObject fifthTask;
    
    [SerializeField] Image painter;

    [SerializeField] PauseMenuButton pauseMenuScript;

    IsColorLocked isColorRedLocked;


    void OnEnable()
    {
        StartCoroutine(NoAming());

        if (!painter.gameObject.activeSelf)
            painter.gameObject.SetActive(true);
        if(!firstTask.activeSelf)
            firstTask.SetActive(true);
        if(secondTask.activeSelf)
            secondTask.SetActive(false); 
        if(thirdTask.activeSelf)
            thirdTask.SetActive(false); 
        if(forthTask.activeSelf)
            forthTask.SetActive(false); 
        if(fifthTask.activeSelf)
            fifthTask.SetActive(false);
    }

    public void FirstDeactivateAndOpenPauseSecondActivate()
    {
        firstTask.SetActive(false);
        pauseMenuScript.PauseMenu();
        secondTask.SetActive(true);
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

    public void ThirdDeactivateBuyRedColorForthActivate()
    {
        GameObject colorRed = GameObject.FindGameObjectWithTag("RedColorTutorial");
        isColorRedLocked = colorRed.GetComponent<IsColorLocked>();
        thirdTask.SetActive(false);
        isColorRedLocked.UnlockColor();
        forthTask.SetActive(true);
        if (SoundManager.Instance != null)
            SoundManager.Instance.PlayButtonClick();
    }

    public void ForthDeactivateResumeFifthActivate()
    {
        forthTask.SetActive(false);
        pauseMenuScript.Resume();
        fifthTask.SetActive(true);
        if (SoundManager.Instance != null)
            SoundManager.Instance.PlayButtonClick();
    }

    public void FifthAndPainterDeactivate()
    {
        fifthTask.SetActive(false);
        painter.gameObject.SetActive(false);
        StartCoroutine(CanAim());
        if(SoundManager.Instance != null)
            SoundManager.Instance.PlayButtonClick();    
    }


    IEnumerator NoAming()
    {
        yield return new WaitForSeconds(1.5f);
        BallContorller.canYouStartAiming = false;
    }

    IEnumerator CanAim()
    {
        yield return new WaitForSeconds(0.2f);
        BallContorller.canYouStartAiming = true;
    }
}
