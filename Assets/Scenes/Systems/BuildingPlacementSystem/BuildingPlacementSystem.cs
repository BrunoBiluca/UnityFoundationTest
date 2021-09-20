using Assets.Scenes.Systems.BuildingPlacementSystem;
using Assets.UnityFoundation.CameraScripts;
using Assets.UnityFoundation.Code.DebugHelper;
using Assets.UnityFoundation.Code.Grid;
using Assets.UnityFoundation.Code.ObjectPooling;
using System;
using System.Collections.Generic;
using UnityEngine;

public class BuildingPlacementSystem : Singleton<BuildingPlacementSystem>
{
    [SerializeField] MultipleObjectPooling buildingPooling;

    [SerializeField] List<GridObjectSO> buildings;

    public event Action<GridObjectSO> OnCurrentSelectedBuildingChange;

    private GridObjectDirection currentDirection;
    private GridObjectSO currentBuilding;
    private GridXZDebug<GridObject> grid;

    private GridObjectSO CurrentBuilding {
        get { return currentBuilding; }
        set {
            currentBuilding = value;
            OnCurrentSelectedBuildingChange?.Invoke(currentBuilding);
        }
    }

    protected override void OnAwake()
    {
        grid = new GridXZDebug<GridObject>(
            new ObjectPlacementGrid(10, 10, 4)
        );
        grid.Display();

        CurrentBuilding = buildings[0];
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
            CreateBuilding();
        }

        if(Input.GetKeyDown(KeyCode.R))
        {
            currentDirection = currentDirection.Next();
        }

        HotkeysInput();
    }

    private void HotkeysInput()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
            CurrentBuilding = buildings[0];
        else if(Input.GetKeyDown(KeyCode.Alpha2))
            CurrentBuilding = buildings[1];
        else if(Input.GetKeyDown(KeyCode.Alpha3))
            CurrentBuilding = buildings[2];
    }

    private void CreateBuilding()
    {
        var position = grid.GetGridPostion(CameraUtils.GetMousePosition3D());
        if(!grid.IsInsideGrid(position.x, position.y))
        {
            DebugPopup.Create("Can't create here.");
            return;
        }

        var gridObject = new GridObject(CurrentBuilding, currentDirection);

        if(!grid.TrySetGridValue(grid.GetWorldPosition(position.x, position.y), gridObject))
            return;

        var building = buildingPooling.GetAvailableObject(CurrentBuilding.Tag).Get();
        building
            .GetComponent<Building>()
            .Setup(gridObject)
            .Activate((go) => {
                go
                .transform
                .position = grid.GetWorldPosition(position.x, position.y, gridObject);

                go
                .transform
                .rotation = Quaternion.Euler(0f, currentDirection.Rotation, 0f);
            });
    }

    public void SetCurrentBuilding(GridObjectSO building)
    {
        CurrentBuilding = building;
    }

    public void RemoveBuilding(Building building)
    {
        grid.ClearGridValue(building.GridObjectRef);
        building.Deactivate();
    }

    public bool CanBuild(Vector3 position, out Vector3 gridPosition, out Quaternion rotation)
    {
        var gridPos = grid.GetGridPostion(position);

        var newGridObject = new GridObject(CurrentBuilding, currentDirection);
        if(!grid.CanSetGridValue(gridPos, newGridObject))
        {
            gridPosition = default;
            rotation = default;
            return false;
        }
            
        gridPosition = grid.GetWorldPosition(gridPos.x, gridPos.y, newGridObject);
        rotation = Quaternion.Euler(0f, currentDirection.Rotation, 0f);
        return true;
    }
}
