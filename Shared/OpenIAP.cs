using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OpenIAP : MonoBehaviour
{
    Button openIAPButton;

    GameObject IAPWindow;

    void Start()
    {
        openIAPButton = this.GetComponent<Button>();
        IAPWindow = GameObject.Find("IAP");

        if (openIAPButton) openIAPButton.onClick.AddListener(IAPOpen);
    }

    void IAPOpen()
    {
        IAPWindow.transform.localPosition = new Vector3(0, 0, 0);
        Debug.Log($"Iap Wind {IAPWindow}");
        Time.timeScale = 0;
    }
}
