using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossPromotionIndicator : MonoBehaviour
{

    [SerializeField] GameObject _indicator;
    public void DeactivateIndicator(){
        _indicator.SetActive(false);
    }
}
