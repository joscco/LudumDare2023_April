using System;
using DG.Tweening;
using GameScene.BuildingMap;
using GameScene.Inventory;
using GameScene.PlayerControl;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerController : MonoBehaviour
{
    public StreetMap streetMap;
    [FormerlySerializedAs("buildingsMap")] public BuildingManager buildingManager;
    public Inventory inventory;
        
    public Vector2Int startIndex = new(0, 0);
    public Vector2Int currentIndex;
    private Tween _moveTween;

    private void Start()
    {
        InstantUpdatePosition(startIndex);
    }

    private void Update()
    {
        var move = CheckInput();

        if (Math.Abs(move.x) == 1)
        {
            Move(new(move.x, 0));
        }
        else if (Math.Abs(move.y) == 1)
        {
            Move(new(0, move.y));
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

    private void Move(Vector2Int move)
    {
        if (buildingManager.HasBuildingAt(currentIndex + move))
        {
            var building = buildingManager.GetBuildingAt(currentIndex + move);
            if (building.CanSupply() && inventory.HasFreeSlot())
            {
                building.TakeItem();
                var itemType = inventory.GetItemForBuildingType(building.type);
                inventory.AddItem(itemType);
            }
        } else if (streetMap.HasStreetAt(currentIndex + move))
        {
            UpdatePosition(currentIndex + move);
        }
    }
    
    
    
    private void InstantUpdatePosition(Vector2Int newIndex)
    {
        currentIndex = newIndex;
        _moveTween?.Kill();
        transform.position = streetMap.IndexToPosition(newIndex);
    }

    private void UpdatePosition(Vector2Int newIndex)
    {
        currentIndex = newIndex;
        _moveTween?.Kill();
        _moveTween = transform.DOMove(streetMap.IndexToPosition(newIndex), 0.1f)
            .SetEase(Ease.InOutQuad);
    }
}