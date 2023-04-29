using UnityEngine;

namespace GameScene.Buildings
{
    public abstract class Building: MonoBehaviour
    {
        public BuildingType type;

        // Will be set at start by grid
        private Vector2Int _index;

        public void SetIndex(Vector2Int index)
        {
            _index = index;
        }

        public Vector2Int GetIndex()
        {
            return _index;
        }
    }

    public enum BuildingType
    {
        HOUSE, PIZZA, SUSHI, BURGER, TACO, HOSPITAL, POLICE, DRUG_DEALER
    }
}