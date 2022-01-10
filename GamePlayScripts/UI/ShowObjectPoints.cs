using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class ShowObjectPoints : MonoBehaviour
{
    Points _pointsOfObject;
    TextMeshPro _pointsTMP;

    [SerializeField] GameObject textForDisplay;

    void Start()
    {
        _pointsOfObject = GetComponent<Points>();
        _pointsTMP = textForDisplay.GetComponent<TextMeshPro>();
    }
    void OnTriggerEnter2D()
    {
        _pointsTMP.text = _pointsOfObject.pointsGainedByCollisionOnThisObject.ToString();
        Instantiate(textForDisplay, new Vector3(transform.position.x, transform.position.y+2f,0), Quaternion.identity);
    }
}
