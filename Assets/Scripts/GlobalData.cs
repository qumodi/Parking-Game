using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GlobalData : MonoBehaviour
{

    public static GlobalData Instance;

    [SerializeField] private Sliders _sliders;
    //music & Sounds
    public float MusicVolume = 1f;
    public float SoundVolume = 1f;

    //Level
    public int LevelNumber = 1;
    public float BestCoplitionTime = 0;
    public int StarsNumber = 0;
    // Start is called before the first frame update
    void Awake()
    {
        Screen.orientation = ScreenOrientation.LandscapeLeft;

        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);

        }
        else
        {
            Destroy(this.gameObject);
            Debug.LogError("Class GlobalData have Duplicate");
        }



        if (SceneManager.GetActiveScene().name == "Main Menu")
        {
            _sliders = Sliders.Instance;

            if (_sliders != null)
            {
                _sliders._musicSlider.value = MusicManager.Instance.GetVolume();
                _sliders._soundSlider.value = SoundManager.Instance.GetVolume();

                _sliders._musicSlider.onValueChanged.AddListener((float musicValue) => this.MusicVolume = musicValue);
                _sliders._soundSlider.onValueChanged.AddListener((float soundValue) => this.SoundVolume = soundValue);


                _sliders._musicSlider.onValueChanged.AddListener((float musicValue) => MusicManager.Instance.ChangeVolume(musicValue));
                _sliders._soundSlider.onValueChanged.AddListener((float soundValue) => SoundManager.Instance.ChangeVolume(soundValue));
            }
        }
    }

    void Start()
    {

    }

}
