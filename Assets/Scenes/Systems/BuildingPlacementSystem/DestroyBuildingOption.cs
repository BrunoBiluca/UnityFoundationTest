using Assets.UnityFoundation.Systems.BuildingPlacementSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DestroyBuildingOption : MonoBehaviour
{
    void Start()
    {
        var building = GetComponentInParent<Building>();

        GetComponent<Button>().onClick
            .AddListener(() => {
                BuildingPlacementSystem.Instance.RemoveBuilding(building);
            });
    }
}
