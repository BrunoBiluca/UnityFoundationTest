using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SumoPlayerController : MonoBehaviour
{
    [SerializeField] private float speed;

    private Rigidbody rigidBody;
    private Transform focalPoint;

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
}
