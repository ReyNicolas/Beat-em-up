using System;
using UnityEngine;
using UniRx;

public class PlayerHealthLogic : IHealth
{
    Animator animator;
    [SerializeField]  PlayerData playerData;
    [SerializeField]  float inmuneWindow;
    float inmuneTimer;

    private void Awake()
    {
        animator = GetComponent<Animator>();       
    }

    private void Start()
    {
        health.Value = playerData.playerHealth.Value;
        actualHealth.Value = playerData.playerHealth.Value;
    }
    private void Update()
    {
        inmuneTimer -= Time.deltaTime;
    }

    public override void LoseHealth(int amount)
    {
        if (inmuneTimer > 0) return;
        actualHealth.Value -= amount;
        inmuneTimer = inmuneWindow;
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