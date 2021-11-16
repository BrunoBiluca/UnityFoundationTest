using Assets.UnityFoundation.Code.Common;
using Assets.UnityFoundation.Systems.ObjectPooling;
using UnityEngine;

public class SumoEnemySpawner : Singleton<SumoEnemySpawner>
{
    private ObjectPooling objectPooling;

    private void Start()
    {
        objectPooling = GetComponentInChildren<ObjectPooling>();
        objectPooling.InstantiateObjects();
    }

    public void InvokeEnemies()
    {
        InvokeRepeating(nameof(InstantiateEnemy), 0f, 3f);
    }

    private void InstantiateEnemy()
    {
        var enemyRef = objectPooling.GetAvailableObject().Get();

        enemyRef.Activate((SumoEnemyController enemyGO) => {
            var spawnPositionX = Random.Range(9f, -9f);
            var spawnPositionZ = Random.Range(9f, -9f);

            var mass = Random.Range(0.8f, 2.0f);

            enemyGO.Setup(new Vector3(spawnPositionX, 0f, spawnPositionZ), mass);
        });
    }
}
