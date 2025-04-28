using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour
{
    [SerializeField] private List<Image> _hearts;
    private const int Max_Health = 3;
    public int CurrHealth { get; private set;}

    void Awake()
    {
        CurrHealth = Max_Health;
    }

    void Start()
    {
        
    }

    public void DecreaseHealth(){
        CurrHealth--;
        for(int i = CurrHealth; i < Max_Health ;i++){
            _hearts[i].enabled = false;
        }
        if(CurrHealth == 0){
            LevelManager.Instance.Defeat.Invoke();
        }
        if(CurrHealth == 0){
            LevelManager.Instance.Lose();
        }
    }

    
}
