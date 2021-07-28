using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SumoPlayerController : MonoBehaviour
{
    [SerializeField] private float speed;

    private Rigidbody rigidBody;
    private Transform focalPoint;

    public float pushStrength = 15f;
    public bool HasPowerup {get; set;}

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();
    }

    void Start()
    {
        focalPoint = GameObject.Find("focal_point").transform;
    }

    void Update()
    {
        var moveInput = Input.GetAxis("Vertical");

        rigidBody.AddForce(focalPoint.forward * moveInput * speed);
    }

    void OnTriggerEnter(Collider other)
    {
        if(!other.TryGetComponent(out StrengthPowerUp powerUp))
            return;

        powerUp.Handle(this);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(!HasPowerup)
            return;

        if(!collision.gameObject.TryGetComponent(out SumoEnemyController enemy))
            return;

        Vector3 awayFromPlayer = enemy.transform.position - transform.position;
        enemy.GetComponent<Rigidbody>()
            .AddForce(
                awayFromPlayer * pushStrength,
                ForceMode.Impulse
            );
    }
}
