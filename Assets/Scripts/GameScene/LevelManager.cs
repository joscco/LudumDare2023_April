using System;
using System.Collections.Generic;
using DefaultNamespace;
using DG.Tweening;
using GameScene.BuildingMap;
using GameScene.Buildings;
using GameScene.Dogs;
using GameScene.Inventory;
using GameScene.PlayerControl;
using GameScene.UI;
using General;
using UnityEngine;
using UnityEngine.Serialization;

namespace GameScene
{
    public class LevelManager : MonoBehaviour, SceneManager
    {
        public int level;
        public int startActionsLeft;
        public int moneyNeeded;
        public List<string> introTexts;
        public bool showMafiaGuyOnStart;

        public bool paused;
        
        public InputManager inputManager;
        public PlayerController player;
        public DialogManager dialogManager;

        public ActionsLeftDisplay actionsLeftDisplay;
        public MoneyBar moneyBar;
        public WinScreen winScreen;
        public LoseScreen loseScreen;

        public StreetMap streetMap;
        public SupplierBuildingManager supplyBuildings;
        public ConsumerBuildingManager consumerBuildings;
        public PoliceManager policeManager;
        public DogManager dogManager;
        public TrafficLightManager trafficLights;
        public Inventory.Inventory inventory;
        public InventorySpriteWiki spriteWiki;
        
        private int _actionsLeft;
        private int _moneyAcquired;

        public void Start()
        {
            moneyBar.SetMoneyNeeded(moneyNeeded);
            SetMoneyAcquired(0);
            SetActionsLeft(startActionsLeft);
            SanitizePlayerPosition();
        }
        
        public void AfterFade()
        {
            ShowOrHideItemHints();
            if (introTexts.Count > 0)
            {
                dialogManager.AddDialog(introTexts, showMafiaGuyOnStart);
            }
        }

        private void ShowOrHideItemHints()
        {
            foreach (var building in supplyBuildings.GetBuildings())
            {
                    if (building.NeedsHint())
                    {
                        building.itemHint.SetSprite(spriteWiki.FindSpriteForType(building.GetItemType()));
                        building.BlendInHint();
                    }
            }
            
            foreach (var building in consumerBuildings.GetBuildings())
            {
                if (building.NeedsHint())
                {
                    building.itemHint.SetSprite(spriteWiki.FindSpriteForType(building.GetItemType()));
                    building.BlendInHint();
                }
            }
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
        
        private void TweenMoneyAcquired(int money)
        {
            _moneyAcquired = money;
            moneyBar.TweenMoneyAcquired(money);
        }

        private void AddMoney(int amount)
        {
            TweenMoneyAcquired(_moneyAcquired + amount);
        }

        private void Update()
        {
            if (!paused && !OptionScreen.instance.IsVisible())
            {
                var move = inputManager.CheckInput();
                if (move.x != 0 || move.y != 0)
                {
                    HandleInput(move);
                }
                
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    dialogManager.SkipCurrentDialog();
                }
            }
        }

        private void RemoveAction()
        {
            SetActionsLeft(_actionsLeft - 1);
            trafficLights.NextStatus();
            inventory.NextStatus();
            ProceedDogs();
            CheckGameStatus();
        }

        private void ProceedDogs()
        {
            // Move all following dogs
            foreach (var followingDog in dogManager.GetFollowingDogs())
            {
                var nextIndex = streetMap.MoveOnStreetTowards(followingDog.GetIndex(), player.GetIndex());
                followingDog.UpdatePosition(nextIndex, streetMap.IndexToPosition(nextIndex));
            }
            
            // Move all homecoming dogs
            foreach (var homeComingDog in dogManager.GetHomeComingDogs())
            {
                var nextIndex = streetMap.MoveOnStreetTowards(homeComingDog.GetIndex(), homeComingDog.GetStartIndex());
                homeComingDog.UpdatePosition(nextIndex, streetMap.IndexToPosition(nextIndex));
            }
            
            // Check if dog has reached player
            if (inventory.HoldsFood() && dogManager.HasDogAt(player.GetIndex()))
            {
                var dog = dogManager.GetDogAt(player.GetIndex());
                AudioManager.instance.PlayDogBark();
                inventory.RemoveFirstFood();
                dog.Cancel();
            }
            
            
            // Check which dogs can be released
            if (!inventory.HoldsFood())
            {
                foreach (var followingDog in dogManager.GetFollowingDogs())
                {
                    followingDog.GoHome();
                }
            }
            else
            {
                // Check which dogs need to start following last!
                foreach (var dog in dogManager.GetAllActiveDogs())
                {
                    if (dog.WatchesIndex(player.GetIndex()) && dog.GetStatus() != DogStatus.Following)
                    {
                        AudioManager.instance.PlayDogBark();
                        dog.StartFollowing();
                    }
                }
            }
        }

        private void CheckGameStatus()
        {
            if (inventory.HasBadHeart())
            {
                ShowLoseScreen(LoseReason.Mafia);
            } 
            else if (inventory.HoldsIllegalItem() && policeManager.WatchesIndex(player.GetIndex()))
            {
                policeManager.ShowAlarms();
                AudioManager.instance.PlayAlarmSound();
                paused = true;
                DOVirtual.DelayedCall(0.6f, () =>
                {
                    ShowLoseScreen(LoseReason.Police);
                });
            }
            else if (_moneyAcquired >= moneyNeeded)
            {
                ShowWinScreen();
            }
            else if (_actionsLeft == 0)
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
            var currentIndex = player.GetIndex();
            if (supplyBuildings.HasBuildingAt(player.GetIndex() + move))
            {
                var building = supplyBuildings.GetBuildingAt(currentIndex + move);
                if (building.CanSupply() && inventory.HasFreeSlot())
                {
                    if ((spriteWiki.IsIllegal(building.GetItemType()) && inventory.HoldsFood()) 
                        || (!spriteWiki.IsIllegal(building.GetItemType()) && inventory.HoldsIllegalItem()))
                    {
                        AudioManager.instance.PlayNoSound();
                        dialogManager.ShowYouCannotHoldFoodAndIllegalStuff();
                    }
                    else
                    {
                        building.TakeItem();
                        var itemType = spriteWiki.FindItemForSupplier(building.type);
                        var sprite = spriteWiki.FindSpriteForType(itemType);
                        inventory.AddItem(itemType, sprite);
                        AudioManager.instance.PlayMoveSound();
                        RemoveAction();
                    }
                    
                }
            } else if (consumerBuildings.HasBuildingAt(currentIndex + move))
            {
                var building = consumerBuildings.GetBuildingAt(currentIndex + move);
                if (building.demandsItem && inventory.HasItem(building.demandedItemType))
                {
                    var type = building.demandedItemType;
                    var freshness = inventory.RemoveItem(type);
                    building.SetSatisfied();
                    AddMoney(spriteWiki.GetPriceForItem(type, freshness));
                    AudioManager.instance.PlayMoveSound();
                    RemoveAction();
                }
            } else if (streetMap.HasStreetAt(currentIndex + move))
            {
                if (trafficLights.CanEnter(currentIndex + move))
                {
                    player.UpdatePosition(currentIndex + move, streetMap.IndexToPosition(currentIndex + move));
                    AudioManager.instance.PlayBlub();
                    RemoveAction();
                }
                // Add NO-Sound playing here!
            }
        }
    }
}