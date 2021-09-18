using Assets.UnityFoundation.Code.ObjectPooling;
using UnityEngine;

public class PowerupSpawner : Singleton<PowerupSpawner>
{
    private ObjectPooling objectPooling;

    private void Start()
    {
        objectPooling = GetComponentInChildren<ObjectPooling>();
        objectPooling.InstantiateObjects();
    }

    public void InvokePowerups()
    {
        CancelInvoke(nameof(InstantiatePowerup));
        InvokeRepeating(nameof(InstantiatePowerup), 0f, 6f);
    }

    private void InstantiatePowerup()
    {
        var spawnPositionX = Random.Range(9f, -9f);
        var spawnPositionZ = Random.Range(9f, -9f);

        objectPooling
            .GetAvailableObject()
            .Some(powerup => {
                powerup.Activate(powerupGO => {
                    powerupGO.transform.position = new Vector3(
                        spawnPositionX, 0f, spawnPositionZ
                    );
                });
            });
    }
}
