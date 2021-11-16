using Assets.UnityFoundation.Systems.ObjectPooling;
using UnityEngine;

public class ShipHandler : MonoBehaviour {

    private ObjectPooling shootObjectPooling;

    void Start() {
        shootObjectPooling = GetComponent<ObjectPooling>();
        shootObjectPooling.InstantiateObjects();

        InvokeRepeating(nameof(Shoot), 0.33f, 0.33f);
    }

    public void Shoot() {
        shootObjectPooling
            .GetAvailableObject()
            .Get()
            .Activate(obj => obj.transform.position = transform.position);
    }

}
