using System.Collections.Generic;
using DG.Tweening;
using GameScene.BuildingMap;
using GameScene.Buildings;
using GameScene.Dogs;
using GameScene.Inventory;
using GameScene.PlayerControl;
using GameScene.UI;
using General;
using LevelDesign;
using UnityEngine;
using UnityEngine.Tilemaps;

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

        [SerializeField] private StreetGrid streetGrid;

        [SerializeField] private Tilemap lawnMap;
        [SerializeField] private RuleTile lawnTile;
        [SerializeField] private Tilemap riverMap;
        [SerializeField] private RuleTile riverTile;
        [SerializeField] private Tilemap streetMap;
        [SerializeField] private RuleTile streetTile;
        [SerializeField] private Tilemap decorationMap;
        [SerializeField] private Tile horizontalBridgeTile;
        [SerializeField] private Tile verticalBridgeTile;

        [SerializeField] private SupplierBuilding pizzaShopPrefab;
        [SerializeField] private SupplierBuilding sushiShopPrefab;
        [SerializeField] private SupplierBuilding burgerShopPrefab;
        [SerializeField] private SupplierBuilding weaponShopPrefab;
        [SerializeField] private SupplierBuilding drugShopPrefab;
        [SerializeField] private SupplierBuilding organShopPrefab;
        [SerializeField] private SupplierBuildingManager supplyBuildings;

        [SerializeField] private ConsumerBuilding littleConsumerPrefab;
        [SerializeField] private ConsumerBuilding mediumConsumerPrefab;
        [SerializeField] private ConsumerBuilding largeConsumerPrefab;
        [SerializeField] private ConsumerBuildingManager consumerBuildings;

        [SerializeField] private PoliceBuilding policeStationPrefab;
        [SerializeField] private PoliceManager policeManager;

        [SerializeField] private Dog dogPrefab;
        [SerializeField] private DogManager dogManager;

        [SerializeField] private TrafficLight trafficLightPrefab;
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
            InitLandscape(data);
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

        private void InitLandscape(LevelData levelData)
        {
            lawnMap.ClearAllTiles();
            riverMap.ClearAllTiles();
            streetMap.ClearAllTiles();
            decorationMap.ClearAllTiles();
            
            // Center grid position
            streetGrid.transform.position = - 0.5f * 100  * new Vector2(levelData.LevelDimensions.x, levelData.LevelDimensions.y);

            for (int x = 0; x < levelData.LevelDimensions.x; x++)
            {
                for (int y = 0; y < levelData.LevelDimensions.y; y++)
                {
                    lawnMap.SetTile(new Vector3Int(x, y, 0), lawnTile);
                }
            }

            foreach (var river in levelData.Rivers)
            {
                riverMap.SetTile(new Vector3Int(river.Position.x, river.Position.y), riverTile);
            }

            foreach (var street in levelData.Streets)
            {
                streetMap.SetTile(new Vector3Int(street.Position.x, street.Position.y), streetTile);
            }

            foreach (var bridge in levelData.Bridges)
            {
                switch (bridge.Type)
                {
                    case BridgeType.Horizontal:
                        decorationMap.SetTile(new Vector3Int(bridge.Position.x, bridge.Position.y), horizontalBridgeTile);
                        break;

                    case BridgeType.Vertical:
                        decorationMap.SetTile(new Vector3Int(bridge.Position.x, bridge.Position.y), verticalBridgeTile);
                        break;
                }
            }
        }

        private void InitInfrastructure(LevelData levelData)
        {
            foreach (var consumer in levelData.Consumers)
            {
                ConsumerBuilding building;
                switch (consumer.HouseType)
                {
                    case HouseType.Little:
                        building = Instantiate(littleConsumerPrefab, consumerBuildings.transform);
                        break;
                    case HouseType.Medium:
                        building = Instantiate(mediumConsumerPrefab, consumerBuildings.transform);
                        break;
                    default:
                        building = Instantiate(largeConsumerPrefab, consumerBuildings.transform);
                        break;
                }
                
                building.demandedItems = 1;
                building.demandedItemType = consumer.Type;
                building.InitHint(consumer.SignPosition, -building.demandedItems);
                consumerBuildings.AddBuildingAt(building, consumer.Position);
            }

            foreach (var supplier in levelData.Suppliers)
            {
                SupplierBuilding building;
                switch (supplier.Type)
                {
                    case ItemType.PIZZA:
                        building = Instantiate(pizzaShopPrefab, supplyBuildings.transform);
                        break;
                    case ItemType.BURGER:
                        building = Instantiate(burgerShopPrefab, supplyBuildings.transform);
                        break;
                    case ItemType.SUSHI:
                        building = Instantiate(sushiShopPrefab, supplyBuildings.transform);
                        break;
                    case ItemType.DRUGS:
                        building = Instantiate(drugShopPrefab, supplyBuildings.transform);
                        break;
                    case ItemType.WEAPON:
                        building = Instantiate(weaponShopPrefab, supplyBuildings.transform);
                        break;
                    default:
                        building = Instantiate(organShopPrefab, supplyBuildings.transform);
                        break;
                }
                
                building.itemsInSupply = supplier.SuppliedItems;
                building.suppliedItemType = supplier.Type;
                building.InitHint(supplier.SignPosition, building.itemsInSupply);
                supplyBuildings.AddBuildingAt(building, supplier.Position);
                
            }

            foreach (var trafficLight in levelData.TrafficLights)
            {
                var trafficLightInstance = Instantiate(trafficLightPrefab, trafficLights.transform);
                trafficLightInstance.status = trafficLight.StartStatus;
                trafficLights.AddLightAt(trafficLightInstance, trafficLight.Position);
            }
            
            foreach (var policeData in levelData.Polices)
            {
                var policeInstance = Instantiate(policeStationPrefab, policeManager.transform);
                policeManager.AddPoliceAt(policeInstance, policeData.Position);
            }
            
            foreach (var dogData in levelData.Dogs)
            {
                var dogInstance = Instantiate(dogPrefab, dogManager.transform);
                dogManager.AddDogAt(dogInstance, dogData.Position);
            }
        }

        private void InitPlayerPosition(LevelData levelData)
        {
            var sanitizedIndex = levelData.playerStartPosition;
            var sanitizedPosition = streetGrid.IndexToPosition(sanitizedIndex);
            player.InstantUpdatePosition(sanitizedIndex, sanitizedPosition);
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
                else
                {
                    building.HideItem();
                }
            }

            foreach (var building in consumerBuildings.GetBuildings())
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
                var nextIndex = streetGrid.MoveOnStreetTowards(followingDog.GetIndex(), player.GetIndex());
                followingDog.UpdatePosition(nextIndex, streetGrid.IndexToPosition(nextIndex));
            }

            // Move all homecoming dogs
            foreach (var homeComingDog in dogManager.GetHomeComingDogs())
            {
                var nextIndex = streetGrid.MoveOnStreetTowards(homeComingDog.GetIndex(), homeComingDog.GetStartIndex());
                homeComingDog.UpdatePosition(nextIndex, streetGrid.IndexToPosition(nextIndex));
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
                        var itemType = building.GetItemType();
                        var sprite = spriteWiki.FindSpriteForType(itemType);
                        inventory.AddItem(itemType, sprite);
                        AudioManager.instance.PlayMoveSound();
                        RemoveAction();
                    }
                }
            }
            else if (consumerBuildings.HasBuildingAt(currentIndex + move))
            {
                var building = consumerBuildings.GetBuildingAt(currentIndex + move);
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
            else if (streetGrid.HasStreetAt(currentIndex + move))
            {
                if (trafficLights.CanEnter(currentIndex + move))
                {
                    player.UpdatePosition(currentIndex + move, streetGrid.IndexToPosition(currentIndex + move));
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