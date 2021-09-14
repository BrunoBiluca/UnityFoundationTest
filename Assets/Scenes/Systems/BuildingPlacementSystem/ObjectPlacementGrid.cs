using Assets.UnityFoundation.Code.Grid;
using UnityEngine;

namespace Assets.Scenes.Systems.BuildingPlacementSystem
{

    public class GridObject
    {
        private readonly int width;
        private readonly int height;
        private readonly GridObjectDirection direction;

        public int Width => width;
        public int Height => height;
        public GridObjectDirection Direction => direction;

        public GridObject(int width, int height, GridObjectDirection direction)
        {
            this.width = width;
            this.height = height;
            this.direction = direction;
        }

        public GridObject(GridObjectSO gridObjectSO, GridObjectDirection direction)
            : this(gridObjectSO.Width, gridObjectSO.Height, direction)
        {
        }

        public override string ToString()
        {
            return $"[{Width},{Height},{Direction}]";
        }
    }

    public class ObjectPlacementGrid : GridXZ<GridObject>
    {
        public ObjectPlacementGrid(int width, int height, int cellSize)
            : base(width, height, cellSize)
        {
        }

        public override bool TrySetGridValue(Vector3 position, GridObject value)
        {
            var gridPosition = GetGridPostion(position);

            var objectDimensionX = gridPosition.x + value.Width;
            var objectDimensionY = gridPosition.y + value.Height;

            if(value.Direction == GridObjectDirection.LEFT
                || value.Direction == GridObjectDirection.RIGHT)
            {
                objectDimensionX = gridPosition.x + value.Height;
                objectDimensionY = gridPosition.y + value.Width;
            }

            for(int x = gridPosition.x; x < objectDimensionX; x++)
                for(int y = gridPosition.y; y < objectDimensionY; y++)
                    if(IsOccupied(x, y))
                        return false;

            for(int x = gridPosition.x; x < objectDimensionX; x++)
                for(int y = gridPosition.y; y < objectDimensionY; y++)
                    gridArray[x, y].Value = value;

            return true;
        }
    }
}
