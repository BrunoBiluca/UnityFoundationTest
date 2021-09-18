using Assets.Scenes.Systems.BuildingPlacementSystem;
using Assets.UnityFoundation.Code.ObjectPooling;

public class Building : PooledObject
{
    public GridObject GridObjectRef { get; private set; }

    public Building Setup(GridObject gridObject)
    {
        GridObjectRef = gridObject;
        return this;
    }
}
