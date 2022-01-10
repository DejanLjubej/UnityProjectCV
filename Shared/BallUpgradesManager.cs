using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Analytics;
using UnityEngine.SceneManagement;

public class BallUpgradesManager : MonoBehaviour
{
    
    [SerializeField] Button _bouncinessUpgradeButton;
    [SerializeField] Button _bouncesUpgradeButton;
    [SerializeField] Button _speedUpgradeButton;

    [SerializeField] TMP_Text _bouncinessPriceText;
    [SerializeField] TMP_Text _bouncesPriceText;
    [SerializeField] TMP_Text _speedPriceText;

    [SerializeField] TMP_Text _bouncinessAmountText;
    [SerializeField] TMP_Text _bouncesAmountText;
    [SerializeField] TMP_Text _speedAmountText;

    [SerializeField] int _bouncinessBasePrice = 500;
    [SerializeField] int _bouncesBasePrice = 400;
    [SerializeField] int _speedBasePrice = 600;

    int _bouncinessPrice;
    int _bouncesPrice;
    int _speedPrice;

    float _bouncinessAmount;
    int _bouncesAmount;
    float _speedAmount;

    int _bouncinessUpgradeStage;
    int _bouncesUpgradeStage;
    int _speedUpgradeStage;

    int _bouncinessUpgradeStageMax = 11;
    int _bouncesUpgradeStageMax = 11;
    int _speedUpgradeStageMax = 11;

    int _playerMoney;
    float _ballSize;

    Animator _bouncinessButtonAnimator;
    Animator _bouncesButtonAnimator;
    Animator _speedButtonAnimator;

    ActionIndicatorManager _actionIndicatorManager;
    UpgradesList _upgradesList;

    // Analytics
    string _sceneName;
    Dictionary<string, object> _analyiticsDictionary;

    void Awake()
    {
        CheckPlayerMoney();

        _bouncinessUpgradeStage = PlayerPrefs.GetInt("BouncinessUpgradeStage", 0);
        _bouncesUpgradeStage = PlayerPrefs.GetInt("BouncesUpgradeStage", 0);
        _speedUpgradeStage = PlayerPrefs.GetInt("SpeedUpgradeStage", 0);

        AdjustUpgradePrices();

        _bouncinessAmount = PlayerPrefs.GetFloat("BouncinessUpgrade", 0.5f);
        _bouncesAmount = PlayerPrefs.GetInt("NumberOfBounces", 4);
        _speedAmount = PlayerPrefs.GetFloat("BallMass", 10f);

        _ballSize = PlayerPrefs.GetFloat("BallSize", 5f);

        StartCoroutine(WaitTillLoaded());

        _bouncinessButtonAnimator = _bouncinessUpgradeButton.GetComponent<Animator>();
        _bouncesButtonAnimator = _bouncesUpgradeButton.GetComponent<Animator>();
        _speedButtonAnimator = _speedUpgradeButton.GetComponent<Animator>();

        _sceneName = SceneManager.GetActiveScene().name;
        _analyiticsDictionary = new Dictionary<string, object>()
        {
            {"SceneName", _sceneName  }
        };
    }


    IEnumerator WaitTillLoaded()
    {
        yield return new WaitForSecondsRealtime(2f);
        UpdateTextFields();
        ButtonDeactovation();
        _upgradesList = FindObjectOfType<UpgradesList>();

    }
    public void ButtonDeactovation()
    {
        CheckPlayerMoney();

        if ((_bouncinessUpgradeStage < _bouncinessUpgradeStageMax) && (_playerMoney >= _bouncinessPrice))
        {
            _bouncinessUpgradeButton.interactable = true;
            _bouncinessButtonAnimator.enabled = true; 
        }
        else
        {
            _bouncinessUpgradeButton.interactable = false;
            _bouncinessButtonAnimator.enabled = false; 
        }

        if ((_bouncesUpgradeStage < _bouncesUpgradeStageMax) && (_playerMoney >= _bouncesPrice))
        {
            _bouncesUpgradeButton.interactable = true;
            _bouncesButtonAnimator.enabled = true;
        }
        else
        {
            _bouncesUpgradeButton.interactable = false;
            _bouncesButtonAnimator.enabled = false;
        }

        if ((_speedUpgradeStage < _speedUpgradeStageMax) && (_playerMoney >= _speedPrice))
        {
            _speedUpgradeButton.interactable = true;
            _speedButtonAnimator.enabled = true;
        }
        else
        {
            _speedUpgradeButton.interactable = false;
            _speedButtonAnimator.enabled = false;
        }

        _actionIndicatorManager = FindObjectOfType<ActionIndicatorManager>();
        if (_actionIndicatorManager != null)
            _actionIndicatorManager.CheckTheConditionsForIndicatorActivationGameItems();
    }


