using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class MegamanXPlayer : Player
{
    public Transform shootSpawnRef;
    public GameObject megamanShootPrefab;

    public bool readyToShoot;

    private SpriteRenderer spriteRenderer;

    protected override void OnAwake()
    {
        base.OnAwake();

        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    protected override void SetCharacterStates() {
        base.SetCharacterStates();

        attackingState = new MegamanXShootCharacterState(this);
    }
}
