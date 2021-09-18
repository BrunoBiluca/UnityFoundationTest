using Assets.Scenes.Systems.BuildingPlacementSystem;
using Assets.UnityFoundation.CameraScripts;
using Assets.UnityFoundation.Code.DebugHelper;
using Assets.UnityFoundation.Code.Grid;
using Assets.UnityFoundation.Code.ObjectPooling;
using System.Collections.Generic;
using UnityEngine;

public class BuildingPlacementSystem : Singleton<BuildingPlacementSystem>
{
    [SerializeField] MultipleObjectPooling buildingPooling;

    [SerializeField] List<GridObjectSO> buildings;

    private GridObjectDirection currentDirection;
    private GridObjectSO currentBuilding;
    private GridXZDebug<GridObject> grid;

    protected override void OnAwake()
    {
        grid = new GridXZDebug<GridObject>(
            new ObjectPlacementGrid(10, 10, 4)
        );
        grid.Display();

        currentBuilding = buildings[0];
        currentDirection = GridObjectDirection.DOWN;
    }

    private void Start()
    {
        buildingPooling.InstantiateObjects();
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            var position = grid.GetGridPostion(CameraUtils.GetMousePosition3D());
            if(!grid.IsInsideGrid(position.x, position.y))
            {
                DebugPopup.Create("Can't create here.");
                return;
            }

            var requestBuildingPos = grid.GetWorldPosition(position.x, position.y);

            var gridObject = new GridObject(currentBuilding, currentDirection);
            if(grid.TrySetGridValue(requestBuildingPos, gridObject))
            {
                var building = buildingPooling.GetAvailableObject(currentBuilding.Tag).Get();

                building
                    .GetComponent<Building>()
                    .Setup(gridObject)
                    .Activate((go) => {
                        go
                        .transform
                        .position = requestBuildingPos + CalculateOffset();

                        go
                        .transform
                        .rotation = Quaternion.Euler(0f, currentDirection.Rotation, 0f);
                    });
            }
        }

        if(Input.GetKeyDown(KeyCode.Alpha1))
            currentBuilding = buildings[0];
        else if(Input.GetKeyDown(KeyCode.Alpha2))
            currentBuilding = buildings[1];
        else if(Input.GetKeyDown(KeyCode.Alpha3))
            currentBuilding = buildings[2];

        if(Input.GetKeyDown(KeyCode.R))
        {
            currentDirection = currentDirection.Next();
        }
    }

    public void SetCurrentBuilding(GridObjectSO building)
    {
        currentBuilding = building;
    }

    private Vector3 CalculateOffset()
    {
        var offsetX = currentBuilding.Width;
        var offsetY = currentBuilding.Height;

        if(
            currentDirection == GridObjectDirection.LEFT
            || currentDirection == GridObjectDirection.RIGHT
        )
        {
            offsetX = currentBuilding.Height;
            offsetY = currentBuilding.Width;
        }

        return new Vector3(
           offsetX * currentDirection.Offset.x * grid.CellSize,
           0f,
           offsetY * currentDirection.Offset.y * grid.CellSize
        );
    }

    public void RemoveBuilding(Building building)
    {
        grid.ClearGridValue(building.GridObjectRef);
        building.Deactivate();
    }

}
