using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Analytics;


public class PauseMenuButton : MonoBehaviour
{
    public GameObject pausePanel;

    ListOfSelectableColors colorList;

    Vector3 pauseHiddenPosition;
    Vector3 pauseShownPosition;

    Dictionary<string, object> analyticsDataToSend;

    [SerializeField] GameObject buyIndicator;

    public void PauseMenu()
    {
        if(SoundManager.Instance != null)
        SoundManager.Instance.PlayButtonClick();

        this.gameObject.GetComponent<Button>().interactable = false;
        StartCoroutine(ShowPanel());
        if (Application.installMode == ApplicationInstallMode.Store)
            Analytics.CustomEvent("gameWasPaused", analyticsDataToSend);
    }
    public void Resume()
    {
        if(SoundManager.Instance != null)
        SoundManager.Instance.PlayButtonClick();

        this.gameObject.GetComponent<Button>().interactable = true;
        StartCoroutine(HidePanel());
        colorList.DisplayColorsToChoose();
        if (Application.installMode == ApplicationInstallMode.Store)
            Analytics.CustomEvent("gameWasResumed", analyticsDataToSend);
    }

    IEnumerator ShowPanel()
    {
        Time.timeScale = 0;
        float timeSinceStart = Time.unscaledTime;
        while (true)
        {
            float timeForTransition =0;
            timeForTransition += Time.unscaledTime -timeSinceStart;

            pausePanel.transform.localPosition = Vector3.Lerp(pauseHiddenPosition, pauseShownPosition, timeForTransition*50);
            if(pausePanel.transform.localPosition == pauseShownPosition)
            {
                yield break;
            }

            yield return null;
        }
    }
    
    IEnumerator HidePanel()
    {
        Time.timeScale = 1;


        float timeSinceStart = 0;
        while (true)
        {
            timeSinceStart += Time.unscaledTime * 10;
            pausePanel.transform.localPosition = Vector3.Lerp(pauseShownPosition, pauseHiddenPosition, timeSinceStart);
            if(pausePanel.transform.localPosition == pauseHiddenPosition)
            {
                yield break;
            }
            yield return null;
        }
    }


   public void ActivateMarkerForAction()
    {
        buyIndicator.SetActive(true);
    }  
    public void DeactivateMarkerForAction()
    {
        buyIndicator.SetActive(false);
    }

    void Awake()
    {
        pauseShownPosition = new Vector3(0, 0, 0);
        pauseHiddenPosition =  new Vector3(2000, 0, 0); 
        colorList = FindObjectOfType<ListOfSelectableColors>();

        analyticsDataToSend = new Dictionary<string, object>() { 
            {"timeSinceStart", AnalyticsSessionInfo.sessionElapsedTime } 
        };
        buyIndicator.SetActive(false);
    }

}
