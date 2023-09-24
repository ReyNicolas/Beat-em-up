using UnityEngine;
using UniRx.Triggers;
using UniRx;

public class CounterControl: MonoBehaviour
{
    [SerializeField] string enemyTag;
    [SerializeField] int attackID;
    [SerializeField] PlayerController playerController;
    private void Start()
    {
        this.OnTriggerEnter2DAsObservable()
            .Where(IsMyEnemyTag)
            .Subscribe(PlayerCounterAttack);
    }

    void PlayerCounterAttack(Collider2D collision)
    {
        if(collision.GetComponent<EnemyLogic>().CanBeCounter()) playerController.CounterAttackSuccessful(attackID);
    }

    bool IsMyEnemyTag(Collider2D collision) =>
        collision.CompareTag(enemyTag);



}