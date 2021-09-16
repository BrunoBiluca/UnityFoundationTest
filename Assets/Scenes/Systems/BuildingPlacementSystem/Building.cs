using Assets.Scenes.Systems.BuildingPlacementSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    public GridObject GridObjectRef { get; private set; }

    public Building Setup(GridObject gridObject)
    {
        GridObjectRef = gridObject;
        return this;
    }
}
