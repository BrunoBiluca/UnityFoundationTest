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
}
