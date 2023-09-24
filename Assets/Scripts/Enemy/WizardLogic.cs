using UniRx;
using UnityEngine;

public class WizardLogic: EnemyLogic
{
    [Header("Speel info")]
    [SerializeField] GameObject spellPrefab;
    [SerializeField] float spellSpeed;
    [SerializeField] float spellLifeTime;
    [SerializeField] float spellCooldown;
    float spellTimer;

    protected override void Start()
    {
        base.Start();
        spellTimer = spellCooldown;
    }

    protected override void SubscribeLogic()
    {
        base.SubscribeLogic();
        compositeDisposable.Add(Observable.EveryUpdate()
             .Do(_=>spellTimer-= Time.deltaTime)
             .Where(_ =>  CanLaunchSpell())
             .Subscribe(_ => StartCastingSpell()));
    }

    bool CanLaunchSpell() =>
        !isDoingAction && spellTimer < 0; 

    void StartCastingSpell()
    {
        isDoingAction = true;
        animator.SetTrigger("CastSpell");

    }

    public void LaunchSpell()
    {
        spellTimer = spellCooldown;
        var newSpellGO = Instantiate(spellPrefab, followTransform.position, Quaternion.identity);
        newSpellGO.GetComponent<Rigidbody2D>().velocity = (playerTransform.position - followTransform.position).normalized * spellSpeed;
        newSpellGO.transform.up = (playerTransform.position - followTransform.position);
        newSpellGO.transform.Rotate(0, 0, 90);
        Destroy(newSpellGO,spellLifeTime);
    }
}