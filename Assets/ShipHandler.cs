using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipHandler : MonoBehaviour {

    private ObjectPooling shootObjectPooling;

    void Start() {
        shootObjectPooling = GetComponent<ObjectPooling>();

        InvokeRepeating(nameof(Shoot), 0.33f, 0.33f);
    }

    public void Shoot() {
        shootObjectPooling
            .GetAvailableObject()
            .Active(obj => obj.transform.position = transform.position);
    }

}
