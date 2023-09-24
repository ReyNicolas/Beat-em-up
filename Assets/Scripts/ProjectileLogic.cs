using UnityEngine;
using UniRx.Triggers;
using UniRx;
using Unity.VisualScripting;

public class ProjectileLogic : MonoBehaviour
{
    [SerializeField] string enemyTag;
    [SerializeField] int damage;
    private void Start()
    {
        this.OnCollisionEnter2DAsObservable()
            .Where(IsMyEnemyTag)
            .Subscribe(DoDamage);
    }

    private void DoDamage(Collision2D collision)
    {
       collision.gameObject.GetComponent<IHealth>().LoseHealth(damage);
        Destroy(gameObject);
    }

    private bool IsMyEnemyTag(Collision2D collision) =>
        collision.gameObject.CompareTag(enemyTag);
}
