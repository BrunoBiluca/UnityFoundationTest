using Assets.UnityFoundation.Code.Character2D;
using Assets.UnityFoundation.Code.TimeUtils;
using Assets.UnityFoundation.HealthSystem;
using UnityEngine;

public class MetallPlayer : BaseCharacter
{
    public GameObject shootPrefab;
    public Transform shootSpawnRef;

    public IdleEnemyCharacterState idleState;
    public WalkEnemyCharacterState walkState;
    public AttackEnemyCharacterState attackState;
    public GetUpEnemyCharacterState getUpState;
    public GetDownEnemyCharacterState getDownState;

    public bool IsInvencible { get; set; }

    protected override void OnAwake()
    {
        var healthSystem = GetComponent<HealthSystem>();
        healthSystem.SetGuardDamage(() => IsInvencible);
    }

    protected override void SetCharacterStates()
    {
        idleState = new IdleEnemyCharacterState(this);
        walkState = new WalkEnemyCharacterState(this);
        attackState = new AttackEnemyCharacterState(this);
        getUpState = new GetUpEnemyCharacterState(this);
        getDownState = new GetDownEnemyCharacterState(this);

        TransitionToState(idleState);
    }
}

public class IdleEnemyCharacterState : BaseCharacterState
{
    protected readonly MetallPlayer player;
    protected readonly Rigidbody2D rigidbody;

    private readonly Timer idleTimer;

    public IdleEnemyCharacterState(MetallPlayer player)
    {
        this.player = player;
        rigidbody = player.GetComponent<Rigidbody2D>();

        idleTimer = new Timer(2f, () => {
            if(player.IsInvencible)
                player.TransitionToState(player.getUpState);
            else
                player.TransitionToState(player.walkState);
        });
    }

    public override void EnterState()
    {
        if(player.IsInvencible)
            player.GetComponent<Animator>().Play("idle_down");
        else
            player.GetComponent<Animator>().Play("idle");

        idleTimer.Start();
    }

    public override void ExitState()
    {
        rigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
        idleTimer.Close();
    }

    public override void FixedUpdate()
    {
        rigidbody.constraints = RigidbodyConstraints2D.FreezeAll;
        rigidbody.velocity = new Vector2(0f, 0f);
    }
}

public class WalkEnemyCharacterState : BaseCharacterState
{
    private const string startWalkAnimation = "start_walk";
    private const string walkAnimation = "walk";

    protected readonly MetallPlayer player;
    protected readonly Animator animator;
    protected readonly Rigidbody2D rigidbody;
    protected readonly SpriteRenderer spriteRenderer;

    private readonly Timer walkTimer;

    public WalkEnemyCharacterState(MetallPlayer player)
    {
        this.player = player;
        animator = player.GetComponent<Animator>();
        rigidbody = player.GetComponent<Rigidbody2D>();
        spriteRenderer = player.GetComponent<SpriteRenderer>();

        walkTimer = new Timer(
            1f, () => player.TransitionToState(player.getDownState)
        );
    }

    public override void EnterState()
    {
        animator.Play(walkAnimation);
        walkTimer.Start();
    }

    public override void FixedUpdate()
    {
        var inputX = spriteRenderer.flipX ? 1 : -1;
        var velocityX = 1000f * inputX * Time.deltaTime;

        rigidbody.velocity = new Vector2(velocityX, rigidbody.velocity.y);
    }
}

public class AttackEnemyCharacterState : BaseCharacterState
{
    protected readonly MetallPlayer player;
    protected readonly Rigidbody2D rigidbody;
    private readonly SpriteRenderer spriteRenderer;

    public AttackEnemyCharacterState(MetallPlayer player)
    {
        this.player = player;
        rigidbody = player.GetComponent<Rigidbody2D>();
        spriteRenderer = player.GetComponent<SpriteRenderer>();
    }

    public override void EnterState()
    {
        player.GetComponent<Animator>().Play("idle");

        var projectile = Object.Instantiate(
            player.shootPrefab,
            player.shootSpawnRef.position,
            Quaternion.identity
        );
        projectile.GetComponent<MegamanProjectile>()
            .Setup(spriteRenderer.flipX ? 1 : -1, player.gameObject);

        player.TransitionToState(player.idleState);
    }
}

public class GetUpEnemyCharacterState : BaseCharacterState
{
    protected readonly MetallPlayer player;

    public GetUpEnemyCharacterState(MetallPlayer player)
    {
        this.player = player;
    }

    public override void EnterState()
    {
        player.GetComponent<Animator>().Play("get_up");
    }

    public override void TriggerAnimationEvent(string eventName)
    {
        if(eventName == "get_up")
        {
            player.IsInvencible = false;
            player.TransitionToState(player.attackState);
        }

    }
}

public class GetDownEnemyCharacterState : BaseCharacterState
{
    protected readonly MetallPlayer player;
    private Rigidbody2D rigidbody;
    private readonly Timer getDownTimer;

    public GetDownEnemyCharacterState(MetallPlayer player)
    {
        this.player = player;
        getDownTimer = new Timer(
            2f, 
            () => player.TransitionToState(player.getUpState)
        );
    }

    public override void EnterState()
    {
        player.GetComponent<Animator>().Play("get_down");
        rigidbody = player.GetComponent<Rigidbody2D>();
    }

    public override void FixedUpdate()
    {
        rigidbody.constraints = RigidbodyConstraints2D.FreezeAll;
        rigidbody.velocity = new Vector2(0f, 0f);
    }

    public override void ExitState()
    {
        rigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    public override void TriggerAnimationEvent(string eventName)
    {
        if(eventName == "get_down")
        {
            player.IsInvencible = true;
            getDownTimer.Start();
        }
            
    }
}