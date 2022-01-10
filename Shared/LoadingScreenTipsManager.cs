using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingScreenTipsManager : MonoBehaviour
{
    [SerializeField] GameObject[] _tipObjects;

    static int currentTip;


    void Start()
    {
        _tipObjects[currentTip].SetActive(true);
    }

    public void ChangeShownTip()
    {
        foreach (var item in _tipObjects)
        {
            item.SetActive(false);
        }
        currentTip = Random.Range(0, _tipObjects.Length);
        _tipObjects[currentTip].SetActive(true);
    }
}
