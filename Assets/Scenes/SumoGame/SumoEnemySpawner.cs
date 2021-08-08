using UnityEngine;

public class SumoEnemySpawner : Singleton<SumoEnemySpawner>
{
    [SerializeField] private ObjectPooling objectPooling;

    public void InvokeEnemies()
    {
        InvokeRepeating(nameof(InstantiateEnemy), 0f, 3f);
    }

    private void InstantiateEnemy()
    {
        var spawnPositionX = Random.Range(9f, -9f);
        var spawnPositionZ = Random.Range(9f, -9f);

        var enemyRef = objectPooling.GetAvailableObject() as SumoEnemyController;

        enemyRef.Activate(enemyGO => {
            enemyGO.transform.position = new Vector3(spawnPositionX, 0f, spawnPositionZ);
        });
    }
}
