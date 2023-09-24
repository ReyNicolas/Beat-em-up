using UnityEngine;
using UniRx;

public class EnemyHealthLogic: IHealth
{
    [SerializeField] Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
        actualHealth.Value = health.Value;
    }

    public override void LoseHealth(int amount)
    {
        if(actualHealth.Value>0) actualHealth.Value -= amount;
        if (actualHealth.Value <= 0) SetDead();
        else animator.SetTrigger("Hit");
    }

    private void SetDead()
    {
        animator.SetTrigger("Dead");
    }
}
