using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SumoEnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;

    void Start()
    {
        InvokeRepeating(nameof(InstantiateEnemy), 0f, 3f);
    }

    private void InstantiateEnemy()
    {
        var spawnPositionX = Random.Range(9f, -9f);
        var spawnPositionZ = Random.Range(9f, -9f);

        Instantiate(
            enemyPrefab, 
            new Vector3(spawnPositionX, 0f, spawnPositionZ), 
            Quaternion.identity
        );       
    }
}
