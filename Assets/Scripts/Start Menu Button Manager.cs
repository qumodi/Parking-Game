using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenuButtonManager : MonoBehaviour
{
    public static StartMenuButtonManager Instance;
    [SerializeField] private GameObject _settingsMenu;
    [SerializeField] private TextMeshProUGUI _starsCount;
    [SerializeField] private TextMeshProUGUI _bestComplitionTime;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogError("Class StartMenuButtonManager have Duplicate");
        }
    }

    public void Start()
    {
        _starsCount.text = ":" + Convert.ToString(GlobalData.Instance.StarsNumber);
        _bestComplitionTime.text = "Best Complition Time:" + Convert.ToString(GlobalData.Instance.BestCoplitionTime);
    }

    public void EnableSettingsMenu(){
        _settingsMenu.SetActive(true);
    }
    public void DisableSettingsMenu(){
        _settingsMenu.SetActive(false);
    }

    public void StartGame(){
        SceneManager.LoadScene("LevelScene");
    }
}
