using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrengthPowerUp : MonoBehaviour
{
    public void Handle(SumoPlayer player)
    {
        player.HasPowerUp = true;

        Destroy(gameObject);
    }
}
