using UnityEngine;
using UnityEngine.UI;
public class AdjustMusicVolume : MonoBehaviour
{
    MusicManager _musicManager;
    Scrollbar _scrollBar;

    float _currentMusicVolume;
    
    void Start()
    {
        _scrollBar= this.gameObject.GetComponent<Scrollbar>();
        _musicManager = FindObjectOfType<MusicManager>();
        if (_musicManager!=null) {
            _currentMusicVolume = _musicManager.CurrentMusicVolume();
            _scrollBar.value = _currentMusicVolume;
        }
    }

    void Update()
    {
        float valueofScrollbar = _scrollBar.value;
        if(_musicManager != null)
        {
            _musicManager.MusicVolumeAdjustment(valueofScrollbar);
        }
    }
}
