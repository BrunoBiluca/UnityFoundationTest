using Assets.Scenes.Systems.BuildingPlacementSystem;
using Assets.UnityFoundation.Systems.BuildingPlacementSystem;
using UnityEngine;
using UnityEngine.UI;

public class BuildingOption : MonoBehaviour
{
    [SerializeField] private GridObjectSO gridObject;

    private void Start()
    {
        GetComponent<Button>()
            .onClick
            .AddListener(() => {
                BuildingPlacementSystem.Instance.SetCurrentBuilding(gridObject);
            });
    }
}
