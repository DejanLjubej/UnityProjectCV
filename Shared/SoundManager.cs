using UnityEngine;

public class SoundManager : MonoBehaviour
{
    
    AudioSource[] _soundPlayed;
    private static SoundManager _instance;

    [Header("Bounce sounds")]
    [SerializeField] AudioClip basicBounceSound;
    [SerializeField] AudioClip comboSound;
    [SerializeField] AudioClip missSound;

    [Header("End Sounds")]
    [SerializeField] AudioClip soundOfVictory;
    [SerializeField] AudioClip soundOfDefeat;
    [SerializeField] AudioClip soundOfEnoughPoints;

    
    [Header("Other sounds")]
    [SerializeField] AudioClip tubeSound;
    [SerializeField] AudioClip buttonClick;
    [SerializeField] AudioClip colorPick;
    [SerializeField] AudioClip lastBallSound;
     [SerializeField] AudioClip gotGems;
    [SerializeField] AudioClip gotThreeGems;


    public static SoundManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<SoundManager>();

                //Tell unity not to destroy this object when loading a new scene!
                if(_instance != null)
                DontDestroyOnLoad(_instance.gameObject);
            }

            return _instance;
        }
    }

    void Awake()
    {
        _soundPlayed = this.gameObject.GetComponents<AudioSource>();
        if (_instance == null)
        {
            //If I am the first instance, make me the Singleton
            _instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            //If a Singleton already exists and you find
            //another reference in scene, destroy it!
            if (this != _instance)
            {
                Destroy(this.gameObject);
            }
        }
    }

    public void PlayBasic()
    {
        _soundPlayed[0].clip = basicBounceSound;
        _soundPlayed[0].Play();
    }   
    public void PlayMiss()
    {
        _soundPlayed[0].clip = missSound;
        _soundPlayed[0].Play();
    }
    public void PlayCombo()
    {
        _soundPlayed[1].clip = comboSound;
        _soundPlayed[1].Play();
    }

    public void PlayTubeSound()
    {
        _soundPlayed[0].clip = tubeSound;
        _soundPlayed[0].Play();
    }
    public void PlayButtonClick()
    {
        _soundPlayed[0].clip = buttonClick;
        _soundPlayed[0].Play();
    }
    public void PlayColorPick()
    {
        _soundPlayed[0].clip = colorPick;
        _soundPlayed[0].Play();
    }
    public void PlayVictorySound()
    {
        _soundPlayed[1].clip = soundOfVictory;
        _soundPlayed[1].Play();
    }
    public void PlayDefeatSound()
    {
        _soundPlayed[1].clip = soundOfDefeat;
        _soundPlayed[1].Play();
    }
    public void PlayEnoughPointsSound()
    {
        _soundPlayed[2].clip = soundOfEnoughPoints;
        _soundPlayed[2].Play();
    }
    public void PlayLastBallSound()
    {
        _soundPlayed[2].clip = lastBallSound;
        _soundPlayed[2].Play();
    }
    public void PlayGotGems()
    {
        _soundPlayed[2].clip = gotGems;
        _soundPlayed[2].Play();
    }
     public void PlayGotThreeGems()
    {
        _soundPlayed[2].clip = gotThreeGems;
        _soundPlayed[2].Play();
    }

    public void MusicVolumeAdjustment(float volume)
    {
        _soundPlayed[0].volume = volume;
        _soundPlayed[1].volume = volume;
        _soundPlayed[2].volume = volume;
    }
    public float CurrentMusicVolume()
    {
        return _soundPlayed[0].volume;
    }
}
