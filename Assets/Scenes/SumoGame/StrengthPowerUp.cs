using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrengthPowerUp : MonoBehaviour
{
    public void Handle(SumoPlayerController player)
    {
        player.HasPowerup = true;

        Destroy(gameObject);
    }
}
