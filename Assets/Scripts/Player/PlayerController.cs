using UnityEngine;
using UniRx;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;

    public Animator anim;
    [SerializeField] AttackLogic attackLogic;

    [Header("Move info")]
    [SerializeField] float moveSpeed;
    [SerializeField] Vector2 vMove;

    [Header("Dash info")]
    [SerializeField] GameObject dashClonePrefab;
    [SerializeField] float dashSpeed;
    [SerializeField] float dashDuration;
    [SerializeField] float dashCooldown;
    public ReactiveProperty<float> dashTimer = new ReactiveProperty<float>();

    [Header("Counter info")]
    [SerializeField] float counterCooldown;
    public ReactiveProperty<float> counterTimer = new ReactiveProperty<float>();


    [Header("Jump info")]
    [SerializeField] float jumpForce;

    [Header("State info")]
    [SerializeField] bool isGrounded;
    [SerializeField] bool isDoingAction;

    CompositeDisposable compositeDisposable = new CompositeDisposable();

    private void Awake()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();       
    }

    private void OnEnable()
    {        
        var update =  Observable.EveryUpdate();
        // update
        compositeDisposable.Add
            (update
            .Do(_=>GetAxis())
            .Where(_=>!isDoingAction)
            .Subscribe(_=>Move()));

        compositeDisposable.Add
             (update
        .Where(_ => CanJump())
        .Subscribe(_ => Jump()));

        compositeDisposable.Add
            (update
        .Where(_ => CanAttack())
        .Subscribe(_ => Attack()));

        compositeDisposable.Add
            (update
        .Where(_ => CanCounter())
        .Subscribe(_ => CounterAttack()));
    }


    private void OnDisable()
    {
        compositeDisposable.Dispose();
        rb.velocity = Vector2.zero;
    }
    void GetAxis()
    {
        vMove.x = Input.GetAxis("Horizontal");
        vMove.y = Input.GetAxis("Vertical");
        vMove = Vector2.ClampMagnitude(vMove, 1);
        dashTimer.Value -= Time.deltaTime;
        counterTimer.Value -= Time.deltaTime;
    }

    void Move()
    {
        if (Input.GetKeyDown(KeyCode.Space) && vMove != Vector2.zero)
        {
            if (dashTimer.Value < 0) StartCoroutine(Dash());
        }
        else
        {
            rb.velocity = vMove * moveSpeed* Time.timeScale;
            anim.SetFloat("Speed", Mathf.Abs(vMove.sqrMagnitude));        
        }
        Flip();
    }

    void Flip()
    {
        if (vMove.x > 0) transform.localScale = new Vector3(1, 1, 1);
        else if (vMove.x < 0) transform.localScale = new Vector3(-1, 1, 1);
    }

    IEnumerator Dash()
    {
        rb.velocity = vMove.normalized * dashSpeed * Time.timeScale;
        isDoingAction = true;
        anim.SetBool("Dash", true);
        for(int i = 1; i <=3; i++)
        {
            yield return new WaitForSeconds(dashDuration/6);
            Instantiate(dashClonePrefab,transform.position,Quaternion.identity).transform.localScale = transform.localScale;
            yield return new WaitForSeconds(dashDuration / 6);
        }
        anim.SetBool("Dash", false);
        isDoingAction=false;
        dashTimer.Value = dashCooldown;
    }

    bool CanAttack() =>
        !isDoingAction && Input.GetButtonDown("Fire1");
    bool CanJump() =>
       isGrounded && Input.GetButtonDown("Jump");
    bool CanCounter() =>
        !isDoingAction && Input.GetButtonDown("Fire2") && counterTimer.Value <0;

    void Attack()
    {
        rb.velocity = Vector2.zero;
        int attackIndex = Random.Range(1, 4); // Choose a random attack (1, 2, or 3)
        attackLogic.attackID = attackIndex;
        anim.SetTrigger("Attack" + attackIndex);
        isDoingAction = true;
    }   

    void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        anim.SetTrigger("Jump");
    }

    void EndAttack() =>
        isDoingAction = false;
    void CounterAttack()
    {
        rb.velocity = Vector2.zero;
        anim.SetTrigger("CounterAttack");
        counterTimer.Value = counterCooldown;
        isDoingAction = true;
    }

    public void CounterAttackSuccessful(int atacckIndex)
    {
        rb.velocity = Vector2.zero;
        anim.SetTrigger("CounterAttackSuccessful");
        attackLogic.attackID = atacckIndex;
        isDoingAction = true;
    }
}


