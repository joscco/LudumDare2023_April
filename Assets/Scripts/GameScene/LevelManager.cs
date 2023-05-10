using System.Collections.Generic;
using DG.Tweening;
using GameScene.Buildings;
using GameScene.Buildings.Consumers;
using GameScene.Dogs;
using GameScene.Inventory;
using GameScene.Obstacles.Dogs;
using GameScene.Obstacles.TrafficLights;
using GameScene.PlayerControl;
using GameScene.StreetMap;
using GameScene.UI;
using General;
using LevelDesign;
using UnityEngine;

namespace GameScene
{
    public class LevelManager : MonoBehaviour
    {
        public List<LevelData> levels;

        [SerializeField] private InputManager inputManager;
        [SerializeField] private PlayerController player;
        [SerializeField] private DialogManager dialogManager;

        [SerializeField] private ActionsLeftDisplay actionsLeftDisplay;
        [SerializeField] private MoneyBar moneyBar;
        [SerializeField] private WinScreen winScreen;
        [SerializeField] private LoseScreen loseScreen;

        [SerializeField] private LevelGrid levelGrid;
        [SerializeField] private SupplierBuildingManager supplyBuildingsManager;
        [SerializeField] private ConsumerBuildingManager consumerBuildingsManager;
        [SerializeField] private PoliceManager policeManager;
        [SerializeField] private DogManager dogManager;
        [SerializeField] private TrafficLightManager trafficLights;
        [SerializeField] private Inventory.Inventory inventory;
        [SerializeField] private InventorySpriteWiki spriteWiki;

        private int _level;
        private bool _paused;
        private int _actionsLeft;
        private int _moneyAcquired;
        private int _moneyNeeded;

        public void StartLevel(int level)
        {
            _level = level;
            var data = levels.Find(data => data.level == level);

            InitBasicData(data);
            levelGrid.InitLandscape(data.LevelDimensions, data.Rivers, data.Bridges, data.Streets);
            InitInfrastructure(data);
            InitPlayerPosition(data);

            ShowOrHideItemHints();
            StartDialog(data);
        }
        
        private void Update()
        {
            if (!SceneTransitionManager.Get().IsInTransition())
            {
                if (Input.GetKeyDown(KeyCode.O))
                {
                    OptionScreen.instance.Toggle();
                }
                
                if (Input.GetKeyDown(KeyCode.R))
                {
                    SceneTransitionManager.Get().ReloadCurrentLevel();
                    OptionScreen.instance.BlendOut();
                }
            }
            
            
            if (!_paused && !OptionScreen.instance.IsVisible())
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

        private void InitBasicData(LevelData levelData)
        {
            SetActionsLeft(levelData.startActionsLeft);
            SetMoneyNeeded(levelData.moneyNeeded);
            SetMoneyAcquired(0);
        }

        private void InitInfrastructure(LevelData levelData)
        {
            consumerBuildingsManager.InitConsumers(levelData.Consumers);
            supplyBuildingsManager.InitSuppliers(levelData.Suppliers);
            trafficLights.InitTrafficLights(levelData.TrafficLights);
            policeManager.InitPoliceDepartments(levelData.Polices);
            dogManager.InitDogs(levelData.Dogs);
        }

        private void InitPlayerPosition(LevelData levelData)
        {
            var sanitizedIndex = levelData.playerStartPosition;
            var sanitizedPosition = levelGrid.IndexToPosition(sanitizedIndex);
            player.InstantUpdatePosition(sanitizedIndex, sanitizedPosition);
        }

        private void ShowOrHideItemHints()
        {
            foreach (var building in supplyBuildingsManager.GetBuildings())
            {
                if (building.NeedsHint())
                {
                    building.itemHint.SetSprite(spriteWiki.FindSpriteForType(building.GetItemType()));
                    building.BlendInHint();
                }
                else
                {
                    building.HideItem();
                }
            }

            foreach (var building in consumerBuildingsManager.GetBuildings())
            {
                if (building.NeedsHint())
                {
                    building.itemHint.SetSprite(spriteWiki.FindSpriteForType(building.GetItemType()));
                    building.BlendInHint();
                } else
                {
                    building.HideItem();
                }
            }
        }

        private void StartDialog(LevelData levelData)
        {
            if (levelData.IntroTexts.Count > 0)
            {
                dialogManager.AddDialog(levelData.IntroTexts);
            }
        }

        private void SetActionsLeft(int actions)
        {
            _actionsLeft = actions;
            actionsLeftDisplay.SetActionsLeft(_actionsLeft);
        }

        private void SetMoneyNeeded(int money)
        {
            _moneyNeeded = money;
            moneyBar.SetMoneyNeeded(_moneyNeeded);
        }

        private void SetMoneyAcquired(int money)
        {
            _moneyAcquired = money;
            moneyBar.SetMoneyAcquired(_moneyAcquired);
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
                var nextIndex = levelGrid.MoveOnStreetTowards(followingDog.GetIndex(), player.GetIndex());
                followingDog.UpdatePosition(nextIndex, levelGrid.IndexToPosition(nextIndex));
            }

            // Move all homecoming dogs
            foreach (var homeComingDog in dogManager.GetHomeComingDogs())
            {
                var nextIndex = levelGrid.MoveOnStreetTowards(homeComingDog.GetIndex(), homeComingDog.GetStartIndex());
                homeComingDog.UpdatePosition(nextIndex, levelGrid.IndexToPosition(nextIndex));
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
                _paused = true;
                DOVirtual.DelayedCall(0.6f, () => { ShowLoseScreen(LoseReason.Police); });
            }
            else if (_moneyAcquired >= _moneyNeeded)
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
            _paused = true;
            loseScreen.BlendIn(reason);
        }

        private void ShowWinScreen()
        {
            _paused = true;
            Game.instance.SaveUnlockedLevel(_level);
            winScreen.BlendIn();
        }

        private void HandleInput(Vector2Int move)
        {
            var currentIndex = player.GetIndex();
            if (supplyBuildingsManager.HasBuildingAt(player.GetIndex() + move))
            {
                var building = supplyBuildingsManager.GetBuildingAt(currentIndex + move);
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
                        var itemType = building.GetItemType();
                        var sprite = spriteWiki.FindSpriteForType(itemType);
                        inventory.AddItem(itemType, sprite);
                        AudioManager.instance.PlayMoveSound();
                        RemoveAction();
                    }
                }
            }
            else if (consumerBuildingsManager.HasBuildingAt(currentIndex + move))
            {
                var building = consumerBuildingsManager.GetBuildingAt(currentIndex + move);
                if (building.NeedsHint() && inventory.HasItem(building.demandedItemType))
                {
                    var type = building.demandedItemType;
                    var freshness = inventory.RemoveItem(type);
                    building.SetSatisfied();
                    AddMoney(spriteWiki.GetPriceForItem(type, freshness));
                    AudioManager.instance.PlayMoveSound();
                    RemoveAction();
                }
            }
            else if (levelGrid.HasStreetAt(currentIndex + move))
            {
                if (trafficLights.CanEnter(currentIndex + move))
                {
                    player.UpdatePosition(currentIndex + move, levelGrid.IndexToPosition(currentIndex + move));
                    AudioManager.instance.PlayBlub();
                    RemoveAction();
                }
                else
                {
                    AudioManager.instance.PlayNoSound();
                }
            }
        }

        public bool IsPaused()
        {
            return _paused;
        }
    }
}