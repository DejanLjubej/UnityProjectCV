using UnityEngine;
using UnityEngine.UI;
public class MoneyManagement : MonoBehaviour
{

    public int playerCurrentMoney = 150;
    Text currentMoneyText;
    void Start()
    {
        currentMoneyText = this.GetComponent<Text>();
        PlayerPrefs.GetInt("PlayerCurrentMoney", playerCurrentMoney);
    }

    void Update()
    {
        currentMoneyText.text = PlayerPrefs.GetInt("PlayerCurrentMoney", 0).ToString();
    }
}
