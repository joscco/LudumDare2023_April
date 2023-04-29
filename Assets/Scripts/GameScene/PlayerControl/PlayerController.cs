using System;
using DG.Tweening;
using GameScene.Buildings;
using UnityEngine;

namespace GameScene.PlayerControl
{
    public class PlayerController : MonoBehaviour
    {
        public StreetMap streetMap;
        public SupplierBuildingManager supplyBuildings;
        public ConsumerBuildingManager consumerBuildings;
        public Inventory.Inventory inventory;
        public SpriteRenderer spriteRenderer;

        private Vector2Int _currentIndex;
        private Tween _moveTween;
        private Tween _scaleTween;

        private void Start()
        {
            var sanitizedIndex = streetMap.grid.WorldToCell(transform.position);
            InstantUpdatePosition(new Vector2Int(sanitizedIndex.x, sanitizedIndex.y));

            StartShaking();
        }

        private void StartShaking()
        {
            _scaleTween = spriteRenderer.transform.DOScale(new Vector3(0.9f, 1.03f, 0.8f), 0.5f)
                .SetEase(Ease.InOutQuad)
                .SetLoops(-1, LoopType.Yoyo);
        }

        private void Update()
        {
            var move = CheckInput();

            if (Math.Abs(move.x) == 1)
            {
                HandleInput(new(move.x, 0));
            }
            else if (Math.Abs(move.y) == 1)
            {
                HandleInput(new(0, move.y));
            }
        }

        private Vector2Int CheckInput()
        {
            int horizontalMove = 0;
            int verticalMove = 0;

            if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
            {
                horizontalMove++;
            }

            if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
            {
                horizontalMove--;
            }

            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
            {
                verticalMove++;
            }

            if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
            {
                verticalMove--;
            }

            return new Vector2Int(horizontalMove, verticalMove);
        }

        private void HandleInput(Vector2Int move)
        {
            if (supplyBuildings.HasBuildingAt(_currentIndex + move))
            {
                Debug.Log("Found supplier");
                var building = supplyBuildings.GetBuildingAt(_currentIndex + move);
                if (building.CanSupply() && inventory.HasFreeSlot())
                {
                    building.TakeItem();
                    var itemType = inventory.GetItemForBuildingType(building.type);
                    inventory.AddItem(itemType);
                }
            } else if (consumerBuildings.HasBuildingAt(_currentIndex + move))
            {
                Debug.Log("Found Consumer");
                var building = consumerBuildings.GetBuildingAt(_currentIndex + move);
                if (building.demandsItem && inventory.HasItem(building.demandedItemType))
                {
                    inventory.RemoveItem(building.demandedItemType);
                    building.SetSatisfied();
                }
            } else if (streetMap.HasStreetAt(_currentIndex + move))
            {
                UpdatePosition(_currentIndex + move);
            }
        }
    
    
    
        private void InstantUpdatePosition(Vector2Int newIndex)
        {
            _currentIndex = newIndex;
            _moveTween?.Kill();
            transform.position = streetMap.IndexToPosition(newIndex);
        }

        private void UpdatePosition(Vector2Int newIndex)
        {
            _currentIndex = newIndex;
            _moveTween?.Kill();
            _moveTween = transform.DOMove(streetMap.IndexToPosition(newIndex), 0.15f)
                .SetEase(Ease.InOutBack);
        }
    }
}