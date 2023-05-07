using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GameScene.Dogs
{
    public class DogManager : MonoBehaviour
    {
        [SerializeField] private Grid grid;
        private List<Dog> _dogs;

        private void Start()
        {
            _dogs = new();
        }

        public List<Dog> GetFollowingDogs()
        {
            return _dogs.Where(dog => dog.GetStatus() == DogStatus.Following).ToList();
        }

        public List<Dog> GetHomeComingDogs()
        {
            return _dogs.Where(dog => dog.GetStatus() == DogStatus.Homecoming).ToList();
        }

        public List<Dog> GetAllActiveDogs()
        {
            return _dogs.Where(dog => dog.GetStatus() != DogStatus.Canceled).ToList();
        }

        public Vector2 IndexToPosition(Vector2Int index)
        {
            return grid.GetCellCenterWorld(indexWrap(index));
        }

        public Vector2 GetCellSize()
        {
            return grid.cellSize;
        }

        public Vector3Int indexWrap(Vector2Int index)
        {
            return new Vector3Int(index.x, index.y, 0);
        }

        public Vector2Int indexUnwrap(Vector3Int index)
        {
            return new Vector2Int(index.x, index.y);
        }

        public Dog GetDogAt(Vector2Int index)
        {
            return _dogs.First(dog => dog.GetIndex() == index);
        }

        public bool HasDogAt(Vector2Int index)
        {
            return _dogs.Any(dog => dog.GetIndex() == index);
        }

        public void AddDogAt(Dog dogInstance, Vector2Int dogDataPosition)
        {
            var startIndex = dogDataPosition;
            dogInstance.SetIndex(startIndex);
            dogInstance.SetStartIndex(startIndex);
            dogInstance.transform.position = grid.GetCellCenterWorld(indexWrap(dogInstance.GetIndex()));
        }
    }
}