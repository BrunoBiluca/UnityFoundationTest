using UnityEngine;

namespace Assets.Scenes.Systems.BuildingPlacementSystem
{
    [CreateAssetMenu(fileName = "new_object", menuName = "BuildingPlacementSystem/Object")]
    public class GridObjectSO : ScriptableObject
    {
        [SerializeField] private int width;
        [SerializeField] private int height;
        [SerializeField] private GameObject objPrefab;

        public int Width => width;
        public int Height => height;
        public GameObject Prefab => objPrefab;

        public override string ToString()
        {
            return objPrefab.name;
        }
    }
}
