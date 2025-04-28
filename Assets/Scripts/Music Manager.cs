using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _music;

    public static MusicManager Instance;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Debug.LogError("Class AudioManager have Duplicate");
            Destroy(this.gameObject);
        }

    }

    void Start()
    {
        //GlobalData.Instance._musicSlider.onValueChanged.AddListener((float newValue) => _audioSource.volume = newValue);

        //_audioSource.clip = _music;
        //_audioSource.Play();
    }

    public void ChangeVolume(float volume){
        _audioSource.volume = volume;
    }

    public float GetVolume(){
        return _audioSource.volume;
    }
}
