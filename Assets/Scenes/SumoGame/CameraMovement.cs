using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private float rotateSpeed;

    void Update()
    {
        var direction = -1;
        var rotationInput = direction * Input.GetAxis("Horizontal");
        transform.Rotate(new Vector3(
            0,
            rotationInput * rotateSpeed * Time.deltaTime
        ));
    }
}
