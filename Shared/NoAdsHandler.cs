using UnityEngine;
using UnityEngine.UI;
using System;
public class NoAdsHandler : MonoBehaviour
{
    public static float noAdsTimer;

    GemManager gemManager;
    TimeSpan interval;

    [SerializeField] float timeAmount;
    [SerializeField] int costOFAction;
    [SerializeField] Text timerText;
    [SerializeField] GameObject timeObject;
    [SerializeField] Animator _timerAnimator;

    private static NoAdsHandler _instance;

    public static NoAdsHandler Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<NoAdsHandler>();

                DontDestroyOnLoad(_instance.gameObject);
            }
            return _instance;
        }
    }



    public void SetNoAdsTimer()
    {
        if(GemManager.gemAmount >= costOFAction)
        {
            _timerAnimator.SetTrigger("TimerOn");
            _timerAnimator.Play("ShowTimer");
            gemManager = FindObjectOfType<GemManager>();
            GemManager.gemAmount -= costOFAction;
            gemManager.UpdateGemAmountText();
            noAdsTimer += timeAmount;
        }
    }

    void Update()
    {
    
        if (noAdsTimer > 0)
        {
            noAdsTimer -= Time.deltaTime;
            interval = TimeSpan.FromSeconds(noAdsTimer);
            timerText.text = string.Format("{0:00}:{1:00}", interval.Minutes, interval.Seconds);
        }
        else
        {
            _timerAnimator.SetTrigger("TimerOff");
            noAdsTimer = 0;
        }
    }

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            if (this != _instance)
            {
                Destroy(this.gameObject);
            }
        }
        gemManager = FindObjectOfType<GemManager>();
    }
}
