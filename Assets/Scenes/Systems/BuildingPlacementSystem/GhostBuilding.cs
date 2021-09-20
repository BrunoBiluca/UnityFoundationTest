using Assets.Scenes.Systems.BuildingPlacementSystem;
using Assets.UnityFoundation.CameraScripts;
using Assets.UnityFoundation.Code;
using UnityEngine;

public class GhostBuilding : MonoBehaviour
{
    private Transform ghostTransform;

    private void Start()
    {
        ghostTransform = transform.GetChild(0);

        TransformUtils.ChangeLayer(ghostTransform, LayerMask.NameToLayer("Ghost"));

        BuildingPlacementSystem.Instance
            .OnCurrentSelectedBuildingChange += CurrentSelectedBuildingChangeHandle;
    }

    private void CurrentSelectedBuildingChangeHandle(GridObjectSO obj)
    {
        Destroy(ghostTransform.gameObject);

        var newGhostBuilding = Instantiate(obj.Prefab, transform);
        TransformUtils.ChangeLayer(
            newGhostBuilding.transform, LayerMask.NameToLayer("Ghost")
        );

        ghostTransform = newGhostBuilding.transform;
    }

    private void Update()
    {
        var canBuild = BuildingPlacementSystem
            .Instance
            .CanBuild(
                CameraUtils.GetMousePosition3D(), 
                out Vector3 buildingPosition,
                out Quaternion buildingRotation
            );

        if(canBuild)
        {
            ghostTransform.gameObject.SetActive(true);
            transform.position = buildingPosition;
            transform.rotation = buildingRotation;
        }
        else
        {
            ghostTransform.gameObject.SetActive(false);
        }

            
    }
}
