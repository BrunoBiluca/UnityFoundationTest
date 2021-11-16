using Assets.UnityFoundation.Code.Common;
using Assets.UnityFoundation.Code.Grid;
using Assets.UnityFoundation.Systems.PathFinder;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;

public class PathFindingDemoManager : MonoBehaviour
{

    [SerializeField] private int gridWidth;
    [SerializeField] private int gridHeight;

    GridXYDebug grid;

    List<Vector3> mouseInputs;

    PathFinding pathFindingGrid;

    void Start()
    {
        grid = new GridXYDebug(new GridXY(gridWidth, gridHeight));
        grid.Display();
        pathFindingGrid = new PathFinding(new Int2(gridWidth, gridHeight), debug: false);

        mouseInputs = new List<Vector3>();
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            SelectGridPosition();
        }

        if(Input.GetMouseButtonDown(1))
        {
            ToogleBlockGridPosition();
        }
    }

    private void ToogleBlockGridPosition()
    {
        var mouse = Input.mousePosition;
        var worldPosition = Camera.main.ScreenToWorldPoint(mouse);
        if(grid.TrySetNodeValue(worldPosition, "###"))
            pathFindingGrid.SetIsWalkable(grid.GetGridPostion(worldPosition), false);
    }

    private void SelectGridPosition()
    {
        var mouse = Input.mousePosition;
        var worldPosition = Camera.main.ScreenToWorldPoint(mouse);

        if(!grid.TrySetNodeValue(worldPosition, "32"))
            return;

        mouseInputs.Add(worldPosition);
        if(mouseInputs.Count == 2)
        {
            var startPos = grid.GetGridPostion(mouseInputs.First());
            var endPos = grid.GetGridPostion(mouseInputs.Last());
            var path = pathFindingGrid.FindPath(new Int2(startPos), new Int2(endPos));

            mouseInputs.Clear();
            if(path.Count() == 1)
            {
                Debug.Log("Caminho não encontrado");
                return;
            }

            foreach(var p in path)
            {
                Debug.Log(p);
            }

            grid.DrawLines(path.ToArray());
        }
    }
}
