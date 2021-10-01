using Assets.UnityFoundation.Code.Character2D;
using UnityEngine;

public class MegamanXShootCharacterState : AttackCharacterState
{
    private readonly MegamanXPlayer megaman;
    private readonly Animator animator;
    private readonly SpriteRenderer spriteRenderer;

    public int maxSimultaneousShoots = 3;
    public int activeShoots;
    private bool canShoot;

    public MegamanXShootCharacterState(MegamanXPlayer player) : base(player)
    {
        megaman = player;
        animator = player.GetComponent<Animator>();
        spriteRenderer = player.GetComponent<SpriteRenderer>();
    }

    public override bool CanEnterState()
    {
        return activeShoots < maxSimultaneousShoots;
    }

    public override void EnterState()
    {
        animator.Play("shot");
        canShoot = true;
    }

    public override void Update()
    {
        if(
            !canShoot
            || !megaman.readyToShoot
        )
        { return; }

        var projectile = Object.Instantiate(
            megaman.megamanShootPrefab, megaman.shootSpawnRef.position, Quaternion.identity
        );
        MegamanProjectile megamanProjectile = projectile.GetComponent<MegamanProjectile>();
        megamanProjectile.Setup(spriteRenderer.flipX ? -1 : 1, megaman.gameObject);

        megamanProjectile.OnShootDestroy += (sender, args) => activeShoots--;

        activeShoots++;
        canShoot = false;

        megaman.TransitionToState(megaman.idleState);
    }
}