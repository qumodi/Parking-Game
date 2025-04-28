using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Sliders : MonoBehaviour
{
    public Slider _musicSlider;
    public Slider _soundSlider;

    public static Sliders Instance;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
            Debug.LogError("Class Sliders have Duplicate");
        }
    }
}
