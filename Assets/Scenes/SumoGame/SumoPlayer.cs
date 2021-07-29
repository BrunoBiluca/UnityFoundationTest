using System;
using System.Collections;
using UnityEngine;

public class SumoPlayer : MonoBehaviour
{
    public class SumoPlayerStats
    {
        public float Speed { get; set; } = 5f;
        public float PushStrength { get; set; } = 15f;
    }

    public bool HasPowerUp { get; set; }
    public SumoPlayerStats stats = new SumoPlayerStats();

    private GameObject powerUpIndicator;
    private Rigidbody rigidBody;
    private Transform focalPoint;

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();
    }

    void Start()
    {
        focalPoint = GameObject.Find("focal_point").transform;

        powerUpIndicator = transform.Find("powerup_indicator").gameObject;
        powerUpIndicator.SetActive(false);
    }

    void Update()
    {
        var movementInput = Input.GetAxis("Vertical");

        rigidBody.AddForce(focalPoint.forward * movementInput * stats.Speed);
    }

    public void OnTriggerEnter(Collider other)
    {
        if(!other.TryGetComponent(out StrengthPowerUp powerUp))
            return;

        powerUp.Handle(this);
        powerUpIndicator.SetActive(true);

        StartCoroutine(ResetPowerup());
    }

    private IEnumerator ResetPowerup()
    {
        yield return new WaitForSeconds(4f);
        HasPowerUp = false;
        powerUpIndicator.SetActive(false);
    }

    public void OnCollisionEnter(Collision collision)
    {
        if(!HasPowerUp)
            return;

        if(!collision.gameObject.TryGetComponent(out SumoEnemyController enemy))
            return;

        Vector3 awayFromPlayer = enemy.transform.position - transform.position;
        enemy.GetComponent<Rigidbody>()
            .AddForce(
                awayFromPlayer * stats.PushStrength,
                ForceMode.Impulse
            );
    }
}
