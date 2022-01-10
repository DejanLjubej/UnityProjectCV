using UnityEngine;

public class MusicManager : MonoBehaviour
{

    [SerializeField] AudioClip[] songs;

    float[] songTime;

    private static MusicManager _instance;
    
    AudioSource _musicPlayed;
    public static MusicManager instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<MusicManager>();

                //Tell unity not to destroy this object when loading a new scene!
                DontDestroyOnLoad(_instance.gameObject);
            }

            return _instance;
        }
    }

    void Awake()
    {
        songTime = new float[songs.Length];
        _musicPlayed = this.gameObject.GetComponent<AudioSource>();
        for (int i = 0; i < songs.Length; i++)
        {
            songTime[i] = songs[i].length;
        }

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
                Play();
                Destroy(this.gameObject);
            }
        }
    }

    public void Play()
    {
        int songToPlay = Random.Range(0, songs.Length - 1);
        _musicPlayed.clip = songs[songToPlay];
        //this.gameObject.GetComponent<AudioSource>();
        _musicPlayed.Play();
    }

    public void MusicVolumeAdjustment(float volume)
    {
        this.gameObject.GetComponent<AudioSource>().volume = volume;
    }
    public float CurrentMusicVolume()
    {
        return this.gameObject.GetComponent<AudioSource>().volume;
    }

    void Update()
    {
        if (!_musicPlayed.isPlaying)
        {
            Play();
        }
    }
}
