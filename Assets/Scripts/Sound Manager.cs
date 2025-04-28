using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _heartLoseSound;
    [SerializeField] private AudioClip _loseSound;
    [SerializeField] private AudioClip _winSound;

    public static SoundManager Instance;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
             DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Debug.LogError("Class Sound Manager have Duplicate");
            Destroy(this.gameObject);

        }

    }
    void Start()
    {
        //GlobalData.Instance._soundSlider.onValueChanged.AddListener((float newValue) => _audioSource.volume = newValue);

        //_audioSource.volume = GlobalData.Instance.SoundVolume;
        
    }

    public void PlayHeartLoseCLip(){
        _audioSource.PlayOneShot(_heartLoseSound);
    }

    public void PlayWinSound(){
        _audioSource.PlayOneShot(_winSound);
    }

    public void PlayLoseSound(){
        _audioSource.PlayOneShot(_loseSound);
    }

    public void ChangeVolume(float volume){
        _audioSource.volume = volume;
    }
    
    public float GetVolume(){
        return _audioSource.volume;
    }
}
