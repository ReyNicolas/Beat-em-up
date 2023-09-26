using System.Collections.Generic;
using UnityEngine;
using UniRx.Triggers;
using UniRx;
using System;

public class AttackLogic : MonoBehaviour
{
    public int attackID;
    [SerializeField] string enemyTag;
    [SerializeField] List<int> attacksDamages;
    public event Action OnHitEnemy;
    public event Action OnLaunch;

    private void Start()
    {       
        this.OnCollisionEnter2DAsObservable()
            .Where(IsMyEnemyTag)
            .Subscribe(DoDamage);
    }

    void DoDamage(Collision2D collision)
    {
        collision.gameObject.GetComponent<IHealth>().LoseHealth(attacksDamages[attackID-1]);
        OnHitEnemy?.Invoke();
    }

    bool IsMyEnemyTag(Collision2D collision) => 
        collision.gameObject.CompareTag(enemyTag);

    void InvokeOnLaunch()
    {
        OnLaunch?.Invoke();
    }
}
