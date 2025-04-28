using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

using Random = UnityEngine.Random;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;
    [SerializeField] private HealthManager _healthManager;
    public UnityEvent Defeat = new UnityEvent();

    float completionTime = 0f;
    public static bool GamePaused = false;
    [SerializeField] private ButtonsManager _buttonsManager;

    [SerializeField] private TextMeshProUGUI _timer;
    [SerializeField] private TextMeshProUGUI _levelNumber;
        //Lose Screen
    [SerializeField] private TextMeshProUGUI _loseScreenText;
    [SerializeField] private TextMeshProUGUI _starsEarnedText;
    [SerializeField] private TextMeshProUGUI _complitionTimeText;
        


    void Awake()
    {
        if(Instance == null){
            Instance = this;
        }else{
            Debug.LogError("Class Level Manager have Duplicate");
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        //_levelNumber.text = $"Level:{Convert.ToString(GlobalData.Instance.LevelNumber)}";
        UpdateLevelNumber();
        Time.timeScale = 1f;
        GamePaused = false;
    }

    // Update is called once per frame
    void Update()
    {
        completionTime += Time.deltaTime;
        _timer.text = "Time:" + Convert.ToString((int) completionTime);
        
    }

    public void LoseHeart(){
        _healthManager.DecreaseHealth();
        SoundManager.Instance.PlayHeartLoseCLip();
    } 

    public void Lose(){
        SoundManager.Instance.PlayLoseSound();
        _loseScreenText.text = $"You`ve Lost On Level {GlobalData.Instance.LevelNumber} with {GlobalData.Instance.StarsNumber} stars";
        _buttonsManager.EnableLoseMenu();
        Time.timeScale = 0;
    }

    public void Win(){
        Debug.Log("Win is called");

        SoundManager.Instance.PlayWinSound();
        _buttonsManager.EnableWinMenu();
        Time.timeScale = 0;

        if(GlobalData.Instance.BestCoplitionTime == 0 ){
            GlobalData.Instance.BestCoplitionTime = completionTime;
        }else if(GlobalData.Instance.BestCoplitionTime > completionTime){
            GlobalData.Instance.BestCoplitionTime = completionTime;
        }

        int starsEarned = Random.Range(0,50);
        _starsEarnedText.text = $"Stars Earned: {starsEarned}";
        GlobalData.Instance.StarsNumber += starsEarned;

        _complitionTimeText.text = $"Complition time: {completionTime}";
    }

    public void RestartTimer(){
        completionTime = 0;
    }

    public void UpdateLevelNumber(){
        _levelNumber.text = $"Level:{Convert.ToString(GlobalData.Instance.LevelNumber)}";
    }
}
