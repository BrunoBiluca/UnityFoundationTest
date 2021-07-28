using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SumoEnemyController : MonoBehaviour
{
    [SerializeField] private float movementSpeed;

    private Rigidbody rigidBody;
    private Transform player;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        player = GameObject.Find("player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        var lookDirection = (player.position - transform.position).normalized;
        rigidBody.AddForce(lookDirection * movementSpeed);
    }
}
