using Assets.UnityFoundation.Code.ObjectPooling;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrengthPowerUp : PooledObject
{
    public void Handle(SumoPlayer player)
    {
        player.HasPowerUp = true;

        Deactivate();
    }
}
