using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class UIHealth : MonoBehaviour
{
    [SerializeField] Slider healthSlider;
    void Start()
    {
        PlayerHealthLogic playerHealthLogic = GameObject.FindWithTag("Player").GetComponent<PlayerHealthLogic>();
        playerHealthLogic.actualHealth.Subscribe(SetActualHealth);
        playerHealthLogic.health.Subscribe(SetMaxHealth);
        SetActualHealth(playerHealthLogic.actualHealth.Value);
        SetMaxHealth(playerHealthLogic.health.Value);
    }

    void SetActualHealth(int actualHealth) =>healthSlider.value =  actualHealth;
    void SetMaxHealth(int maxHealth) =>healthSlider.maxValue =  maxHealth;

    
}