    public void UpgradeBounciness()
    {
        _bouncinessUpgradeStage++;
        PlayerPrefs.SetInt("BouncinessUpgradeStage", _bouncinessUpgradeStage);

        _playerMoney -= _bouncinessPrice;
        PlayerPrefs.SetInt("PlayerCurrentMoney", _playerMoney);

        _bouncinessAmount += 0.05f;
        PlayerPrefs.SetFloat("BouncinessUpgrade",_bouncinessAmount);

        AdjustUpgradePrices();
        UpdateTextFields();
        ButtonDeactovation();
        HandleColorsPriceTag();

        if(Application.installMode == ApplicationInstallMode.Store)
            Analytics.CustomEvent($"Bounciness Upgrade Stage {_bouncinessUpgradeStage}", _analyiticsDictionary);
    }

    public void UpgradeBounces()
    {
        _bouncesUpgradeStage++;
        PlayerPrefs.SetInt("BouncesUpgradeStage", _bouncesUpgradeStage);

        _playerMoney -= _bouncesPrice;
        PlayerPrefs.SetInt("PlayerCurrentMoney", _playerMoney);

        _bouncesAmount++;
        PlayerPrefs.SetInt("NumberOfBounces", _bouncesAmount);

        _ballSize += 0.5f;
        PlayerPrefs.SetFloat("BallSize", _ballSize);

        AdjustUpgradePrices();
        UpdateTextFields();
        ButtonDeactovation();
        HandleColorsPriceTag();
        if(Application.installMode == ApplicationInstallMode.Store)
            Analytics.CustomEvent($"Bounces Upgrade Stage {_bouncesUpgradeStage}", _analyiticsDictionary);
    }

    public void UpgradeSpeed()
    {
        
        _speedUpgradeStage++;
        PlayerPrefs.SetInt("SpeedUpgradeStage", _speedUpgradeStage);

        _playerMoney -= _speedPrice;
        PlayerPrefs.SetInt("PlayerCurrentMoney", _playerMoney);

        _speedAmount -= 0.5f;
        PlayerPrefs.SetFloat("BallMass", _speedAmount);

        AdjustUpgradePrices();
        UpdateTextFields();
        ButtonDeactovation();
        HandleColorsPriceTag();
        if(Application.installMode == ApplicationInstallMode.Store)
            Analytics.CustomEvent($"Speed Upgrade Stage {_speedUpgradeStage}", _analyiticsDictionary);
    }
     
    void CheckPlayerMoney()
    {
        _playerMoney = PlayerPrefs.GetInt("PlayerCurrentMoney", 0);
    }
    void AdjustUpgradePrices()
    {
        _bouncinessPrice = (int)(_bouncinessBasePrice * Mathf.Pow(2, _bouncinessUpgradeStage));
        _bouncesPrice = (int)(_bouncesBasePrice * Mathf.Pow(2, _bouncesUpgradeStage));
        _speedPrice = (int)(_speedBasePrice * Mathf.Pow(2,  _speedUpgradeStage));

        PlayerPrefs.SetInt("BouncinessPrice", _bouncinessPrice);
        PlayerPrefs.SetInt("BouncesPrice", _bouncesPrice);
        PlayerPrefs.SetInt("SpeedPrice", _speedPrice);
    }
    void UpdateTextFields()
    {
        _bouncinessPriceText.text = _bouncinessPrice.ToString();
        _bouncesPriceText.text = _bouncesPrice.ToString();
        _speedPriceText.text = _speedPrice.ToString();

        _bouncinessAmountText.text = _bouncinessAmount.ToString("F2");
        _bouncesAmountText.text = (_bouncesAmount-1).ToString();
        _speedAmountText.text = (1000/_speedAmount).ToString("F0");
    }

    void HandleColorsPriceTag()
    {
        _upgradesList.HandleEachColor();

    }
}
