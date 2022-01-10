using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideScorePreview : MonoBehaviour
{
    float _time = 0;

    void Update()
    {
        _time += Time.deltaTime;
        if (_time > 1f)
        {
            Destroy(gameObject);
        }
    }
}
