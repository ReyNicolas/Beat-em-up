using System.Collections.Generic;
using UnityEngine;
using UniRx.Triggers;
using UniRx;

public class AttackLogic : MonoBehaviour
{
    public int attackID;
    [SerializeField] string enemyTag;
    [SerializeField] List<int> attacksDamages;

    private void Start()
    {       
        this.OnCollisionEnter2DAsObservable()
            .Where(IsMyEnemyTag)
            .Subscribe(DoDamage);
    }

    private void DoDamage(Collision2D collision) => 
        collision.gameObject.GetComponent<IHealth>().LoseHealth(attacksDamages[attackID-1]);

    private bool IsMyEnemyTag(Collision2D collision) => 
        collision.gameObject.CompareTag(enemyTag);
}
