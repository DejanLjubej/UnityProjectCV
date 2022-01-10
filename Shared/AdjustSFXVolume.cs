using UnityEngine;
using UnityEngine.UI;
public class AdjustSFXVolume : MonoBehaviour
{
    Scrollbar _scrollBar;
    float _currentMusicVolume;

    void Start()
    {
        _scrollBar = this.gameObject.GetComponent<Scrollbar>();
        if (SoundManager.Instance!= null)
        {
            _currentMusicVolume = SoundManager.Instance.CurrentMusicVolume();
            _scrollBar.value = _currentMusicVolume;
        }
    }

    void Update()
    {
        float valueofScrollbar = _scrollBar.value;
        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.MusicVolumeAdjustment(valueofScrollbar);
        }
    }  
}
