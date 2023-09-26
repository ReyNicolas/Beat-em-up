using UnityEngine;
using UniRx;

public class EnemyHealthLogic: IHealth
{
    [SerializeField] EnemyLogic enemyLogic;

    private void Start()
    {
        enemyLogic = GetComponent<EnemyLogic>();
        actualHealth.Value = health.Value;
    }

    public override void LoseHealth(int amount)
    {
        if(actualHealth.Value>0) actualHealth.Value -= amount;
        if (actualHealth.Value <= 0) enemyLogic.SetDead();
        else enemyLogic.SetHit();
    }

}
