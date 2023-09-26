using UnityEngine;
using UniRx.Triggers;
using UniRx;
using System;

public class EnemyLogic : MonoBehaviour, IEventEntity
{
    protected Animator animator;
    Rigidbody2D rb;
    [Header("Attack info")]
    [SerializeField] int numberOfAttacks;
    [SerializeField] AttackLogic attackLogic;

    [Header("State info")]
    [SerializeField] protected bool isDoingAction;
    [SerializeField] protected bool canBeCounter;
    [SerializeField] protected bool isStunned;
    [SerializeField] float stunnedTime;
    [SerializeField] bool isPlayerClose;

    public event Action onTakeDamage;
    public event Action onDead;
    public event Action onAttack;


    [Header("Move info")]
    [SerializeField] int minSpeedInt;
    [SerializeField] int maxSpeedInt;
    [SerializeField] float moveSpeed;
    [SerializeField] Vector3 vMove;
    [SerializeField] protected Transform followTransform;
    [SerializeField] protected Transform playerTransform;

    protected CompositeDisposable compositeDisposable = new CompositeDisposable();


    protected virtual void Start()
    {
        animator = GetComponent<Animator>();
        playerTransform = GameObject.FindWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>();
        moveSpeed = (float)(UnityEngine.Random.Range(minSpeedInt, maxSpeedInt+1))/10;
        SubscribeLogic();       
    }

    protected virtual void SubscribeLogic()
    {
        // collisions
        compositeDisposable.Add(this.OnCollisionStay2DAsObservable()
            .Where(collider => collider.gameObject.CompareTag("Player")).
            Subscribe(_ => rb.velocity = Vector2.zero));

        compositeDisposable.Add(this.OnTriggerEnter2DAsObservable()
             .Where(IsPlayerTag)
             .Subscribe(_ => isPlayerClose = true));

        compositeDisposable.Add(this.OnTriggerExit2DAsObservable()
             .Where(IsPlayerTag)
             .Subscribe(_ => isPlayerClose = false));

        //update
        var update = Observable.EveryUpdate();

        compositeDisposable.Add(update
             .Where(_ => !isDoingAction)
             .Do(_ => Move())
             .Where(_ => isPlayerClose)
             .Subscribe(_ => Attack()));
    }

    public void DisposeLogic()
    {
        compositeDisposable.Dispose();
    }


    bool IsPlayerTag(Collider2D collision) => 
        collision.CompareTag("Player");

    protected virtual void Attack()
    {
        rb.velocity = Vector2.right * transform.localScale.x * 0.5f;
        int attackIndex = UnityEngine.Random.Range(1, numberOfAttacks +1 ); // Choose a random attack 
        attackLogic.attackID = attackIndex;
        animator.SetTrigger("Attack" + attackIndex);
        isDoingAction = true;
    }

    public virtual bool CanBeCounter()
    {
        if (canBeCounter)
        {
            isStunned = true;
            animator.SetBool("Stunned", true);
            Invoke("SetStunnedFalse", stunnedTime);
            rb.velocity = Vector2.zero;
            canBeCounter = false;
            return true;
        }
        return false;
    }
    void Move()
    {
        vMove = playerTransform.position - followTransform.position;
        transform.position = Vector2.MoveTowards(transform.position, transform.position + vMove, moveSpeed * Time.deltaTime);
        animator.SetFloat("Speed", vMove.sqrMagnitude);
        if (vMove.x > 0.05f) transform.localScale = new Vector3(1, 1, 1);
        else if (vMove.x < -0.05f) transform.localScale = new Vector3(-1, 1, 1);
    }

    void EndAction()
    {
       if(!isStunned)  isDoingAction = false;
        canBeCounter = false;
    }
    void SetCounterFalse()=> 
        canBeCounter = false;
    
    void SetCounterTrue() => 
        canBeCounter = true;

    void WhenGetHit() => 
        isDoingAction = true;

    void SetStunnedFalse()
    {
        isStunned=false;
        isDoingAction=false;
        animator.SetBool("Stunned", false);
    }

    public void SetHit()
    {
        rb.velocity = Vector2.right * -transform.localScale.x;
        animator.SetTrigger("Hit");
        onTakeDamage?.Invoke();
    }

    public void SetDead()
    {
        rb.velocity = Vector2.zero;
        animator.SetTrigger("Dead");
        onDead?.Invoke();
    }

    void InvokeOnAttack()
    {
        onAttack?.Invoke();
    }
}
