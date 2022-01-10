using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Analytics;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
public class IsColorLocked : MonoBehaviour
{


    Animator _colorAnimator;
    public Button isColorLocked;
    public Image imageForColor;
    public int price;

    ActionIndicatorManager _actionIndicatorManager;
    UpgradesList _upgradesList;

    Dictionary<string, object> _parametersToSendToAnalytics;
    void Start()
    {
        _actionIndicatorManager = FindObjectOfType<ActionIndicatorManager>();
        _upgradesList = FindObjectOfType<UpgradesList>();
        _colorAnimator = GetComponent<Animator>();
        ShowColors();
        _parametersToSendToAnalytics = new Dictionary<string, object>() { 
            {"sceneWhenColorWasBought", SceneManager.GetActiveScene().name}, 
        };
    }

    public void ShowColors()
    {
        if(isColorLocked != null)
        {
            if (PlayerPrefs.GetInt("Selectable"+this.name, 0) != 0)
            {
                isColorLocked.gameObject.SetActive(false);
            }
            else
            {
                isColorLocked.gameObject.SetActive(true);
                isColorLocked.GetComponentInChildren<Text>().text = price.ToString();
                ChangePricetagColor();
            }
        }
    }

    public void ChangePricetagColor()
    {
        Image[] listOfGos = gameObject.GetComponentsInChildren<Image>();
        foreach (Image item in listOfGos)
        {
            if (item.gameObject.name == "Money")
            {
                if (PlayerPrefs.GetInt("PlayerCurrentMoney", 0) >= price)
                {
                    item.color = Color.white;
                    if(_colorAnimator)
                        _colorAnimator.enabled = true;
                }
                else
                {
                    item.color = Color.gray;
                    if(_colorAnimator)
                        _colorAnimator.enabled = false;
                }
            }
        }
    }

    public void UnlockColor()
    {
        if (PlayerPrefs.GetInt("PlayerCurrentMoney", 0) >= price)
        {
            if(SoundManager.Instance != null)
                SoundManager.Instance.PlayButtonClick();
            if(PlayerPrefs.GetInt("Selectable"+this.name,0) == 0)
            {
                PlayerPrefs.SetInt("PlayerCurrentMoney", PlayerPrefs.GetInt("PlayerCurrentMoney", 0) - price);
                PlayerPrefs.SetInt("Selectable"+this.name, 1);

                _parametersToSendToAnalytics.Add("ColorPrice", price);

                UpgradesList.amountOfColorsBought++;
                PlayerPrefs.SetInt("AmountOfColorsBought", UpgradesList.amountOfColorsBought);
                HandleSettingActionNotifier();
                if (_actionIndicatorManager != null)
                {
                    _actionIndicatorManager.CheckTheConditionsForIndicatorActivationGameItems();
                }

                if (Application.installMode == ApplicationInstallMode.Store)
                    Analytics.CustomEvent($"{this.name} was bought", _parametersToSendToAnalytics);

            }
        }
        ShowColors();

        if (Application.installMode == ApplicationInstallMode.Store)
            Analytics.CustomEvent("colorWasClicked", _parametersToSendToAnalytics);

    }

    void HandleSettingActionNotifier()
    {
        _upgradesList.HandleEachColor();
    }

}
