using Assets.UnityFoundation.Code.Character2D;
using Assets.UnityFoundation.Systems.HealthSystem;
using UnityEngine;

public class MegamanXPlayer : Player
{
    public Transform shootSpawnRef;
    public GameObject megamanShootPrefab;

    public bool readyToShoot;

    protected override void OnAwake()
    {
        GetComponent<HealthSystem>();
    }

    protected override void SetCharacterStates() {
        base.SetCharacterStates();

        attackingState = new MegamanXShootCharacterState(this);
    }
}
