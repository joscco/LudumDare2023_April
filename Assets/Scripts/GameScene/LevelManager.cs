using System;
using GameScene.BuildingMap;
using GameScene.Buildings;
using GameScene.PlayerControl;
using GameScene.UI;
using UnityEngine;

namespace GameScene
{
    public class LevelManager : MonoBehaviour
    {
        public int level;
        public int startActionsLeft;
        public int moneyNeeded;

        public bool paused;
        
        public InputManager InputManager;
        public PlayerController player;

        public ActionsLeftDisplay actionsLeftDisplay;
        public MoneyBar moneyBar;
        public WinScreen winScreen;
        public LoseScreen loseScreen;
        
        public StreetMap streetMap;
        public SupplierBuildingManager supplyBuildings;
        public ConsumerBuildingManager consumerBuildings;
        public TrafficLightManager trafficLights;
        public Inventory.Inventory inventory;
        
        private int _actionsLeft;
        private int _moneyAcquired;

        public void Start()
        {
            SetMoneyAcquired(0);
            moneyBar.SetMoneyNeeded(moneyNeeded);
            SetActionsLeft(startActionsLeft);
            SanitizePlayerPosition();
        }
        
        private void SanitizePlayerPosition()
        {
            var sanitizedIndex = streetMap.grid.WorldToCell(player.transform.position);
            var sanitizedIndex2D = new Vector2Int(sanitizedIndex.x, sanitizedIndex.y);
            var sanitizedPosition = streetMap.IndexToPosition(sanitizedIndex2D);
            player.InstantUpdatePosition(sanitizedIndex2D, sanitizedPosition);
        }
        
        private void SetActionsLeft(int actions)
        {
            _actionsLeft = actions;
            actionsLeftDisplay.SetActionsLeft(_actionsLeft);
        }

        private void SetMoneyAcquired(int money)
        {
            _moneyAcquired = money;
            moneyBar.SetMoneyAcquired(money);
        }

        public void StartLevel()
        {
            
        }

        private void Update()
        {
            if (!paused && !OptionScreen.instance.IsVisible())
            {
                var move = InputManager.CheckInput();
                if (move.x != 0 || move.y != 0)
                {
                    HandleInput(move);
                }
            }
        }

        private void RemoveAction()
        {
            SetActionsLeft(_actionsLeft - 1);
            trafficLights.NextStatus();
            CheckGameStatus();
        }

        private void CheckGameStatus()
        {
            if (_moneyAcquired >= moneyNeeded)
            {
                ShowWinScreen();
            } else if (_actionsLeft == 0)
            {
                ShowLoseScreen(LoseReason.NoActionsLeft);
            }
        }

        private void ShowLoseScreen(LoseReason reason)
        {
            paused = true;
            loseScreen.BlendIn(reason);
        }

        private void ShowWinScreen()
        {
            paused = true;
            Game.instance.SaveUnlockedLevel(level);
            winScreen.BlendIn();
        }

        private void HandleInput(Vector2Int move)
        {
            var _currentIndex = player.GetIndex();
            if (supplyBuildings.HasBuildingAt(player.GetIndex() + move))
            {
                Debug.Log("Found supplier");
                var building = supplyBuildings.GetBuildingAt(_currentIndex + move);
                if (building.CanSupply() && inventory.HasFreeSlot())
                {
                    building.TakeItem();
                    var itemType = inventory.GetItemForBuildingType(building.type);
                    inventory.AddItem(itemType);
                    RemoveAction();
                }
            } else if (consumerBuildings.HasBuildingAt(_currentIndex + move))
            {
                Debug.Log("Found Consumer");
                var building = consumerBuildings.GetBuildingAt(_currentIndex + move);
                if (building.demandsItem && inventory.HasItem(building.demandedItemType))
                {
                    inventory.RemoveItem(building.demandedItemType);
                    building.SetSatisfied();
                    RemoveAction();
                }
            } else if (streetMap.HasStreetAt(_currentIndex + move))
            {
                if (trafficLights.CanEnter(_currentIndex + move))
                {
                    player.UpdatePosition(_currentIndex + move, streetMap.IndexToPosition(_currentIndex + move));
                    RemoveAction();
                }
                // Add NO-Sound playing here!
            }
        }
    }
}