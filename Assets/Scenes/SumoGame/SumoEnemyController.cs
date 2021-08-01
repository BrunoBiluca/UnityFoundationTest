using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SumoEnemyController : MonoBehaviour
{
    [SerializeField] private float movementSpeed;

    private Rigidbody rigidBody;
    private Transform player;

    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();

        var playerGO = GameObject.Find("player");
        if(playerGO != null)
            player = playerGO.transform;
    }

    void Update()
    {
        FollowPlayer();

        if(transform.position.y < -10)
        {
            SumoGameManager.Instance.EnemyFell();
            Destroy(gameObject);
        }
    }

    private void FollowPlayer()
    {
        if(player == null) {
            Destroy(gameObject);
            return;
        }

        try
        {
            var lookDirection = (player.position - transform.position).normalized;
            rigidBody.AddForce(lookDirection * movementSpeed);
        }
        catch(MissingReferenceException)
        {
            Destroy(gameObject);
        }
    }
}
