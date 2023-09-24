using System;
using UnityEngine;
using UniRx;

public class PlayerHealthLogic : IHealth
{
    Animator animator;
    [SerializeField]  PlayerData playerData;

    private void Awake()
    {
        animator = GetComponent<Animator>();       
    }

    private void Start()
    {
        health.Value = playerData.playerHealth.Value;
        actualHealth.Value = playerData.playerHealth.Value;
    }


    public override void LoseHealth(int amount)
    {
        actualHealth.Value -= amount;
        animator.SetTrigger("Hit");
        if (actualHealth.Value <= 0) animator.SetBool("Dead", true);
    }
}

public abstract class IHealth : MonoBehaviour
{
    public ReactiveProperty<int> health = new ReactiveProperty<int>();
    public  ReactiveProperty<int> actualHealth = new ReactiveProperty<int>();

    public abstract void LoseHealth(int amount);



}