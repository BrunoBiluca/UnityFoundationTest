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
        player = GameObject.Find("player").transform;
    }

    void Update()
    {
        var lookDirection = (player.position - transform.position).normalized;
        rigidBody.AddForce(lookDirection * movementSpeed);
    }
}
